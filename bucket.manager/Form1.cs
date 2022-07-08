/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Forge Partner Development
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

using Autodesk.Forge;
using Autodesk.Forge.Model;
using bucket.manager.Utils;
using CefSharp;
using CefSharp.WinForms;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bucket.manager
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      InitBrowser();
    }

    public ChromiumWebBrowser browser;

    public void InitBrowser()
    {
      var settings = new CefSettings();
      //settings.CefCommandLineArgs.Add("enable-gpu", "1");
      //settings.CefCommandLineArgs.Add("enable-webgl", "1");
      Cef.Initialize(settings);
      browser = new ChromiumWebBrowser("file:///HTML/Viewer.html"); // CefSharp needs a initial page...

      browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
          | System.Windows.Forms.AnchorStyles.Left)
          | System.Windows.Forms.AnchorStyles.Right)));
      browser.MinimumSize = new System.Drawing.Size(20, 20);
      browser.Name = "webBrowser1";
      browser.TabIndex = 1;
      browser.Dock = DockStyle.Fill;
      panel1.Controls.Add(browser);
    }

    private Timer _tokenTimer = new Timer();
    private Timer _translationTimer = new Timer();
    private DateTime _expiresAt;

    private async void btnAuthenticate_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(txtClientId.Text) || string.IsNullOrWhiteSpace(txtClientSecret.Text)) return;

      try
      {
        // get the access token
        TwoLeggedApi oAuth = new TwoLeggedApi();
        Bearer token = (await oAuth.AuthenticateAsync(
          txtClientId.Text,
          txtClientSecret.Text,
          oAuthConstants.CLIENT_CREDENTIALS,
          new Scope[] { Scope.BucketRead, Scope.BucketCreate, Scope.DataRead, Scope.DataWrite })).ToObject<Bearer>();
        txtAccessToken.Text = token.AccessToken;
        _expiresAt = DateTime.Now.AddSeconds(token.ExpiresIn.Value);

        // keep track on time
        _tokenTimer.Tick += new EventHandler(tickTokenTimer);
        _tokenTimer.Interval = 1000;
        _tokenTimer.Enabled = true;

        btnRefreshToken_Click(null, null);
      } catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception when authenticating", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    void tickTokenTimer(object sender, EventArgs e)
    {
      // update the time left on the access token
      double secondsLeft = (_expiresAt - DateTime.Now).TotalSeconds;
      txtTimeout.Text = secondsLeft.ToString("0");
      txtTimeout.BackColor = (secondsLeft < 60 ? System.Drawing.Color.Red : System.Drawing.SystemColors.Control);
    }

    private string AccessToken
    {
      get
      {
        return txtAccessToken.Text;
      }
    }

    private async void btnRefreshToken_Click(object sender, EventArgs e)
    {
      treeBuckets.Nodes.Clear();

      try
      {
        BucketsApi bucketApi = new BucketsApi();
        bucketApi.Configuration.AccessToken = AccessToken;

        // control GetBucket pagination
        string lastBucket = null;

        Buckets buckets = null;
        do
        {
          buckets = (await bucketApi.GetBucketsAsync(cmbRegion.Text, 100, lastBucket)).ToObject<Buckets>();
          foreach (var bucket in buckets.Items)
          {
            TreeNode nodeBucket = new TreeNode(bucket.BucketKey);
            nodeBucket.Tag = bucket.BucketKey;
            treeBuckets.Nodes.Add(nodeBucket);
            lastBucket = bucket.BucketKey; // after the loop, this will contain the last bucketKey
          }
        } while (buckets.Items.Count > 0);

        // for each bucket, show the objects
        foreach (TreeNode n in treeBuckets.Nodes)
          if (n != null) // async?
            await ShowBucketObjects(n);
      } catch (Exception ex)
			{
        MessageBox.Show(ex.Message, "Exception when refreshing token", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private async Task ShowBucketObjects(TreeNode nodeBucket)
    {
      nodeBucket.Nodes.Clear();

      ObjectsApi objects = new ObjectsApi();
      objects.Configuration.AccessToken = AccessToken;

      // show objects on the given TreeNode
      BucketObjects objectsList = (await objects.GetObjectsAsync((string)nodeBucket.Tag)).ToObject<BucketObjects>();
      foreach (var objInfo in objectsList.Items)
      {
        TreeNode nodeObject = new TreeNode(objInfo.ObjectKey);
        nodeObject.Tag = ((string)objInfo.ObjectId).Base64Encode();
        nodeBucket.Nodes.Add(nodeObject);
      }
    }

    private const int UPLOAD_CHUNK_SIZE = 2; // Mb

    private async void btnUpload_Click(object sender, EventArgs e)
    {
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 0)
      {
        MessageBox.Show("Please select a bucket", "Bucket required", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {
        string bucketKey = treeBuckets.SelectedNode.Text;

        // ask user to select file
        OpenFileDialog formSelectFile = new OpenFileDialog();
        formSelectFile.Multiselect = false;
        if (formSelectFile.ShowDialog() != DialogResult.OK) return;
        string filePath = formSelectFile.FileName;
        string objectKey = Path.GetFileName(filePath);

        ObjectsApi objects = new ObjectsApi();
        objects.Configuration.AccessToken = AccessToken;

        // get file size
        long fileSize = (new FileInfo(filePath)).Length;

        // show progress bar for upload
        progressBar.DisplayStyle = ProgressBarDisplayText.CustomText;
        progressBar.Show();
        progressBar.Value = 0;
        progressBar.Minimum = 0;
        progressBar.CustomText = "Preparing to upload file...";

        // decide if upload direct or resumable (by chunks)
        if (fileSize > UPLOAD_CHUNK_SIZE * 1024 * 1024) // upload in chunks
        {
          long chunkSize = 2 * 1024 * 1024; // 2 Mb
          long numberOfChunks = (long)Math.Round((double)(fileSize / chunkSize)) + 1;

          progressBar.Maximum = (int)numberOfChunks;

          long start = 0;
          chunkSize = (numberOfChunks > 1 ? chunkSize : fileSize);
          long end = chunkSize;
          string sessionId = Guid.NewGuid().ToString();

          // upload one chunk at a time
          using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
          {
            for (int chunkIndex = 0; chunkIndex < numberOfChunks; chunkIndex++)
            {
              string range = string.Format("bytes {0}-{1}/{2}", start, end, fileSize);

              long numberOfBytes = chunkSize + 1;
              byte[] fileBytes = new byte[numberOfBytes];
              MemoryStream memoryStream = new MemoryStream(fileBytes);
              reader.BaseStream.Seek((int)start, SeekOrigin.Begin);
              int count = reader.Read(fileBytes, 0, (int)numberOfBytes);
              memoryStream.Write(fileBytes, 0, (int)numberOfBytes);
              memoryStream.Position = 0;

              dynamic chunkUploadResponse = await objects.UploadChunkAsync(bucketKey, objectKey, (int)numberOfBytes, range, sessionId, memoryStream);

              start = end + 1;
              chunkSize = ((start + chunkSize > fileSize) ? fileSize - start - 1 : chunkSize);
              end = start + chunkSize;

              progressBar.CustomText = string.Format("{0} Mb uploaded...", (chunkIndex * chunkSize) / 1024 / 1024);
              progressBar.Value = chunkIndex;
            }
          }
        } else // upload in a single call
        {
          using (StreamReader streamReader = new StreamReader(filePath))
          {
            progressBar.Value = 50; // random...
            progressBar.Maximum = 100;
            dynamic uploadedObj = await objects.UploadObjectAsync(bucketKey,
                   objectKey, (int)streamReader.BaseStream.Length, streamReader.BaseStream,
                   "application/octet-stream");
          }

        }

        progressBar.Hide();
        await ShowBucketObjects(treeBuckets.SelectedNode);
        treeBuckets.SelectedNode.Expand();
      } catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception when uploading", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      // authenticate when starts
      btnAuthenticate_Click(null, null);
    }

    private void btnTranslate_Click(object sender, EventArgs e)
    {
      // show menu with translation options
      // ToDo: include other translation formats
      menuTranslate.Items.Clear();
      menuTranslate.Items.Add("Viewer (SVF)", null, onClickTranslate);
      menuTranslate.Show(btnTranslate, new Point(0, btnTranslate.Height));
    }

    private async void onClickTranslate(object sender, EventArgs e)
    {
      // for now, just one translation at a time
      if (_translationTimer.Enabled) return;

      // check level 1 of objects
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 1)
      {
        MessageBox.Show("Please select an object", "Objects required", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {
        string urn = (string)treeBuckets.SelectedNode.Tag;

        // prepare a SVF translation
        List<JobPayloadItem> outputs = new List<JobPayloadItem>()
      {
       new JobPayloadItem(
         JobPayloadItem.TypeEnum.Svf,
         new List<JobPayloadItem.ViewsEnum>()
         {
           JobPayloadItem.ViewsEnum._2d,
           JobPayloadItem.ViewsEnum._3d
         })
      };
        JobPayload job;
        //if (string.IsNullOrEmpty(objModel.rootFilename))
        job = new JobPayload(new JobPayloadInput(urn), new JobPayloadOutput(outputs));
        //else
        //  job = new JobPayload(new JobPayloadInput(objModel.objectKey, true, objModel.rootFilename), new JobPayloadOutput(outputs));

        // start progress bar for translation
        progressBar.Show();
        progressBar.Value = 0;
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.CustomText = "Starting translation job...";

        // start translation job
        DerivativesApi derivative = new DerivativesApi();
        derivative.Configuration.AccessToken = AccessToken;
        dynamic jobPosted = await derivative.TranslateAsync(job, true);

        // start a monitor job to follow the translation
        _translationTimer.Tick += new EventHandler(isTranslationReady);
        _translationTimer.Tag = urn;
        _translationTimer.Interval = 5000;
        _translationTimer.Enabled = true;
      } catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception when translating", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private async void isTranslationReady(object sender, EventArgs e)
    {
      DerivativesApi derivative = new DerivativesApi();
      derivative.Configuration.AccessToken = AccessToken;

      // get the translation manifest
      dynamic manifest = await derivative.GetManifestAsync((string)_translationTimer.Tag);
      int progress = (string.IsNullOrWhiteSpace(Regex.Match(manifest.progress, @"\d+").Value) ? 100 : Int32.Parse(Regex.Match(manifest.progress, @"\d+").Value));

      // for better UX, show a small number of progress (instead zero)
      progressBar.Value = (progress == 0 ? 10 : progress);
      progressBar.CustomText = string.Format("Translation in progress: {0}", progress);
      Debug.WriteLine(progress);

      // if ready, hide everything
      if (progress >= 100)
      {
        progressBar.Hide();
        _translationTimer.Enabled = false;
      }
    }

    private async void btnDeleteObject_Click(object sender, EventArgs e)
    {
      // treeBuckets level 1 are for Objects
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 1)
      {
        MessageBox.Show("Please select an object", "Objects required", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {
        if (MessageBox.Show("This objects will be permantly delete, confirm?", "Are you sure?",
          MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;

        // call API to delete object on the bucket
        ObjectsApi objects = new ObjectsApi();
        objects.Configuration.AccessToken = AccessToken;
        await objects.DeleteObjectAsync((string)treeBuckets.SelectedNode.Parent.Tag, (string)treeBuckets.SelectedNode.Text);
        await ShowBucketObjects(treeBuckets.SelectedNode.Parent);
      } catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception when deleting", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      // required by CefSharp Browser
      Cef.Shutdown();
    }

    private void treeBuckets_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 1) return;

      // this basic HTML page to show the model passing URN & Access Token
      browser.Load(string.Format("file:///HTML/Viewer.html?URN={0}&Token={1}", treeBuckets.SelectedNode.Tag, AccessToken));
    }

    private async void btnCreateBucket_Click(object sender, EventArgs e)
    {
      string bucketKey = string.Empty;
      if (Prompt.ShowDialog("Enter bucket name: ", "Create new bucket", txtClientId.Text.ToLower() + DateTime.Now.Ticks.ToString(), out bucketKey) == DialogResult.OK)
      {
        try
        {
          BucketsApi buckets = new BucketsApi();
          buckets.Configuration.AccessToken = AccessToken;
          PostBucketsPayload bucketPayload = new PostBucketsPayload(bucketKey.ToLower(), null, PostBucketsPayload.PolicyKeyEnum.Transient);
          await buckets.CreateBucketAsync(bucketPayload, cmbRegion.Text);

          btnRefreshToken_Click(null, null);
        } catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Exception when creating bucket", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void btnShowDevTools_Click(object sender, EventArgs e)
    {
      browser.ShowDevTools();
    }
    
    private async void btnDownloadSVF_Click(object sender, EventArgs e)
    {
      // ensure the selected node is an object and get its URN
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 1)
      {
        MessageBox.Show("Please select an object", "Objects required", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      try
      {
        string urn = (string)treeBuckets.SelectedNode.Tag;

        // select a folder to download the files
        string folderPath = string.Empty;
        using (var fbd = new FolderBrowserDialog())
        {
          if (fbd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;
          folderPath = fbd.SelectedPath;
          folderPath = Path.Combine(folderPath, treeBuckets.SelectedNode.Text);
          if (Directory.Exists(folderPath)) Directory.Delete(folderPath, true);
          Directory.CreateDirectory(folderPath);
        }

        // prepare the UI
        progressBar.Show();
        progressBar.DisplayStyle = ProgressBarDisplayText.CustomText;
        progressBar.Value = 0;
        progressBar.CustomText = "Starting extraction...";

        // get the list of resources to download
        List<ForgeUtils.Derivatives.Resource> resourcesToDownload = await ForgeUtils.Derivatives.ExtractSVFAsync(urn, AccessToken);

        // update the UI
        progressBar.Minimum = 0;
        progressBar.Maximum = resourcesToDownload.Count;
        progressBar.Step = 1;

        IRestClient client = new RestClient("https://developer.api.autodesk.com/");
        foreach (ForgeUtils.Derivatives.Resource resource in resourcesToDownload)
        {
          progressBar.PerformStep();
          progressBar.CustomText = "Downloading " + resource.FileName;

          // prepare the GET to download the file
          RestRequest request = new RestRequest(resource.RemotePath, Method.GET);
          request.AddHeader("Authorization", "Bearer " + AccessToken);
          request.AddHeader("Accept-Encoding", "gzip, deflate");
          IRestResponse response = await client.ExecuteTaskAsync(request);

          if (response.StatusCode != System.Net.HttpStatusCode.OK)
          {
            // something went wrong with this file...
            MessageBox.Show(string.Format("Error downloading {0}: {1}",
              resource.FileName, response.StatusCode.ToString()));

            // any other action?
          } else
          {
            // combine with selected local path
            string pathToSave = Path.Combine(folderPath, resource.LocalPath);
            // ensure local dir exists
            Directory.CreateDirectory(Path.GetDirectoryName(pathToSave));
            // save file
            File.WriteAllBytes(pathToSave, response.RawBytes);
          }
        }
      } catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception when downloading svf", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      progressBar.Hide();
    }

    private void btnJavaScript_Click(object sender, EventArgs e)
    {
      Tools.JSEditor editor = new Tools.JSEditor(this.browser);
      editor.Show();
    }
  }
}
