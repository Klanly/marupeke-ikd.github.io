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
	public partial class SinStraightWaveUC : UserControl
	{
		SinStraightWave wave_;

		public Wave Wave {
			set {
				wave_ = value as SinStraightWave;
				if (wave_ == null)
					return;
				dirTxt.Text = wave_.Degree.ToString();
				amplitudeTxt.Text = wave_.Amplitude.ToString();
				waveLengthTxt.Text = wave_.WaveLength.ToString();
				phaseTxt.Text = wave_.PhaseSpeed.ToString();
				timeTxt.Text = wave_.Time.ToString();
				peakedPowerTxt.Text = wave_.PeakedPower.ToString();
			}
		}

		public SinStraightWaveUC() {
			InitializeComponent();
		}

		private void label4_Click(object sender, EventArgs e) {

		}

		private void dirYTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.Degree = StrConv.toFloat( dirTxt.Text, wave_.Degree );
		}

		private void waveLengthTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.WaveLength = StrConv.toFloat( waveLengthTxt.Text, wave_.WaveLength );
		}

		private void amplitudeTxt_TextChanged(object sender, EventArgs e) {
			if (wave_ == null)
				return;
			wave_.Amplitude = StrConv.toFloat( amplitudeTxt.Text, wave_.Amplitude );
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
