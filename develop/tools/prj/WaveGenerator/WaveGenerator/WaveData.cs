using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator {
	class WaveData {
		static protected float frand( float s, float e, Random r ) {
			return ( float )( s + ( e - s ) * r.NextDouble() );
		}
		static protected float frand( float[] values, Random r ) {
			return frand( values[ 0 ], values[ 1 ], r );
		}
		// 円内一様乱数
		static protected Vector2 circleRrand( float[] radius, Random r, ref Vector2 outPos ) {
			float rs = radius[ 0 ];
			float re = radius[ 1 ];
			float radian = ( float )( 2.0 * Math.PI * r.NextDouble() );
			if ( rs == re ) {
				// 円周上一様乱数
				outPos.x_ = ( float )( rs * Math.Cos( radian ) );
				outPos.y_ = ( float )( rs * Math.Sin( radian ) );
				return outPos;
			}
			if ( re < rs ) {
				float tmp = rs;
				rs = re;
				re = tmp;
			}
			float hp = ( float )( Math.PI * 0.5f );
			float rs2 = rs * rs;
			float re2 = re * re;
			float S = hp * ( rs2 + re2 ) * ( re - rs );
			float A = hp * ( re2 - rs2 ) / ( re - rs ) / S;
			float B = 2 * hp * rs2 / S;
			float rad = ( float )( ( -B + Math.Sqrt( B * B + 4.0 * A * r.NextDouble() ) ) / ( 2.0 * A ) ) + rs;
			outPos.x_ = rad * ( float )( Math.Cos( radian ) );
			outPos.y_ = rad * ( float )( Math.Sin( radian ) );
			return outPos;
		}
	}
}
