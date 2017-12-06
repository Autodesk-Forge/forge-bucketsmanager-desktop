using Autodesk.Forge;
using Autodesk.Forge.Model;
using bucket.manager.Utils;
using CefSharp;
using CefSharp.WinForms;
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
      Cef.Initialize(new CefSettings());
      browser = new ChromiumWebBrowser("file:///HTML/Viewer.html");

      browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
          | System.Windows.Forms.AnchorStyles.Left)
          | System.Windows.Forms.AnchorStyles.Right)));
      //browser.Location = new System.Drawing.Point(323, 117);
      browser.MinimumSize = new System.Drawing.Size(20, 20);
      browser.Name = "webBrowser1";
      //browser.Size = new System.Drawing.Size(594, 622);
      browser.TabIndex = 1;
      browser.Dock = DockStyle.Fill;
      panel1.Controls.Add(browser);
    }

    private Timer _tokenTimer = new Timer();
    private Timer _translationTimer = new Timer();
    private DateTime _expiresAt;

    private async void btnAuthorize_Click(object sender, EventArgs e)
    {
      // get the access token
      TwoLeggedApi oAuth = new TwoLeggedApi();
      dynamic token = await oAuth.AuthenticateAsync(
        txtClientId.Text,
        txtClientSecret.Text,
        oAuthConstants.CLIENT_CREDENTIALS,
        new Scope[] { Scope.BucketRead, Scope.DataRead, Scope.DataWrite });
      txtAccessToken.Text = token.access_token;
      _expiresAt = DateTime.Now.AddSeconds(token.expires_in);

      // keep track on time
      _tokenTimer.Tick += new EventHandler(tickTokenTimer);
      _tokenTimer.Interval = 1000;
      _tokenTimer.Enabled = true;

      btnRefreshToken_Click(null, null);
    }

    void tickTokenTimer(object sender, EventArgs e)
    {
      // update the time left
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

      BucketsApi bucketApi = new BucketsApi();
      bucketApi.Configuration.AccessToken = AccessToken;
      dynamic buckets = await bucketApi.GetBucketsAsync(null, 100);
         
      foreach (KeyValuePair<string, dynamic> bucket in new DynamicDictionaryItems(buckets.items))
      {
        TreeNode nodeBucket = new TreeNode(bucket.Value.bucketKey);
        nodeBucket.Tag = bucket.Value.bucketKey;
        treeBuckets.Nodes.Add(nodeBucket);
      }

      foreach (TreeNode n in treeBuckets.Nodes)
        if (n!=null) // async?
          await ShowBucketObjects(n);
    }

    private async Task ShowBucketObjects(TreeNode nodeBucket)
    {
      nodeBucket.Nodes.Clear();
      ObjectsApi objects = new ObjectsApi();

      objects.Configuration.AccessToken = AccessToken;
      var objectsList = await objects.GetObjectsAsync((string)nodeBucket.Tag);
      foreach (KeyValuePair<string, dynamic> objInfo in new DynamicDictionaryItems(objectsList.items))
      {
        TreeNode nodeObject = new TreeNode(objInfo.Value.objectKey);
        nodeObject.Tag = ((string)objInfo.Value.objectId).Base64Encode();
        nodeBucket.Nodes.Add(nodeObject);
      }
    }

    private async void btnUpload_Click(object sender, EventArgs e)
    {
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 0)
      {
        MessageBox.Show("Please select a bucket", "Bucket required", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      OpenFileDialog formSelectFile = new OpenFileDialog();
      formSelectFile.Multiselect = false;
      if (formSelectFile.ShowDialog() != DialogResult.OK) return;
      string filePath = formSelectFile.FileName;
      string objectKey = Path.GetFileName(filePath);

      string bucketKey = treeBuckets.SelectedNode.Text;

      ObjectsApi objects = new ObjectsApi();
      objects.Configuration.AccessToken = AccessToken;

      long fileSize = (new FileInfo(filePath)).Length;
      long chunkSize = 2 * 1024 * 1024; // 2 Mb
      long numberOfChunks = (long)Math.Round((double)(fileSize / chunkSize)) + 1;

      long start = 0;
      chunkSize = (numberOfChunks > 1 ? chunkSize : fileSize);
      long end = chunkSize;
      string sessionId = Guid.NewGuid().ToString();

      progressBar.DisplayStyle = ProgressBarDisplayText.CustomText;
      progressBar.Show();
      progressBar.Value = 0;
      progressBar.Minimum = 0;
      progressBar.Maximum = (int)numberOfChunks;

      progressBar.CustomText = "Preparing to upload file...";

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
      progressBar.Hide();
      await ShowBucketObjects(treeBuckets.SelectedNode);
      treeBuckets.SelectedNode.Expand();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      //btnAuthorize_Click(null, null);
    }

    private void btnTranslate_Click(object sender, EventArgs e)
    {
      menuTranslate.Items.Clear();
      menuTranslate.Items.Add("Viewer (SVF)", null, onClickTranslate);
      menuTranslate.Show(btnTranslate, new Point(0, btnTranslate.Height));
    }

    private async void onClickTranslate(object sender, EventArgs e)
    {
      if (_translationTimer.Enabled) return;

      string urn = (string)treeBuckets.SelectedNode.Tag; 

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

      progressBar.Show();
      progressBar.Minimum = 0;
      progressBar.Maximum = 100;
      progressBar.CustomText = "Starting translation job...";

      DerivativesApi derivative = new DerivativesApi();
      derivative.Configuration.AccessToken = AccessToken;
      dynamic jobPosted = await derivative.TranslateAsync(job);

      // keep track on time
      _translationTimer.Tick += new EventHandler(isTranslationReady);
      _translationTimer.Tag = urn;
      _translationTimer.Interval = 5000;
      _translationTimer.Enabled = true;
    }

    private async void isTranslationReady(object sender, EventArgs e)
    {
      DerivativesApi derivative = new DerivativesApi();
      derivative.Configuration.AccessToken = AccessToken;
      dynamic manifest = await derivative.GetManifestAsync((string)_translationTimer.Tag);
      int progress = (string.IsNullOrWhiteSpace(Regex.Match(manifest.progress, @"\d+").Value) ? 100 : Int32.Parse(Regex.Match(manifest.progress, @"\d+").Value));
      progressBar.Value = (progress == 0 ? 10 : progress);
      progressBar.CustomText = string.Format("Translation in progress: {0}", progress);
      Debug.WriteLine(progress);
      if (progress >= 100)
      {
        progressBar.Hide();
        _translationTimer.Enabled = false;
      }
    }

    private async void btnDeleteObject_Click(object sender, EventArgs e)
    {
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 1)
      {
        MessageBox.Show("Please select an object", "Objects required", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      if (MessageBox.Show("This objects will be permantly delete, confirm?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)!= DialogResult.Yes)
        return;

      ObjectsApi objects = new ObjectsApi();
      objects.Configuration.AccessToken = AccessToken;
      await objects.DeleteObjectAsync((string)treeBuckets.SelectedNode.Parent.Tag, (string)treeBuckets.SelectedNode.Text);
      await ShowBucketObjects(treeBuckets.SelectedNode.Parent);
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      Cef.Shutdown();
    }

    private void treeBuckets_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (treeBuckets.SelectedNode == null || treeBuckets.SelectedNode.Level != 1) return;

      browser.Load(string.Format("file:///HTML/Viewer.html?URN={0}&Token={1}", treeBuckets.SelectedNode.Tag, AccessToken));
      browser.ShowDevTools();
    }
  }
}
