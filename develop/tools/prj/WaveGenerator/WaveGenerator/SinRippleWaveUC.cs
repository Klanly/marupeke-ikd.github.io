using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveGenerator
{
	public partial class SinRippleWaveUC : UserControl
	{
		public Wave Wave {
			set {
				wave_ = value as SinRippleWave;
				if (wave_ == null)
					return;
				centerXTxt.Text = wave_.Center.x_.ToString();
				centerYTxt.Text = wave_.Center.y_.ToString();
				amplitudeTxt.Text = wave_.Amplitude.ToString();
				waveLengthTxt.Text = wave_.WaveLength.ToString();
				phaseTxt.Text = wave_.PhaseSpeed.ToString();
				timeTxt.Text = wave_.Time.ToString();
				peakedPowerTxt.Text = wave_.PeakedPower.ToString();
			}
		}
		public SinRippleWaveUC() {
			InitializeComponent();
		}

		private void centerXTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.Center = new Vector2(
				StrConv.toFloat( centerXTxt.Text, wave_.Center.x_ ),
				wave_.Center.y_
			);
		}

		SinRippleWave wave_;

		private void centerYTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.Center = new Vector2(
				wave_.Center.x_,
				StrConv.toFloat( centerYTxt.Text, wave_.Center.y_ )
			);
		}

		private void waveLengthTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.WaveLength = StrConv.toFloat( waveLengthTxt.Text, wave_.WaveLength );
		}

		private void textBox1_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.Amplitude = StrConv.toFloat( amplitudeTxt.Text, wave_.Amplitude );
		}

		private void label4_Click(object sender, EventArgs e) {

		}

		private void phaseTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.PhaseSpeed = StrConv.toFloat( phaseTxt.Text, wave_.PhaseSpeed );
		}

		private void timeTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.setTime( StrConv.toFloat( timeTxt.Text, wave_.Time ) );
		}

		private void peakedPowerTxt_TextChanged(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.PeakedPower = StrConv.toFloat( peakedPowerTxt.Text, wave_.PeakedPower );
		}
	}
}
