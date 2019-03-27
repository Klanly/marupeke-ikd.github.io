namespace WaveGenerator
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && ( components != null )) {
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
			this.outputPict = new System.Windows.Forms.PictureBox();
			this.runBtn = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.waveSelector = new System.Windows.Forms.CheckedListBox();
			this.addWaveBtn = new System.Windows.Forms.Button();
			this.removeWaveBtn = new System.Windows.Forms.Button();
			this.saveToClipBoardBtn = new System.Windows.Forms.Button();
			this.waveParamContPanel = new System.Windows.Forms.Panel();
			this.waveTypeLB = new System.Windows.Forms.ListBox();
			this.gridWidthTxt = new System.Windows.Forms.TextBox();
			this.gridHeightTxt = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.worldWidthTxt = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.worldCenterXTxt = new System.Windows.Forms.TextBox();
			this.worldCenterYTxt = new System.Windows.Forms.TextBox();
			this.dupricateWaveBtn = new System.Windows.Forms.Button();
			this.trochoidRippleWaveUC = new WaveGenerator.TrochoidRippleWaveUC();
			this.sinStraightWaveUC = new WaveGenerator.SinStraightWaveUC();
			this.sinRippleWaveUC = new WaveGenerator.SinRippleWaveUC();
			((System.ComponentModel.ISupportInitialize)(this.outputPict)).BeginInit();
			this.panel1.SuspendLayout();
			this.waveParamContPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// outputPict
			// 
			this.outputPict.BackColor = System.Drawing.Color.Silver;
			this.outputPict.Location = new System.Drawing.Point(3, 2);
			this.outputPict.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.outputPict.Name = "outputPict";
			this.outputPict.Size = new System.Drawing.Size(256, 256);
			this.outputPict.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.outputPict.TabIndex = 0;
			this.outputPict.TabStop = false;
			// 
			// runBtn
			// 
			this.runBtn.Location = new System.Drawing.Point(592, 490);
			this.runBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.runBtn.Name = "runBtn";
			this.runBtn.Size = new System.Drawing.Size(104, 31);
			this.runBtn.TabIndex = 1;
			this.runBtn.Text = "Run";
			this.runBtn.UseVisualStyleBackColor = true;
			this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.outputPict);
			this.panel1.Location = new System.Drawing.Point(11, 10);
			this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(347, 325);
			this.panel1.TabIndex = 2;
			// 
			// waveSelector
			// 
			this.waveSelector.FormattingEnabled = true;
			this.waveSelector.Location = new System.Drawing.Point(365, 12);
			this.waveSelector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.waveSelector.Name = "waveSelector";
			this.waveSelector.Size = new System.Drawing.Size(295, 106);
			this.waveSelector.TabIndex = 3;
			this.waveSelector.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.waveSelector_ItemCheck);
			this.waveSelector.SelectedIndexChanged += new System.EventHandler(this.waveSelector_SelectedIndexChanged);
			// 
			// addWaveBtn
			// 
			this.addWaveBtn.Location = new System.Drawing.Point(669, 15);
			this.addWaveBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.addWaveBtn.Name = "addWaveBtn";
			this.addWaveBtn.Size = new System.Drawing.Size(27, 25);
			this.addWaveBtn.TabIndex = 4;
			this.addWaveBtn.Text = "+";
			this.addWaveBtn.UseVisualStyleBackColor = true;
			this.addWaveBtn.Click += new System.EventHandler(this.addWaveBtn_Click);
			// 
			// removeWaveBtn
			// 
			this.removeWaveBtn.Location = new System.Drawing.Point(669, 48);
			this.removeWaveBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.removeWaveBtn.Name = "removeWaveBtn";
			this.removeWaveBtn.Size = new System.Drawing.Size(27, 25);
			this.removeWaveBtn.TabIndex = 5;
			this.removeWaveBtn.Text = "-";
			this.removeWaveBtn.UseVisualStyleBackColor = true;
			this.removeWaveBtn.Click += new System.EventHandler(this.removeWaveBtn_Click);
			// 
			// saveToClipBoardBtn
			// 
			this.saveToClipBoardBtn.Location = new System.Drawing.Point(11, 490);
			this.saveToClipBoardBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.saveToClipBoardBtn.Name = "saveToClipBoardBtn";
			this.saveToClipBoardBtn.Size = new System.Drawing.Size(153, 31);
			this.saveToClipBoardBtn.TabIndex = 6;
			this.saveToClipBoardBtn.Text = "Save to clipboard";
			this.saveToClipBoardBtn.UseVisualStyleBackColor = true;
			this.saveToClipBoardBtn.Click += new System.EventHandler(this.saveToClipBoardBtn_Click);
			// 
			// waveParamContPanel
			// 
			this.waveParamContPanel.Controls.Add(this.trochoidRippleWaveUC);
			this.waveParamContPanel.Controls.Add(this.sinStraightWaveUC);
			this.waveParamContPanel.Controls.Add(this.sinRippleWaveUC);
			this.waveParamContPanel.Location = new System.Drawing.Point(365, 232);
			this.waveParamContPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.waveParamContPanel.Name = "waveParamContPanel";
			this.waveParamContPanel.Size = new System.Drawing.Size(331, 250);
			this.waveParamContPanel.TabIndex = 8;
			this.waveParamContPanel.TabStop = true;
			this.waveParamContPanel.Visible = false;
			// 
			// waveTypeLB
			// 
			this.waveTypeLB.FormattingEnabled = true;
			this.waveTypeLB.ItemHeight = 15;
			this.waveTypeLB.Items.AddRange(new object[] {
            "Ripple Sin",
            "Straight Sin",
            "Ripple Trochoid"});
			this.waveTypeLB.Location = new System.Drawing.Point(365, 130);
			this.waveTypeLB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.waveTypeLB.Name = "waveTypeLB";
			this.waveTypeLB.Size = new System.Drawing.Size(295, 94);
			this.waveTypeLB.TabIndex = 9;
			this.waveTypeLB.SelectedIndexChanged += new System.EventHandler(this.waveTypeLB_SelectedIndexChanged);
			// 
			// gridWidthTxt
			// 
			this.gridWidthTxt.Location = new System.Drawing.Point(197, 342);
			this.gridWidthTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gridWidthTxt.Name = "gridWidthTxt";
			this.gridWidthTxt.Size = new System.Drawing.Size(73, 22);
			this.gridWidthTxt.TabIndex = 10;
			this.gridWidthTxt.TextChanged += new System.EventHandler(this.gridWidthTxt_TextChanged);
			// 
			// gridHeightTxt
			// 
			this.gridHeightTxt.Location = new System.Drawing.Point(280, 342);
			this.gridHeightTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.gridHeightTxt.Name = "gridHeightTxt";
			this.gridHeightTxt.Size = new System.Drawing.Size(73, 22);
			this.gridHeightTxt.TabIndex = 11;
			this.gridHeightTxt.TextChanged += new System.EventHandler(this.gridHeightTxt_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 346);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(159, 15);
			this.label1.TabIndex = 12;
			this.label1.Text = "grid size( width, height )";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 378);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 15);
			this.label2.TabIndex = 13;
			this.label2.Text = "World width";
			// 
			// worldWidthTxt
			// 
			this.worldWidthTxt.Location = new System.Drawing.Point(197, 374);
			this.worldWidthTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.worldWidthTxt.Name = "worldWidthTxt";
			this.worldWidthTxt.Size = new System.Drawing.Size(73, 22);
			this.worldWidthTxt.TabIndex = 14;
			this.worldWidthTxt.TextChanged += new System.EventHandler(this.worldWidthTxt_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 410);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 15);
			this.label3.TabIndex = 15;
			this.label3.Text = "World center";
			// 
			// worldCenterXTxt
			// 
			this.worldCenterXTxt.Location = new System.Drawing.Point(197, 406);
			this.worldCenterXTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.worldCenterXTxt.Name = "worldCenterXTxt";
			this.worldCenterXTxt.Size = new System.Drawing.Size(73, 22);
			this.worldCenterXTxt.TabIndex = 16;
			this.worldCenterXTxt.TextChanged += new System.EventHandler(this.worldCenterXTxt_TextChanged);
			// 
			// worldCenterYTxt
			// 
			this.worldCenterYTxt.Location = new System.Drawing.Point(280, 406);
			this.worldCenterYTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.worldCenterYTxt.Name = "worldCenterYTxt";
			this.worldCenterYTxt.Size = new System.Drawing.Size(73, 22);
			this.worldCenterYTxt.TabIndex = 17;
			this.worldCenterYTxt.TextChanged += new System.EventHandler(this.worldCenterYTxt_TextChanged);
			// 
			// dupricateWaveBtn
			// 
			this.dupricateWaveBtn.Location = new System.Drawing.Point(669, 80);
			this.dupricateWaveBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.dupricateWaveBtn.Name = "dupricateWaveBtn";
			this.dupricateWaveBtn.Size = new System.Drawing.Size(27, 25);
			this.dupricateWaveBtn.TabIndex = 18;
			this.dupricateWaveBtn.Text = "D";
			this.dupricateWaveBtn.UseVisualStyleBackColor = true;
			this.dupricateWaveBtn.Click += new System.EventHandler(this.dupricateWaveBtn_Click);
			// 
			// trochoidRippleWaveUC
			// 
			this.trochoidRippleWaveUC.Location = new System.Drawing.Point(3, 3);
			this.trochoidRippleWaveUC.Name = "trochoidRippleWaveUC";
			this.trochoidRippleWaveUC.Size = new System.Drawing.Size(292, 232);
			this.trochoidRippleWaveUC.TabIndex = 19;
			// 
			// sinStraightWaveUC
			// 
			this.sinStraightWaveUC.Location = new System.Drawing.Point(0, 0);
			this.sinStraightWaveUC.Margin = new System.Windows.Forms.Padding(5);
			this.sinStraightWaveUC.Name = "sinStraightWaveUC";
			this.sinStraightWaveUC.Size = new System.Drawing.Size(292, 246);
			this.sinStraightWaveUC.TabIndex = 9;
			// 
			// sinRippleWaveUC
			// 
			this.sinRippleWaveUC.Location = new System.Drawing.Point(0, 0);
			this.sinRippleWaveUC.Margin = new System.Windows.Forms.Padding(5);
			this.sinRippleWaveUC.Name = "sinRippleWaveUC";
			this.sinRippleWaveUC.Size = new System.Drawing.Size(292, 246);
			this.sinRippleWaveUC.TabIndex = 7;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(707, 536);
			this.Controls.Add(this.dupricateWaveBtn);
			this.Controls.Add(this.worldCenterYTxt);
			this.Controls.Add(this.worldCenterXTxt);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.worldWidthTxt);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridHeightTxt);
			this.Controls.Add(this.gridWidthTxt);
			this.Controls.Add(this.waveTypeLB);
			this.Controls.Add(this.waveParamContPanel);
			this.Controls.Add(this.saveToClipBoardBtn);
			this.Controls.Add(this.removeWaveBtn);
			this.Controls.Add(this.addWaveBtn);
			this.Controls.Add(this.waveSelector);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.runBtn);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MainForm";
			this.Text = "Wave Generator v0.01";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.outputPict)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.waveParamContPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox outputPict;
		private System.Windows.Forms.Button runBtn;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckedListBox waveSelector;
		private System.Windows.Forms.Button addWaveBtn;
		private System.Windows.Forms.Button removeWaveBtn;
		private System.Windows.Forms.Button saveToClipBoardBtn;
		private SinRippleWaveUC sinRippleWaveUC;
		private System.Windows.Forms.Panel waveParamContPanel;
		private SinStraightWaveUC sinStraightWaveUC;
		private System.Windows.Forms.ListBox waveTypeLB;
		private System.Windows.Forms.TextBox gridWidthTxt;
		private System.Windows.Forms.TextBox gridHeightTxt;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox worldWidthTxt;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox worldCenterXTxt;
		private System.Windows.Forms.TextBox worldCenterYTxt;
		private System.Windows.Forms.Button dupricateWaveBtn;
		private TrochoidRippleWaveUC trochoidRippleWaveUC;
	}
}

