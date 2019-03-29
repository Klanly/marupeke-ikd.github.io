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
	}
}
