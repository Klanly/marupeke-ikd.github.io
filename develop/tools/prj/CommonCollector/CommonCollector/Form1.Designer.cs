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
            this.bCheckOverwrite = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // inputCommonFolderBtn
            // 
            this.inputCommonFolderBtn.Location = new System.Drawing.Point(9, 10);
            this.inputCommonFolderBtn.Margin = new System.Windows.Forms.Padding(2);
            this.inputCommonFolderBtn.Name = "inputCommonFolderBtn";
            this.inputCommonFolderBtn.Size = new System.Drawing.Size(56, 21);
            this.inputCommonFolderBtn.TabIndex = 0;
            this.inputCommonFolderBtn.Text = "入力元";
            this.inputCommonFolderBtn.UseVisualStyleBackColor = true;
            this.inputCommonFolderBtn.Click += new System.EventHandler(this.inputCommonFolderBtn_Click);
            // 
            // inputCommonFolderTxt
            // 
            this.inputCommonFolderTxt.Location = new System.Drawing.Point(70, 10);
            this.inputCommonFolderTxt.Margin = new System.Windows.Forms.Padding(2);
            this.inputCommonFolderTxt.Name = "inputCommonFolderTxt";
            this.inputCommonFolderTxt.Size = new System.Drawing.Size(462, 19);
            this.inputCommonFolderTxt.TabIndex = 1;
            // 
            // outputFolderTxt
            // 
            this.outputFolderTxt.Location = new System.Drawing.Point(70, 33);
            this.outputFolderTxt.Margin = new System.Windows.Forms.Padding(2);
            this.outputFolderTxt.Name = "outputFolderTxt";
            this.outputFolderTxt.Size = new System.Drawing.Size(462, 19);
            this.outputFolderTxt.TabIndex = 3;
            // 
            // outputFolderBtn
            // 
            this.outputFolderBtn.Location = new System.Drawing.Point(9, 33);
            this.outputFolderBtn.Margin = new System.Windows.Forms.Padding(2);
            this.outputFolderBtn.Name = "outputFolderBtn";
            this.outputFolderBtn.Size = new System.Drawing.Size(56, 21);
            this.outputFolderBtn.TabIndex = 2;
            this.outputFolderBtn.Text = "出力先";
            this.outputFolderBtn.UseVisualStyleBackColor = true;
            this.outputFolderBtn.Click += new System.EventHandler(this.outputFolderBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.Location = new System.Drawing.Point(9, 84);
            this.runBtn.Margin = new System.Windows.Forms.Padding(2);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(56, 18);
            this.runBtn.TabIndex = 4;
            this.runBtn.Text = "Run";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // bCheckOverwrite
            // 
            this.bCheckOverwrite.AutoSize = true;
            this.bCheckOverwrite.Checked = true;
            this.bCheckOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bCheckOverwrite.Location = new System.Drawing.Point(90, 57);
            this.bCheckOverwrite.Name = "bCheckOverwrite";
            this.bCheckOverwrite.Size = new System.Drawing.Size(76, 16);
            this.bCheckOverwrite.TabIndex = 5;
            this.bCheckOverwrite.Text = "上書きする";
            this.bCheckOverwrite.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 113);
            this.Controls.Add(this.bCheckOverwrite);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.outputFolderTxt);
            this.Controls.Add(this.outputFolderBtn);
            this.Controls.Add(this.inputCommonFolderTxt);
            this.Controls.Add(this.inputCommonFolderBtn);
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.CheckBox bCheckOverwrite;
    }
}

