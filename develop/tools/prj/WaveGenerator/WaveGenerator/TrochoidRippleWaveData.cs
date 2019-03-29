using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator {
	class TrochoidRippleWaveData : WaveData {
		static public bool create(Dictionary<string, object> dict, List<Wave> waves) {
			if ( MiniJsonHelper.getString( dict, "type", "" ) != "trochoidRipple" )
				return false;
			int num = MiniJsonHelper.getInt( dict, "num", 1 );
			float[] radius = MiniJsonHelper.getFloatArray( dict, "radius" );
			float[] waveLength = MiniJsonHelper.getFloatArray( dict, "waveLength" );
			float[] amplitudeRate = MiniJsonHelper.getFloatArray( dict, "amplitudeRate" );
			float[] phase = MiniJsonHelper.getFloatArray( dict, "phase" );
			float[] sec = MiniJsonHelper.getFloatArray( dict, "sec" );

			// 生成
			var rand = new Random();
			for ( int i = 0; i < num; ++i ) {
				var wave = new TrochoidRippleWave();
				float rad = ( float )( rand.NextDouble() * 2.0 * Math.PI );
				float r = frand( radius, rand );
				wave.Center = new Vector2( ( float )( r * Math.Cos( rad ) ), ( float )( r * Math.Sin( rad ) ) );
				wave.WaveLength = frand( waveLength, rand );
				wave.AmplitudeRate = frand( amplitudeRate, rand );
				wave.PhaseSpeed = frand( phase, rand );
				wave.setTime( frand( sec, rand ) );
				waves.Add( wave );
			}

			return true;
		}
	}
}
