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
      this.btnAuthorize = new System.Windows.Forms.Button();
      this.txtClientSecret = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtClientId = new System.Windows.Forms.TextBox();
      this.btnRefreshToken = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.btnUpload = new System.Windows.Forms.Button();
      this.btnTranslate = new System.Windows.Forms.Button();
      this.menuTranslate = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.btnDeleteObject = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.progressBar = new bucket.manager.Utils.CustomProgressBar();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeBuckets
      // 
      this.treeBuckets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.treeBuckets.Location = new System.Drawing.Point(16, 224);
      this.treeBuckets.Margin = new System.Windows.Forms.Padding(4);
      this.treeBuckets.Name = "treeBuckets";
      this.treeBuckets.Size = new System.Drawing.Size(299, 515);
      this.treeBuckets.TabIndex = 0;
      this.treeBuckets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeBuckets_AfterSelect);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(19, 30);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(25, 17);
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
      this.groupBox1.Controls.Add(this.btnAuthorize);
      this.groupBox1.Controls.Add(this.txtClientSecret);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.txtClientId);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Location = new System.Drawing.Point(17, 16);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
      this.groupBox1.Size = new System.Drawing.Size(900, 94);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Forge Credentials";
      // 
      // txtTimeout
      // 
      this.txtTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTimeout.Location = new System.Drawing.Point(777, 57);
      this.txtTimeout.Margin = new System.Windows.Forms.Padding(4);
      this.txtTimeout.Name = "txtTimeout";
      this.txtTimeout.ReadOnly = true;
      this.txtTimeout.Size = new System.Drawing.Size(113, 22);
      this.txtTimeout.TabIndex = 10;
      this.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtAccessToken
      // 
      this.txtAccessToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtAccessToken.Location = new System.Drawing.Point(133, 57);
      this.txtAccessToken.Margin = new System.Windows.Forms.Padding(4);
      this.txtAccessToken.Name = "txtAccessToken";
      this.txtAccessToken.ReadOnly = true;
      this.txtAccessToken.Size = new System.Drawing.Size(635, 22);
      this.txtAccessToken.TabIndex = 9;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(19, 60);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(101, 17);
      this.label4.TabIndex = 8;
      this.label4.Text = "Access Token:";
      // 
      // btnAuthorize
      // 
      this.btnAuthorize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAuthorize.Location = new System.Drawing.Point(777, 22);
      this.btnAuthorize.Margin = new System.Windows.Forms.Padding(4);
      this.btnAuthorize.Name = "btnAuthorize";
      this.btnAuthorize.Size = new System.Drawing.Size(115, 28);
      this.btnAuthorize.TabIndex = 5;
      this.btnAuthorize.Text = "Authenticate";
      this.btnAuthorize.UseVisualStyleBackColor = true;
      this.btnAuthorize.Click += new System.EventHandler(this.btnAuthorize_Click);
      // 
      // txtClientSecret
      // 
      this.txtClientSecret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtClientSecret.Location = new System.Drawing.Point(512, 25);
      this.txtClientSecret.Margin = new System.Windows.Forms.Padding(4);
      this.txtClientSecret.Name = "txtClientSecret";
      this.txtClientSecret.PasswordChar = '*';
      this.txtClientSecret.Size = new System.Drawing.Size(256, 22);
      this.txtClientSecret.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(449, 30);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(53, 17);
      this.label2.TabIndex = 3;
      this.label2.Text = "Secret:";
      // 
      // txtClientId
      // 
      this.txtClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtClientId.Location = new System.Drawing.Point(47, 25);
      this.txtClientId.Margin = new System.Windows.Forms.Padding(4);
      this.txtClientId.Name = "txtClientId";
      this.txtClientId.Size = new System.Drawing.Size(393, 22);
      this.txtClientId.TabIndex = 2;
      // 
      // btnRefreshToken
      // 
      this.btnRefreshToken.Location = new System.Drawing.Point(17, 117);
      this.btnRefreshToken.Margin = new System.Windows.Forms.Padding(4);
      this.btnRefreshToken.Name = "btnRefreshToken";
      this.btnRefreshToken.Size = new System.Drawing.Size(161, 28);
      this.btnRefreshToken.TabIndex = 3;
      this.btnRefreshToken.Text = "Refresh buckets";
      this.btnRefreshToken.UseVisualStyleBackColor = true;
      this.btnRefreshToken.Click += new System.EventHandler(this.btnRefreshToken_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(187, 117);
      this.button2.Margin = new System.Windows.Forms.Padding(4);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(129, 28);
      this.button2.TabIndex = 4;
      this.button2.Text = "Create Bucket";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // btnUpload
      // 
      this.btnUpload.Location = new System.Drawing.Point(17, 188);
      this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
      this.btnUpload.Name = "btnUpload";
      this.btnUpload.Size = new System.Drawing.Size(124, 28);
      this.btnUpload.TabIndex = 5;
      this.btnUpload.Text = "Upload File";
      this.btnUpload.UseVisualStyleBackColor = true;
      this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
      // 
      // btnTranslate
      // 
      this.btnTranslate.Location = new System.Drawing.Point(151, 188);
      this.btnTranslate.Margin = new System.Windows.Forms.Padding(4);
      this.btnTranslate.Name = "btnTranslate";
      this.btnTranslate.Size = new System.Drawing.Size(165, 28);
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
      this.btnDeleteObject.Location = new System.Drawing.Point(17, 153);
      this.btnDeleteObject.Margin = new System.Windows.Forms.Padding(4);
      this.btnDeleteObject.Name = "btnDeleteObject";
      this.btnDeleteObject.Size = new System.Drawing.Size(137, 28);
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
      this.panel1.Location = new System.Drawing.Point(323, 117);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(594, 622);
      this.panel1.TabIndex = 9;
      // 
      // progressBar
      // 
      this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar.CustomText = null;
      this.progressBar.DisplayStyle = bucket.manager.Utils.ProgressBarDisplayText.Percentage;
      this.progressBar.Location = new System.Drawing.Point(16, 747);
      this.progressBar.Margin = new System.Windows.Forms.Padding(4);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new System.Drawing.Size(901, 28);
      this.progressBar.TabIndex = 7;
      this.progressBar.Visible = false;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(933, 790);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.btnDeleteObject);
      this.Controls.Add(this.progressBar);
      this.Controls.Add(this.btnTranslate);
      this.Controls.Add(this.btnUpload);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.btnRefreshToken);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.treeBuckets);
      this.Margin = new System.Windows.Forms.Padding(4);
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
    private System.Windows.Forms.Button btnAuthorize;
    private System.Windows.Forms.TextBox txtClientSecret;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtClientId;
    private System.Windows.Forms.TextBox txtTimeout;
    private System.Windows.Forms.Button btnRefreshToken;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button btnUpload;
    private System.Windows.Forms.Button btnTranslate;
    private Utils.CustomProgressBar progressBar;
    private System.Windows.Forms.ContextMenuStrip menuTranslate;
    private System.Windows.Forms.Button btnDeleteObject;
    private System.Windows.Forms.Panel panel1;
  }
}

