namespace CommonCollector {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
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
        private void InitializeComponent() {
            this.inputCommonFolderBtn = new System.Windows.Forms.Button();
            this.inputCommonFolderTxt = new System.Windows.Forms.TextBox();
            this.outputFolderTxt = new System.Windows.Forms.TextBox();
            this.outputFolderBtn = new System.Windows.Forms.Button();
            this.runBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputCommonFolderBtn
            // 
            this.inputCommonFolderBtn.Location = new System.Drawing.Point(12, 12);
            this.inputCommonFolderBtn.Name = "inputCommonFolderBtn";
            this.inputCommonFolderBtn.Size = new System.Drawing.Size(75, 26);
            this.inputCommonFolderBtn.TabIndex = 0;
            this.inputCommonFolderBtn.Text = "入力元";
            this.inputCommonFolderBtn.UseVisualStyleBackColor = true;
            this.inputCommonFolderBtn.Click += new System.EventHandler(this.inputCommonFolderBtn_Click);
            // 
            // inputCommonFolderTxt
            // 
            this.inputCommonFolderTxt.Location = new System.Drawing.Point(93, 12);
            this.inputCommonFolderTxt.Name = "inputCommonFolderTxt";
            this.inputCommonFolderTxt.Size = new System.Drawing.Size(614, 22);
            this.inputCommonFolderTxt.TabIndex = 1;
            // 
            // outputFolderTxt
            // 
            this.outputFolderTxt.Location = new System.Drawing.Point(93, 41);
            this.outputFolderTxt.Name = "outputFolderTxt";
            this.outputFolderTxt.Size = new System.Drawing.Size(614, 22);
            this.outputFolderTxt.TabIndex = 3;
            // 
            // outputFolderBtn
            // 
            this.outputFolderBtn.Location = new System.Drawing.Point(12, 41);
            this.outputFolderBtn.Name = "outputFolderBtn";
            this.outputFolderBtn.Size = new System.Drawing.Size(75, 26);
            this.outputFolderBtn.TabIndex = 2;
            this.outputFolderBtn.Text = "出力先";
            this.outputFolderBtn.UseVisualStyleBackColor = true;
            this.outputFolderBtn.Click += new System.EventHandler(this.outputFolderBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.Location = new System.Drawing.Point(12, 85);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(75, 23);
            this.runBtn.TabIndex = 4;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 122);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.outputFolderTxt);
            this.Controls.Add(this.outputFolderBtn);
            this.Controls.Add(this.inputCommonFolderTxt);
            this.Controls.Add(this.inputCommonFolderBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button inputCommonFolderBtn;
        private System.Windows.Forms.TextBox inputCommonFolderTxt;
        private System.Windows.Forms.TextBox outputFolderTxt;
        private System.Windows.Forms.Button outputFolderBtn;
        private System.Windows.Forms.Button runBtn;
    }
}

