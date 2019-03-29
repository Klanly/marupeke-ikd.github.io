using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator {
	class TrochoidStraightWaveData : WaveData {
		static public bool create(Dictionary<string, object> dict, List<Wave> waves) {
			if ( MiniJsonHelper.getString( dict, "type", "" ) != "trochoidStraight" )
				return false;
			int num = MiniJsonHelper.getInt( dict, "num", 1 );
			float[] degree = MiniJsonHelper.getFloatArray( dict, "degree" );
			float[] waveLength = MiniJsonHelper.getFloatArray( dict, "waveLength" );
			float[] amplitudeRate = MiniJsonHelper.getFloatArray( dict, "amplitudeRate" );
			float[] phase = MiniJsonHelper.getFloatArray( dict, "phase" );
			float[] sec = MiniJsonHelper.getFloatArray( dict, "sec" );

			// 生成
			var rand = new Random();
			for ( int i = 0; i < num; ++i ) {
				var wave = new TrochoidStraightWave();
				wave.Degree = frand( degree, rand );
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
