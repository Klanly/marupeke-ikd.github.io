using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// sin波による直進波

namespace WaveGenerator
{
	public class SinStraightWave : Wave
	{
		public SinStraightWave() {
			waveType_ = EWaveType.SinStraignt;
		}

		// 進行向き
		public Vector2 Direction {
			set {
				dir_ = value;
				dir_ = dir_.normalize();
				var ang = Math.Atan2( dir_.y_, dir_.x_ );
				if (ang < 0.0f)
					deg_ = 360.0f - ( float )ang;
				else
					deg_ = ( float )ang;
			}
			get {
				return dir_;
			}
		}
		public float Degree {
			set {
				deg_ = value;
				dir_.x_ = ( float )Math.Cos( deg_ / 180.0f * Math.PI );
				dir_.y_ = ( float )Math.Sin( deg_ / 180.0f * Math.PI );
			}
			get {
				return deg_;
			}
		}
		public float Amplitude { set { setAmplitude( value ); } get { return amp_; } }

		public float WaveLength { set { setWaveLen( value ); } get { return waveLen_; } }

		public float PhaseSpeed { set { setPhaseSpeed( value ); } get { return phaseSpeed_; } }

		public float PeakedPower { set { setPeakedPower( value ); } get { return peakedPower_; } }

		// 振幅を設定
		// amp: 振幅値（0以上）
		public void setAmplitude(float amp) {
			if (amp < 0.0f)
				amp = 0.0f;
			amp_ = amp;
		}

		// 波長を設定
		//  waveLen: ワールドスケール座標での波長
		public void setWaveLen(float waveLen) {
			waveLen_ = waveLen;
		}

		// 位相速度を設定
		//  phaseSpeed: ワールドスケールでの波頂が移動する速度(dist/sec) 
		public void setPhaseSpeed(float phaseSpeed) {
			phaseSpeed_ = phaseSpeed;
		}

		// 尖度を設定
		public void setPeakedPower(float peakedPower) {
			if ( peakedPower < 1.0f )
				peakedPower = 1.0f;
			peakedPower_ = peakedPower;
		}

		// ワールドに波を加算
		override public void addMe(World world) {
			Vector2 wp = new Vector2(); // ワールド位置
			Vector2 tmp = new Vector2();
			var grid = world.Grid;
			float grav = 9.8f;
			float _2pi_per_L = ( float )( Math.PI * 2.0f / waveLen_ );
			float velo = ( float )( Math.Sqrt( _2pi_per_L * grav ) );
			for ( int y = 0; y < world.GridPixelHeight; ++y) {
				for (int x = 0; x < world.GridPixelWidth; ++x) {
					world.getWorldPos( x, y, ref wp );
					float d = dir_.x_ * wp.x_ + dir_.y_ * wp.y_;
					float h = ( float )( Math.Sin( d * _2pi_per_L - phaseSpeed_ * velo * sec_ ) );
					grid[ x, y ] += ( float )( amp_ * Math.Pow( ( h + 1.0f ) / 2.0f, peakedPower_ ) );
				}
			}
		}

		// クローン作成
		override public Wave clone() {
			var obj = new SinStraightWave();
			obj.Degree = Degree;
			obj.WaveLength = WaveLength;
			obj.Amplitude = Amplitude;
			obj.phaseSpeed_ = phaseSpeed_;
			obj.setTime( Time );
			obj.PeakedPower = PeakedPower;
			return obj;
		}

		Vector2 dir_ = new Vector2( 1.0f, 0.0f );
		float deg_ = 0.0f;
		float amp_ = 1.0f;
		float waveLen_ = 2.0f;
		float phaseSpeed_ = 1.0f;
		float peakedPower_ = 1.0f;
	}
}
