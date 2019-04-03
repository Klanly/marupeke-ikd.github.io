namespace WaveGenerator {
	partial class TrochoidStraightWaveUC {
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

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.timeTxt = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.phaseTxt = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.amplitudeTxt = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.waveLengthTxt = new System.Windows.Forms.TextBox();
			this.dirTxt = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// timeTxt
			// 
			this.timeTxt.Location = new System.Drawing.Point(173, 152);
			this.timeTxt.Margin = new System.Windows.Forms.Padding(4);
			this.timeTxt.Name = "timeTxt";
			this.timeTxt.Size = new System.Drawing.Size(83, 22);
			this.timeTxt.TabIndex = 32;
			this.timeTxt.TextChanged += new System.EventHandler(this.timeTxt_TextChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 156);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(71, 15);
			this.label6.TabIndex = 31;
			this.label6.Text = "Time(sec)";
			// 
			// phaseTxt
			// 
			this.phaseTxt.Location = new System.Drawing.Point(173, 122);
			this.phaseTxt.Margin = new System.Windows.Forms.Padding(4);
			this.phaseTxt.Name = "phaseTxt";
			this.phaseTxt.Size = new System.Drawing.Size(83, 22);
			this.phaseTxt.TabIndex = 30;
			this.phaseTxt.TextChanged += new System.EventHandler(this.phaseTxt_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 129);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 15);
			this.label5.TabIndex = 29;
			this.label5.Text = "Phase";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 12);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(116, 15);
			this.label4.TabIndex = 28;
			this.label4.Text = "Straight sin wave";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 95);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 15);
			this.label3.TabIndex = 27;
			this.label3.Text = "Amplitude";
			// 
			// amplitudeTxt
			// 
			this.amplitudeTxt.Location = new System.Drawing.Point(173, 92);
			this.amplitudeTxt.Margin = new System.Windows.Forms.Padding(4);
			this.amplitudeTxt.Name = "amplitudeTxt";
			this.amplitudeTxt.Size = new System.Drawing.Size(83, 22);
			this.amplitudeTxt.TabIndex = 26;
			this.amplitudeTxt.TextChanged += new System.EventHandler(this.amplitudeTxt_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 68);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 15);
			this.label2.TabIndex = 25;
			this.label2.Text = "wave Length";
			// 
			// waveLengthTxt
			// 
			this.waveLengthTxt.Location = new System.Drawing.Point(173, 64);
			this.waveLengthTxt.Margin = new System.Windows.Forms.Padding(4);
			this.waveLengthTxt.Name = "waveLengthTxt";
			this.waveLengthTxt.Size = new System.Drawing.Size(83, 22);
			this.waveLengthTxt.TabIndex = 24;
			this.waveLengthTxt.TextChanged += new System.EventHandler(this.waveLengthTxt_TextChanged);
			// 
			// dirTxt
			// 
			this.dirTxt.Location = new System.Drawing.Point(173, 36);
			this.dirTxt.Margin = new System.Windows.Forms.Padding(4);
			this.dirTxt.Name = "dirTxt";
			this.dirTxt.Size = new System.Drawing.Size(83, 22);
			this.dirTxt.TabIndex = 23;
			this.dirTxt.TextChanged += new System.EventHandler(this.dirTxt_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 40);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(115, 15);
			this.label1.TabIndex = 22;
			this.label1.Text = "direction(degree)";
			// 
			// TrochoidStraightWaveUC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.timeTxt);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.phaseTxt);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.amplitudeTxt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.waveLengthTxt);
			this.Controls.Add(this.dirTxt);
			this.Controls.Add(this.label1);
			this.Name = "TrochoidStraightWaveUC";
			this.Size = new System.Drawing.Size(330, 322);
			this.Load += new System.EventHandler(this.TrochoidStraightWaveUC_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox timeTxt;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox phaseTxt;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox amplitudeTxt;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox waveLengthTxt;
		private System.Windows.Forms.TextBox dirTxt;
		private System.Windows.Forms.Label label1;
	}
}
