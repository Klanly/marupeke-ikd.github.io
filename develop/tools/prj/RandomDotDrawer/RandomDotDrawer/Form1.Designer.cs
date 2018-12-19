namespace RandomDotDrawer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.runBtn = new System.Windows.Forms.Button();
            this.dotImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.distanceTxt = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pixelRadiusTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.randomSeedTxt = new System.Windows.Forms.TextBox();
            this.colorRangeMinTxt = new System.Windows.Forms.TextBox();
            this.colorRangeMaxTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textureSizeTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.filtersLB = new System.Windows.Forms.ListBox();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.filterApplyBtn = new System.Windows.Forms.Button();
            this.gaussianBlurCtl = new RandomDotDrawer.gaussianblurctl();
            ((System.ComponentModel.ISupportInitialize)(this.dotImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Location = new System.Drawing.Point(378, 245);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(75, 23);
            this.runBtn.TabIndex = 0;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // dotImage
            // 
            this.dotImage.BackColor = System.Drawing.Color.White;
            this.dotImage.Location = new System.Drawing.Point(0, 0);
            this.dotImage.Name = "dotImage";
            this.dotImage.Size = new System.Drawing.Size(256, 256);
            this.dotImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.dotImage.TabIndex = 1;
            this.dotImage.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.dotImage);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 262);
            this.panel1.TabIndex = 2;
            // 
            // distanceTxt
            // 
            this.distanceTxt.Location = new System.Drawing.Point(377, 46);
            this.distanceTxt.Name = "distanceTxt";
            this.distanceTxt.Size = new System.Drawing.Size(75, 22);
            this.distanceTxt.TabIndex = 3;
            this.distanceTxt.Text = "5";
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(290, 203);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(163, 23);
            this.saveBtn.TabIndex = 5;
            this.saveBtn.Text = "Save to clipboard";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "distance";
            // 
            // pixelRadiusTxt
            // 
            this.pixelRadiusTxt.Location = new System.Drawing.Point(377, 74);
            this.pixelRadiusTxt.Name = "pixelRadiusTxt";
            this.pixelRadiusTxt.Size = new System.Drawing.Size(75, 22);
            this.pixelRadiusTxt.TabIndex = 7;
            this.pixelRadiusTxt.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(287, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "random seed";
            // 
            // randomSeedTxt
            // 
            this.randomSeedTxt.Location = new System.Drawing.Point(377, 18);
            this.randomSeedTxt.Name = "randomSeedTxt";
            this.randomSeedTxt.Size = new System.Drawing.Size(75, 22);
            this.randomSeedTxt.TabIndex = 9;
            this.randomSeedTxt.Text = "0";
            // 
            // colorRangeMinTxt
            // 
            this.colorRangeMinTxt.Location = new System.Drawing.Point(364, 125);
            this.colorRangeMinTxt.Name = "colorRangeMinTxt";
            this.colorRangeMinTxt.Size = new System.Drawing.Size(34, 22);
            this.colorRangeMinTxt.TabIndex = 11;
            this.colorRangeMinTxt.Text = "0";
            // 
            // colorRangeMaxTxt
            // 
            this.colorRangeMaxTxt.Location = new System.Drawing.Point(418, 125);
            this.colorRangeMaxTxt.Name = "colorRangeMaxTxt";
            this.colorRangeMaxTxt.Size = new System.Drawing.Size(34, 22);
            this.colorRangeMaxTxt.TabIndex = 12;
            this.colorRangeMaxTxt.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(366, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "min";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(420, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(281, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "color range";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(401, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 15);
            this.label7.TabIndex = 16;
            this.label7.Text = "-";
            // 
            // textureSizeTxt
            // 
            this.textureSizeTxt.Location = new System.Drawing.Point(378, 164);
            this.textureSizeTxt.Name = "textureSizeTxt";
            this.textureSizeTxt.Size = new System.Drawing.Size(74, 22);
            this.textureSizeTxt.TabIndex = 17;
            this.textureSizeTxt.Text = "256";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "pixel radius";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(293, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 15);
            this.label8.TabIndex = 18;
            this.label8.Text = "texture size";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(472, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 15);
            this.label9.TabIndex = 19;
            this.label9.Text = "filters";
            // 
            // filtersLB
            // 
            this.filtersLB.FormattingEnabled = true;
            this.filtersLB.ItemHeight = 15;
            this.filtersLB.Items.AddRange(new object[] {
            "Gaussian Blur"});
            this.filtersLB.Location = new System.Drawing.Point(475, 39);
            this.filtersLB.Name = "filtersLB";
            this.filtersLB.Size = new System.Drawing.Size(135, 49);
            this.filtersLB.TabIndex = 20;
            // 
            // filterPanel
            // 
            this.filterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filterPanel.Controls.Add(this.gaussianBlurCtl);
            this.filterPanel.Location = new System.Drawing.Point(475, 94);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(268, 180);
            this.filterPanel.TabIndex = 21;
            // 
            // filterApplyBtn
            // 
            this.filterApplyBtn.Location = new System.Drawing.Point(616, 65);
            this.filterApplyBtn.Name = "filterApplyBtn";
            this.filterApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.filterApplyBtn.TabIndex = 22;
            this.filterApplyBtn.Text = "Apply";
            this.filterApplyBtn.UseVisualStyleBackColor = true;
            this.filterApplyBtn.Click += new System.EventHandler(this.filterApplyBtn_Click);
            // 
            // gaussianBlurCtl
            // 
            this.gaussianBlurCtl.Location = new System.Drawing.Point(3, 3);
            this.gaussianBlurCtl.Name = "gaussianBlurCtl";
            this.gaussianBlurCtl.Size = new System.Drawing.Size(229, 150);
            this.gaussianBlurCtl.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(750, 282);
            this.Controls.Add(this.filterApplyBtn);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.filtersLB);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textureSizeTxt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.colorRangeMaxTxt);
            this.Controls.Add(this.colorRangeMinTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.randomSeedTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pixelRadiusTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.distanceTxt);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.runBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Random Dot Drawer v1.0.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dotImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.filterPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.PictureBox dotImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox distanceTxt;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pixelRadiusTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox randomSeedTxt;
        private System.Windows.Forms.TextBox colorRangeMinTxt;
        private System.Windows.Forms.TextBox colorRangeMaxTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textureSizeTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox filtersLB;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.Button filterApplyBtn;
        private gaussianblurctl gaussianBlurCtl;
    }
}

