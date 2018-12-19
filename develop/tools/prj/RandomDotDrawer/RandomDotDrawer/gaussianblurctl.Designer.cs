namespace RandomDotDrawer
{
    partial class gaussianblurctl
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.radiusTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.samplingNum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Blur Radius";
            // 
            // radiusTxt
            // 
            this.radiusTxt.Location = new System.Drawing.Point(109, 9);
            this.radiusTxt.Name = "radiusTxt";
            this.radiusTxt.Size = new System.Drawing.Size(100, 22);
            this.radiusTxt.TabIndex = 1;
            this.radiusTxt.Text = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sampling Num";
            // 
            // samplingNum
            // 
            this.samplingNum.Location = new System.Drawing.Point(109, 35);
            this.samplingNum.Name = "samplingNum";
            this.samplingNum.Size = new System.Drawing.Size(100, 22);
            this.samplingNum.TabIndex = 1;
            this.samplingNum.Text = "3";
            // 
            // gaussianblurctl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.samplingNum);
            this.Controls.Add(this.radiusTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "gaussianblurctl";
            this.Size = new System.Drawing.Size(229, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox radiusTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox samplingNum;
    }
}
