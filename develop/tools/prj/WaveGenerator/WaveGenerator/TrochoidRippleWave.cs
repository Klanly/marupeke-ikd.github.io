using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// トロコイド環状波

namespace WaveGenerator {
	class TrochoidRippleWave : Wave {
		public TrochoidRippleWave() {
			waveType_ = EWaveType.TrochoidRipple;
		}

		// 中心点を設定
		//  x, y: ワールドスケール座標での中心位置
		public Vector2 Center { set { center_ = value; } get { return center_; } }

		public float AmplitudeRate { set { setAmplitudeRate( value ); } get { return ampRate_; } }

		public float WaveLength { set { setWaveLen( value ); } get { return waveLen_; } }

		public float PhaseSpeed { set { setPhaseSpeed( value ); } get { return phaseSpeed_; } }

		// 波長に対する振幅率を設定
		// amp: 振幅率（0～1）
		public void setAmplitudeRate(float ampRate) {
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
			float amp = ( float )( waveLen_ / ( 2.0f * Math.PI ) ) * ampRate_;
			float A = ( float )( waveLen_ / ( 2.0f * Math.PI ) );
			var grid = world.Grid;
			for ( int y = 0; y < world.GridPixelHeight; ++y ) {
				for ( int x = 0; x < world.GridPixelWidth; ++x ) {
					world.getWorldPos( x, y, ref wp );
					float d = center_.sub( ref tmp, wp ).Len;

					// ニュートン法でθ算出
					float th = ( float )( 2.0f * Math.PI / waveLen_ * ( d - phaseSpeed_ * sec_ ) );
					for ( int e = 0; e < 3; ++e ) {
						float fth = ( float )( A * th - amp * Math.Sin( th ) + phaseSpeed_ * sec_ - d );
						float dfth = A - ( float )( amp * Math.Cos( th ) );
						th = th - fth / dfth;
					}
					// 波の高さ算出
					float v = ( float )( amp * ( 1.0f + Math.Cos( th ) ) * 0.5f );
					grid[ x, y ] += v;
				}
			}
		}

		// クローン作成
		override public Wave clone() {
			var obj = new TrochoidRippleWave();
			obj.Center = Center;
			obj.WaveLength = WaveLength;
			obj.AmplitudeRate = AmplitudeRate;
			obj.phaseSpeed_ = phaseSpeed_;
			obj.setTime( Time );
			return obj;
		}


		Vector2 center_ = new Vector2();
		float ampRate_ = 0.8f;
		float waveLen_ = 2.0f;
		float phaseSpeed_ = 1.0f;
	}
}
