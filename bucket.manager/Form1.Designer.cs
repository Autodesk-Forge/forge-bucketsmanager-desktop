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

namespace bucket.manager
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.treeBuckets = new System.Windows.Forms.TreeView();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.txtTimeout = new System.Windows.Forms.TextBox();
      this.txtAccessToken = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.btnAuthenticate = new System.Windows.Forms.Button();
      this.txtClientSecret = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtClientId = new System.Windows.Forms.TextBox();
      this.btnRefreshToken = new System.Windows.Forms.Button();
      this.btnCreateBucket = new System.Windows.Forms.Button();
      this.btnUpload = new System.Windows.Forms.Button();
      this.btnTranslate = new System.Windows.Forms.Button();
      this.menuTranslate = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.btnDeleteObject = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnShowDevTools = new System.Windows.Forms.Button();
      this.btnDownloadSVF = new System.Windows.Forms.Button();
      this.btnJavaScript = new System.Windows.Forms.Button();
      this.progressBar = new bucket.manager.Utils.CustomProgressBar();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeBuckets
      // 
      this.treeBuckets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.treeBuckets.Location = new System.Drawing.Point(12, 182);
      this.treeBuckets.Name = "treeBuckets";
      this.treeBuckets.Size = new System.Drawing.Size(225, 419);
      this.treeBuckets.TabIndex = 0;
      this.treeBuckets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeBuckets_AfterSelect);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(21, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "ID:";
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.txtTimeout);
      this.groupBox1.Controls.Add(this.txtAccessToken);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.btnAuthenticate);
      this.groupBox1.Controls.Add(this.txtClientSecret);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.txtClientId);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Location = new System.Drawing.Point(13, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(675, 76);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Forge Credentials";
      // 
      // txtTimeout
      // 
      this.txtTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTimeout.Location = new System.Drawing.Point(583, 46);
      this.txtTimeout.Name = "txtTimeout";
      this.txtTimeout.ReadOnly = true;
      this.txtTimeout.Size = new System.Drawing.Size(86, 20);
      this.txtTimeout.TabIndex = 10;
      this.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtAccessToken
      // 
      this.txtAccessToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtAccessToken.Location = new System.Drawing.Point(100, 46);
      this.txtAccessToken.Name = "txtAccessToken";
      this.txtAccessToken.ReadOnly = true;
      this.txtAccessToken.Size = new System.Drawing.Size(477, 20);
      this.txtAccessToken.TabIndex = 9;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(14, 49);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(79, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Access Token:";
      // 
      // btnAuthenticate
      // 
      this.btnAuthenticate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAuthenticate.Location = new System.Drawing.Point(583, 18);
      this.btnAuthenticate.Name = "btnAuthenticate";
      this.btnAuthenticate.Size = new System.Drawing.Size(86, 23);
      this.btnAuthenticate.TabIndex = 5;
      this.btnAuthenticate.Text = "Authenticate";
      this.btnAuthenticate.UseVisualStyleBackColor = true;
      this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
      // 
      // txtClientSecret
      // 
      this.txtClientSecret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtClientSecret.Location = new System.Drawing.Point(384, 20);
      this.txtClientSecret.Name = "txtClientSecret";
      this.txtClientSecret.PasswordChar = '*';
      this.txtClientSecret.Size = new System.Drawing.Size(193, 20);
      this.txtClientSecret.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(337, 24);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(41, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Secret:";
      // 
      // txtClientId
      // 
      this.txtClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtClientId.Location = new System.Drawing.Point(35, 20);
      this.txtClientId.Name = "txtClientId";
      this.txtClientId.Size = new System.Drawing.Size(296, 20);
      this.txtClientId.TabIndex = 2;
      // 
      // btnRefreshToken
      // 
      this.btnRefreshToken.Location = new System.Drawing.Point(13, 95);
      this.btnRefreshToken.Name = "btnRefreshToken";
      this.btnRefreshToken.Size = new System.Drawing.Size(121, 23);
      this.btnRefreshToken.TabIndex = 3;
      this.btnRefreshToken.Text = "Refresh buckets";
      this.btnRefreshToken.UseVisualStyleBackColor = true;
      this.btnRefreshToken.Click += new System.EventHandler(this.btnRefreshToken_Click);
      // 
      // btnCreateBucket
      // 
      this.btnCreateBucket.Location = new System.Drawing.Point(140, 95);
      this.btnCreateBucket.Name = "btnCreateBucket";
      this.btnCreateBucket.Size = new System.Drawing.Size(97, 23);
      this.btnCreateBucket.TabIndex = 4;
      this.btnCreateBucket.Text = "Create Bucket";
      this.btnCreateBucket.UseVisualStyleBackColor = true;
      this.btnCreateBucket.Click += new System.EventHandler(this.btnCreateBucket_Click);
      // 
      // btnUpload
      // 
      this.btnUpload.Location = new System.Drawing.Point(13, 153);
      this.btnUpload.Name = "btnUpload";
      this.btnUpload.Size = new System.Drawing.Size(93, 23);
      this.btnUpload.TabIndex = 5;
      this.btnUpload.Text = "Upload File";
      this.btnUpload.UseVisualStyleBackColor = true;
      this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
      // 
      // btnTranslate
      // 
      this.btnTranslate.Location = new System.Drawing.Point(113, 153);
      this.btnTranslate.Name = "btnTranslate";
      this.btnTranslate.Size = new System.Drawing.Size(124, 23);
      this.btnTranslate.TabIndex = 6;
      this.btnTranslate.Text = "Translate file";
      this.btnTranslate.UseVisualStyleBackColor = true;
      this.btnTranslate.Click += new System.EventHandler(this.btnTranslate_Click);
      // 
      // menuTranslate
      // 
      this.menuTranslate.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuTranslate.Name = "menuTranslate";
      this.menuTranslate.Size = new System.Drawing.Size(61, 4);
      // 
      // btnDeleteObject
      // 
      this.btnDeleteObject.Location = new System.Drawing.Point(13, 124);
      this.btnDeleteObject.Name = "btnDeleteObject";
      this.btnDeleteObject.Size = new System.Drawing.Size(103, 23);
      this.btnDeleteObject.TabIndex = 8;
      this.btnDeleteObject.Text = "Delete Object";
      this.btnDeleteObject.UseVisualStyleBackColor = true;
      this.btnDeleteObject.Click += new System.EventHandler(this.btnDeleteObject_Click);
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.Location = new System.Drawing.Point(242, 95);
      this.panel1.Margin = new System.Windows.Forms.Padding(2);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(446, 477);
      this.panel1.TabIndex = 9;
      // 
      // btnShowDevTools
      // 
      this.btnShowDevTools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnShowDevTools.Location = new System.Drawing.Point(613, 577);
      this.btnShowDevTools.Name = "btnShowDevTools";
      this.btnShowDevTools.Size = new System.Drawing.Size(75, 23);
      this.btnShowDevTools.TabIndex = 10;
      this.btnShowDevTools.Text = "DevTools";
      this.btnShowDevTools.UseVisualStyleBackColor = true;
      this.btnShowDevTools.Click += new System.EventHandler(this.btnShowDevTools_Click);
      // 
      // btnDownloadSVF
      // 
      this.btnDownloadSVF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDownloadSVF.Location = new System.Drawing.Point(492, 577);
      this.btnDownloadSVF.Name = "btnDownloadSVF";
      this.btnDownloadSVF.Size = new System.Drawing.Size(115, 23);
      this.btnDownloadSVF.TabIndex = 11;
      this.btnDownloadSVF.Text = "Download SVF";
      this.btnDownloadSVF.UseVisualStyleBackColor = true;
      this.btnDownloadSVF.Click += new System.EventHandler(this.btnDownloadSVF_Click);
      // 
      // btnJavaScript
      // 
      this.btnJavaScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnJavaScript.Location = new System.Drawing.Point(411, 577);
      this.btnJavaScript.Name = "btnJavaScript";
      this.btnJavaScript.Size = new System.Drawing.Size(75, 23);
      this.btnJavaScript.TabIndex = 12;
      this.btnJavaScript.Text = "JavaScript";
      this.btnJavaScript.UseVisualStyleBackColor = true;
      this.btnJavaScript.Click += new System.EventHandler(this.btnJavaScript_Click);
      // 
      // progressBar
      // 
      this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar.CustomText = null;
      this.progressBar.DisplayStyle = bucket.manager.Utils.ProgressBarDisplayText.Percentage;
      this.progressBar.Location = new System.Drawing.Point(12, 607);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(676, 23);
      this.progressBar.TabIndex = 7;
      this.progressBar.Visible = false;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(700, 642);
      this.Controls.Add(this.btnJavaScript);
      this.Controls.Add(this.btnDownloadSVF);
      this.Controls.Add(this.btnShowDevTools);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.btnDeleteObject);
      this.Controls.Add(this.progressBar);
      this.Controls.Add(this.btnTranslate);
      this.Controls.Add(this.btnUpload);
      this.Controls.Add(this.btnCreateBucket);
      this.Controls.Add(this.btnRefreshToken);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.treeBuckets);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Bucket Manager";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView treeBuckets;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox txtAccessToken;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnAuthenticate;
    private System.Windows.Forms.TextBox txtClientSecret;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtClientId;
    private System.Windows.Forms.TextBox txtTimeout;
    private System.Windows.Forms.Button btnRefreshToken;
    private System.Windows.Forms.Button btnCreateBucket;
    private System.Windows.Forms.Button btnUpload;
    private System.Windows.Forms.Button btnTranslate;
    private Utils.CustomProgressBar progressBar;
    private System.Windows.Forms.ContextMenuStrip menuTranslate;
    private System.Windows.Forms.Button btnDeleteObject;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnShowDevTools;
    private System.Windows.Forms.Button btnDownloadSVF;
    private System.Windows.Forms.Button btnJavaScript;
  }
}

