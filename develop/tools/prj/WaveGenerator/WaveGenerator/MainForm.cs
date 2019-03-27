using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveGenerator
{
	public partial class MainForm : Form
	{
		int gridWidth_ = 512;
		int gridHeight_ = 512;
		float worldWidth_ = 100.0f;
		Vector2 worldCenter_ = new Vector2();
		List<Wave> waves_ = new List<Wave>();

		public MainForm() {
			InitializeComponent();
		}

		private void runBtn_Click(object sender, EventArgs e) {

			// 指定サイズのBMPを作成
			var bmp = new Bitmap( gridWidth_, gridHeight_ );

			// BMPData取得
			//  ARGBフォーマットのはず
			var bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat );

			// ワールド作成
			var world = new World( bmp.Width, bmp.Height, worldWidth_, worldCenter_ );

			// Waveデータを重ね合わせ
			foreach ( var w in waves_ ) {
				if ( w.Active == true )
					w.addMe( world );
			}

			// 色情報配列をbmpDataへコピー
			var data = world.createColorByteArray( true, false, 1.0f );
			System.Runtime.InteropServices.Marshal.Copy( data, 0, bmpData.Scan0, data.Length );

			// ロック解除
			bmp.UnlockBits( bmpData );

			// outputPictへ反映
			outputPict.Image = bmp;
		}

		private void addWaveBtn_Click(object sender, EventArgs e) {
			// Wave追加
			waveSelector.Items.Add( "Wave", true );

			// デフォルト波生成
			waves_.Add( new SinRippleWave() );
		}

		// 波を削除
		private void removeWaveBtn_Click(object sender, EventArgs e) {
			var index = waveSelector.SelectedIndex;
			if (index < 0)
				return;
			// 選択中のWaveを削除
			waveSelector.Items.Remove( waveSelector.SelectedItem );
			waves_.RemoveAt( index );
			// 波リストとタイプリストを非選択に
			waveSelector.SelectedIndex = -1;
			waveTypeLB.SelectedIndex = -1;
		}

		private void saveToClipBoardBtn_Click(object sender, EventArgs e) {
			Clipboard.SetImage( outputPict.Image );
		}

		private void waveSelector_SelectedIndexChanged(object sender, EventArgs e) {
			var index = waveSelector.SelectedIndex;
			if (index < 0) {
				waveParamContPanel.Visible = false;
				return;
			}
			var item = waveSelector.SelectedItem;

			invisibleAllWaveControllers();
			visibleWaveController( waves_[ index ] );
			waveParamContPanel.Visible = true;

			selectWaveList( waves_[ index ] );
		}

		void invisibleAllWaveControllers() {
			sinRippleWaveUC.Visible = false;
			sinStraightWaveUC.Visible = false;
			trochoidRippleWaveUC.Visible = false;
		}

		void visibleWaveController( Wave wave ) {
			switch (wave.WaveType) {
			case Wave.EWaveType.SinRipple:
				sinRippleWaveUC.Wave = wave;
				sinRippleWaveUC.Visible = true;
				break;
			case Wave.EWaveType.SinStraignt:
				sinStraightWaveUC.Wave = wave;
				sinStraightWaveUC.Visible = true;
				break;
			case Wave.EWaveType.TrochoidRipple:
				trochoidRippleWaveUC.Wave = wave;
				trochoidRippleWaveUC.Visible = true;
				break;
			}
			return;
		}

		void selectWaveList( Wave wave ) {
			if (wave.WaveType == Wave.EWaveType.None)
				return;
			waveTypeLB.SelectedIndex = ( int )wave.WaveType - 1;
		}

		private void waveTypeLB_SelectedIndexChanged(object sender, EventArgs e) {
			int listIndex = waveTypeLB.SelectedIndex;
			if (listIndex < 0)
				return;
			int waveIndex = waveSelector.SelectedIndex;
			if (waveIndex < 0)
				return;
			var waveType = waves_[ waveIndex ].WaveType;
			if (waveType == Wave.EWaveType.None)
				return;
			if (listIndex == ( int )waveType - 1)
				return; // 同じタイプ

			waves_[ waveIndex ] = createWave( (Wave.EWaveType)( listIndex + 1 ) );

			invisibleAllWaveControllers();
			visibleWaveController( waves_[ waveIndex ] );
			waveParamContPanel.Visible = true;

			selectWaveList( waves_[ waveIndex ] );
		}

		Wave createWave( Wave.EWaveType waveType ) {
			switch (waveType) {
			case Wave.EWaveType.None:
				return null;
			case Wave.EWaveType.SinRipple:
				return new SinRippleWave();
			case Wave.EWaveType.SinStraignt:
				return new SinStraightWave();
				case Wave.EWaveType.TrochoidRipple:
					return new TrochoidRippleWave();
			}
			return null;
		}

		private void gridWidthTxt_TextChanged(object sender, EventArgs e) {
			gridWidth_ = StrConv.toInt( gridWidthTxt.Text, gridWidth_ );
		}

		private void gridHeightTxt_TextChanged(object sender, EventArgs e) {
			gridHeight_ = StrConv.toInt( gridHeightTxt.Text, gridHeight_ );
		}

		private void worldWidthTxt_TextChanged(object sender, EventArgs e) {
			worldWidth_ = StrConv.toFloat( worldWidthTxt.Text, worldWidth_ );
		}

		private void MainForm_Load(object sender, EventArgs e) {
			gridWidthTxt.Text = gridWidth_.ToString();
			gridHeightTxt.Text = gridHeight_.ToString();
			worldWidthTxt.Text = worldWidth_.ToString();
			worldCenterXTxt.Text = worldCenter_.x_.ToString();
			worldCenterYTxt.Text = worldCenter_.y_.ToString();
		}

		private void worldCenterXTxt_TextChanged(object sender, EventArgs e) {
			worldCenter_.x_ = StrConv.toFloat( worldCenterXTxt.Text, worldCenter_.x_ );
		}

		private void worldCenterYTxt_TextChanged(object sender, EventArgs e) {
			worldCenter_.y_ = StrConv.toFloat( worldCenterYTxt.Text, worldCenter_.y_ );
		}

		private void dupricateWaveBtn_Click(object sender, EventArgs e) {
			// 選択されているwaveを複製して追加
			var index = waveSelector.SelectedIndex;
			if (index < 0)
				return;
			var srcWave = waves_[ index ];
			var dupWave = srcWave.clone();
			if (dupWave == null)
				return;

			// Wave追加
			waveSelector.Items.Add( "Wave", true );
			waves_.Add( dupWave );
		}

		private void waveSelector_ItemCheck(object sender, ItemCheckEventArgs e) {
			if (e.Index >= waves_.Count)
				return;
			waves_[ e.Index ].Active = ( e.NewValue == CheckState.Checked );
		}
	}
}
