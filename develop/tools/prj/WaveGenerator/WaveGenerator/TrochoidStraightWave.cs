using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator {
	class TrochoidStraightWave : Wave {
		public TrochoidStraightWave() {
			waveType_ = EWaveType.TrochoidStraight;
		}

		// 進行向き
		public Vector2 Direction {
			set {
				dir_ = value;
				dir_ = dir_.normalize();
				var ang = Math.Atan2( dir_.y_, dir_.x_ );
				if ( ang < 0.0f )
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
		public float AmplitudeRate { set { setAmplitudeRate( value ); } get { return ampRate_; } }

		public float WaveLength { set { setWaveLen( value ); } get { return waveLen_; } }

		public float PhaseSpeed { set { setPhaseSpeed( value ); } get { return phaseSpeed_; } }

		// 波長に対する振幅率を設定
		// ampRate: 振幅率（0～1）
		public void setAmplitudeRate(float ampRate ) {
			if ( ampRate < 0.0f )
				ampRate = 0.0f;
			else if ( ampRate >= 1.0f )
				ampRate = 0.99f;
			ampRate_ = ampRate;
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

		// ワールドに波を加算
		override public void addMe(World world) {
			Vector2 wp = new Vector2(); // ワールド位置
			Vector2 tmp = new Vector2();
			float grav = 9.8f;
			float A = ( float )( waveLen_ / ( 2.0f * Math.PI ) );
			float B = ( float )( A * ampRate_ / 2.0f );
			float C = ( float )( Math.Sqrt( A * grav ) ) * sec_;
			var grid = world.Grid;
			for ( int y = 0; y < world.GridPixelHeight; ++y ) {
				for ( int x = 0; x < world.GridPixelWidth; ++x ) {
					world.getWorldPos( x, y, ref wp );
					float d = dir_.x_ * wp.x_ + dir_.y_ * wp.y_;

					// ニュートン法でθ算出
					float th = ( d - C ) / A;
					for ( int e = 0; e < 3; ++e ) {
						float fth = ( float )( A * th - B * Math.Sin( th ) + C - d );
						float dfth = ( float )( A - B * Math.Cos( th ) );
						th = th - fth / dfth;
					}
					// 波の高さ算出
					float h = ( float )( B * Math.Cos( th ) );
					grid[ x, y ] += h;
				}
			}
		}

		// クローン作成
		override public Wave clone() {
			var obj = new TrochoidStraightWave();
			obj.Degree = Degree;
			obj.WaveLength = WaveLength;
			obj.AmplitudeRate = AmplitudeRate;
			obj.phaseSpeed_ = phaseSpeed_;
			obj.setTime( Time );
			return obj;
		}

		Vector2 dir_ = new Vector2( 1.0f, 0.0f );
		float deg_ = 0.0f;
		float ampRate_ = 0.8f;
		float waveLen_ = 2.0f;
		float phaseSpeed_ = 1.0f;
	}
}
