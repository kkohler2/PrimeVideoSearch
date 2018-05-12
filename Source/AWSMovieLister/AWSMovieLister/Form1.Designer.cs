namespace AWSMovieLister
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rbMovies = new System.Windows.Forms.RadioButton();
            this.rbTV = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbDownloadScope = new System.Windows.Forms.ComboBox();
            this.tbNew = new System.Windows.Forms.TextBox();
            this.lblNew = new System.Windows.Forms.Label();
            this.tbFound = new System.Windows.Forms.TextBox();
            this.lblFound = new System.Windows.Forms.Label();
            this.tbPage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDetails = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDetailProgress = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(8, 72);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(2);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(13, 14);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1342, 690);
            this.webBrowser1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rbMovies
            // 
            this.rbMovies.AutoSize = true;
            this.rbMovies.Checked = true;
            this.rbMovies.Location = new System.Drawing.Point(901, 9);
            this.rbMovies.Margin = new System.Windows.Forms.Padding(2);
            this.rbMovies.Name = "rbMovies";
            this.rbMovies.Size = new System.Drawing.Size(83, 24);
            this.rbMovies.TabIndex = 8;
            this.rbMovies.TabStop = true;
            this.rbMovies.Text = "Movies";
            this.rbMovies.UseVisualStyleBackColor = true;
            this.rbMovies.CheckedChanged += new System.EventHandler(this.rbMovies_CheckedChanged);
            // 
            // rbTV
            // 
            this.rbTV.AutoSize = true;
            this.rbTV.Location = new System.Drawing.Point(995, 9);
            this.rbTV.Margin = new System.Windows.Forms.Padding(2);
            this.rbTV.Name = "rbTV";
            this.rbTV.Size = new System.Drawing.Size(106, 24);
            this.rbTV.TabIndex = 9;
            this.rbTV.Text = "TV Shows";
            this.rbTV.UseVisualStyleBackColor = true;
            this.rbTV.CheckedChanged += new System.EventHandler(this.rbTV_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1204, 8);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbDownloadScope);
            this.panel1.Controls.Add(this.tbNew);
            this.panel1.Controls.Add(this.lblNew);
            this.panel1.Controls.Add(this.tbFound);
            this.panel1.Controls.Add(this.lblFound);
            this.panel1.Controls.Add(this.tbPage);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(8, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(874, 46);
            this.panel1.TabIndex = 12;
            // 
            // cbDownloadScope
            // 
            this.cbDownloadScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDownloadScope.FormattingEnabled = true;
            this.cbDownloadScope.Items.AddRange(new object[] {
            "All",
            "Last 7 Days",
            "Last 30 Days",
            "Last 90 Days"});
            this.cbDownloadScope.Location = new System.Drawing.Point(714, 11);
            this.cbDownloadScope.Name = "cbDownloadScope";
            this.cbDownloadScope.Size = new System.Drawing.Size(147, 28);
            this.cbDownloadScope.TabIndex = 18;
            // 
            // tbNew
            // 
            this.tbNew.Location = new System.Drawing.Point(560, 11);
            this.tbNew.Margin = new System.Windows.Forms.Padding(2);
            this.tbNew.Name = "tbNew";
            this.tbNew.ReadOnly = true;
            this.tbNew.Size = new System.Drawing.Size(124, 26);
            this.tbNew.TabIndex = 17;
            // 
            // lblNew
            // 
            this.lblNew.AutoSize = true;
            this.lblNew.Location = new System.Drawing.Point(440, 11);
            this.lblNew.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNew.Name = "lblNew";
            this.lblNew.Size = new System.Drawing.Size(93, 20);
            this.lblNew.TabIndex = 16;
            this.lblNew.Text = "New Movies";
            // 
            // tbFound
            // 
            this.tbFound.Location = new System.Drawing.Point(291, 11);
            this.tbFound.Margin = new System.Windows.Forms.Padding(2);
            this.tbFound.Name = "tbFound";
            this.tbFound.ReadOnly = true;
            this.tbFound.Size = new System.Drawing.Size(124, 26);
            this.tbFound.TabIndex = 15;
            // 
            // lblFound
            // 
            this.lblFound.AutoSize = true;
            this.lblFound.Location = new System.Drawing.Point(161, 11);
            this.lblFound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(108, 20);
            this.lblFound.TabIndex = 14;
            this.lblFound.Text = "Movies Found";
            this.lblFound.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbPage
            // 
            this.tbPage.Location = new System.Drawing.Point(72, 11);
            this.tbPage.Margin = new System.Windows.Forms.Padding(2);
            this.tbPage.Name = "tbPage";
            this.tbPage.ReadOnly = true;
            this.tbPage.Size = new System.Drawing.Size(66, 26);
            this.tbPage.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Page";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbDetails
            // 
            this.cbDetails.AutoSize = true;
            this.cbDetails.Location = new System.Drawing.Point(1107, 8);
            this.cbDetails.Name = "cbDetails";
            this.cbDetails.Size = new System.Drawing.Size(84, 24);
            this.cbDetails.TabIndex = 13;
            this.cbDetails.Text = "Details";
            this.cbDetails.UseVisualStyleBackColor = true;
            this.cbDetails.CheckedChanged += new System.EventHandler(this.cbDetails_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblDetailProgress);
            this.panel2.Location = new System.Drawing.Point(8, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(874, 48);
            this.panel2.TabIndex = 14;
            this.panel2.Visible = false;
            // 
            // lblDetailProgress
            // 
            this.lblDetailProgress.AutoSize = true;
            this.lblDetailProgress.Location = new System.Drawing.Point(17, 13);
            this.lblDetailProgress.Name = "lblDetailProgress";
            this.lblDetailProgress.Size = new System.Drawing.Size(145, 20);
            this.lblDetailProgress.TabIndex = 0;
            this.lblDetailProgress.Text = "0 of 0                        ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1375, 775);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cbDetails);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.rbTV);
            this.Controls.Add(this.rbMovies);
            this.Controls.Add(this.webBrowser1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "AWS Movie Lister";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton rbMovies;
        private System.Windows.Forms.RadioButton rbTV;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbDownloadScope;
        private System.Windows.Forms.TextBox tbNew;
        private System.Windows.Forms.Label lblNew;
        private System.Windows.Forms.TextBox tbFound;
        private System.Windows.Forms.Label lblFound;
        private System.Windows.Forms.TextBox tbPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblDetailProgress;
    }
}

