namespace WaveGenerator {
	partial class TrochoidRippleWaveUC {
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
			this.centerYTxt = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.centerXTxt = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// timeTxt
			// 
			this.timeTxt.Location = new System.Drawing.Point(171, 152);
			this.timeTxt.Margin = new System.Windows.Forms.Padding(4);
			this.timeTxt.Name = "timeTxt";
			this.timeTxt.Size = new System.Drawing.Size(83, 22);
			this.timeTxt.TabIndex = 35;
			this.timeTxt.TextChanged += new System.EventHandler(this.timeTxt_TextChanged_1);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(24, 156);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(71, 15);
			this.label6.TabIndex = 34;
			this.label6.Text = "Time(sec)";
			// 
			// phaseTxt
			// 
			this.phaseTxt.Location = new System.Drawing.Point(171, 122);
			this.phaseTxt.Margin = new System.Windows.Forms.Padding(4);
			this.phaseTxt.Name = "phaseTxt";
			this.phaseTxt.Size = new System.Drawing.Size(83, 22);
			this.phaseTxt.TabIndex = 33;
			this.phaseTxt.TextChanged += new System.EventHandler(this.phaseTxt_TextChanged_1);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 126);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 15);
			this.label5.TabIndex = 32;
			this.label5.Text = "Phase";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 12);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(138, 15);
			this.label4.TabIndex = 31;
			this.label4.Text = "Ripple trochoid wave";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 95);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 15);
			this.label3.TabIndex = 30;
			this.label3.Text = "AmplitudeRate";
			// 
			// amplitudeTxt
			// 
			this.amplitudeTxt.Location = new System.Drawing.Point(171, 92);
			this.amplitudeTxt.Margin = new System.Windows.Forms.Padding(4);
			this.amplitudeTxt.Name = "amplitudeTxt";
			this.amplitudeTxt.Size = new System.Drawing.Size(83, 22);
			this.amplitudeTxt.TabIndex = 29;
			this.amplitudeTxt.TextChanged += new System.EventHandler(this.amplitudeTxt_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 67);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 15);
			this.label2.TabIndex = 28;
			this.label2.Text = "wave Length";
			// 
			// waveLengthTxt
			// 
			this.waveLengthTxt.Location = new System.Drawing.Point(171, 63);
			this.waveLengthTxt.Margin = new System.Windows.Forms.Padding(4);
			this.waveLengthTxt.Name = "waveLengthTxt";
			this.waveLengthTxt.Size = new System.Drawing.Size(83, 22);
			this.waveLengthTxt.TabIndex = 27;
			this.waveLengthTxt.TextChanged += new System.EventHandler(this.waveLengthTxt_TextChanged_1);
			// 
			// centerYTxt
			// 
			this.centerYTxt.Location = new System.Drawing.Point(171, 36);
			this.centerYTxt.Margin = new System.Windows.Forms.Padding(4);
			this.centerYTxt.Name = "centerYTxt";
			this.centerYTxt.Size = new System.Drawing.Size(83, 22);
			this.centerYTxt.TabIndex = 26;
			this.centerYTxt.TextChanged += new System.EventHandler(this.centerYTxt_TextChanged_1);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 40);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 15);
			this.label1.TabIndex = 25;
			this.label1.Text = "center";
			// 
			// centerXTxt
			// 
			this.centerXTxt.Location = new System.Drawing.Point(79, 36);
			this.centerXTxt.Margin = new System.Windows.Forms.Padding(4);
			this.centerXTxt.Name = "centerXTxt";
			this.centerXTxt.Size = new System.Drawing.Size(83, 22);
			this.centerXTxt.TabIndex = 24;
			this.centerXTxt.TextChanged += new System.EventHandler(this.centerXTxt_TextChanged);
			// 
			// TrochoidRippleWaveUC
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
			this.Controls.Add(this.centerYTxt);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.centerXTxt);
			this.Name = "TrochoidRippleWaveUC";
			this.Size = new System.Drawing.Size(292, 232);
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
		private System.Windows.Forms.TextBox centerYTxt;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox centerXTxt;
	}
}
