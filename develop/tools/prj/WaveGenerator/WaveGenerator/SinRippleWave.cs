using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// sin波による波紋

namespace WaveGenerator
{
	public class SinRippleWave : Wave
	{
		public SinRippleWave() {
			waveType_ = EWaveType.SinRipple;
		}

		// 中心点を設定
		//  x, y: ワールドスケール座標での中心位置
		public Vector2 Center { set { center_ = value; } get { return center_; } }

		public float Amplitude { set { setAmplitude( value ); } get { return amp_; } }

		public float WaveLength { set { setWaveLen( value ); } get { return waveLen_; } }

		public float PhaseSpeed { set { setPhaseSpeed( value ); } get { return phaseSpeed_; } }

		public float PeakedPower { set { setPeakedPower( value ); } get { return peakedPower_; } }

		// 振幅を設定
		// amp: 振幅値（0以上）
		public void setAmplitude( float amp ) {
			if (amp < 0.0f)
				amp = 0.0f;
			amp_ = amp;
		}

		// 波長を設定
		//  waveLen: ワールドスケール座標での波長
		public void setWaveLen( float waveLen ) {
			waveLen_ = waveLen;
		}

		// 位相速度を設定
		//  phaseSpeed: ワールドスケールでの波頂が移動する速度(dist/sec) 
		public void setPhaseSpeed( float phaseSpeed ) {
			phaseSpeed_ = phaseSpeed;
		}

		// 尖度を設定
		public void setPeakedPower( float peakedPower ) {
			if ( peakedPower < 1.0f )
				peakedPower = 1.0f;
			peakedPower_ = peakedPower;
		}

		// ワールドに波を加算
		override public void addMe(World world) {
			Vector2 wp = new Vector2(); // ワールド位置
			Vector2 tmp = new Vector2();
			var grid = world.Grid;
			for ( int y = 0; y < world.GridPixelHeight; ++y ) {
				for ( int x = 0; x < world.GridPixelWidth; ++x ) {
					world.getWorldPos( x, y, ref wp );
					float d = center_.sub( ref tmp, wp ).Len;
					float v = ( float )( Math.Sin( d / waveLen_ + phaseSpeed_ * sec_ ) );
					grid[ x, y ] += ( float )( amp_ * Math.Pow( ( v + 1.0f ) / 2.0f, peakedPower_ ) );
				}
			}
		}

		// クローン作成
		override public Wave clone() {
			var obj = new SinRippleWave();
			obj.Center = Center;
			obj.WaveLength = WaveLength;
			obj.Amplitude = Amplitude;
			obj.phaseSpeed_ = phaseSpeed_;
			obj.setTime( Time );
			obj.PeakedPower = PeakedPower;
			return obj;
		}


		Vector2 center_ = new Vector2();
		float amp_ = 1.0f;
		float waveLen_ = 2.0f;
		float phaseSpeed_ = 1.0f;
		float peakedPower_ = 1.0f;
	}
}
