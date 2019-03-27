using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveGenerator {
	public partial class TrochoidRippleWaveUC : UserControl {
		public Wave Wave {
			set {
				wave_ = value as TrochoidRippleWave;
				if ( wave_ == null )
					return;
				centerXTxt.Text = wave_.Center.x_.ToString();
				centerYTxt.Text = wave_.Center.y_.ToString();
				amplitudeTxt.Text = wave_.AmplitudeRate.ToString();
				waveLengthTxt.Text = wave_.WaveLength.ToString();
				phaseTxt.Text = wave_.PhaseSpeed.ToString();
				timeTxt.Text = wave_.Time.ToString();
			}
		}

		public TrochoidRippleWaveUC() {
			InitializeComponent();
		}

		private void centerXTxt_TextChanged(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.Center = new Vector2(
				StrConv.toFloat( centerXTxt.Text, wave_.Center.x_ ),
				wave_.Center.y_
			);
		}

		TrochoidRippleWave wave_;

		private void centerYTxt_TextChanged_1(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.Center = new Vector2(
				wave_.Center.x_,
				StrConv.toFloat( centerYTxt.Text, wave_.Center.y_ )
			);
		}

		private void waveLengthTxt_TextChanged_1(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.WaveLength = StrConv.toFloat( waveLengthTxt.Text, wave_.WaveLength );
		}

		private void amplitudeTxt_TextChanged(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.AmplitudeRate = StrConv.toFloat( amplitudeTxt.Text, wave_.AmplitudeRate );
		}

		private void phaseTxt_TextChanged_1(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.PhaseSpeed = StrConv.toFloat( phaseTxt.Text, wave_.PhaseSpeed );
		}

		private void timeTxt_TextChanged_1(object sender, EventArgs e) {
			if ( wave_ == null )
				return;
			wave_.setTime( StrConv.toFloat( timeTxt.Text, wave_.Time ) );
		}
	}
}
