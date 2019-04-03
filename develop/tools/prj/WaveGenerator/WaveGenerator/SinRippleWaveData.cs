﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator {
	class SinRippleWaveData : WaveData {
		static public bool create(Dictionary<string, object> dict, List<Wave> waves ) {
			if ( MiniJsonHelper.getString( dict, "type", "" ) != "sinRipple" )
				return false;
			int num = MiniJsonHelper.getInt( dict, "num", 1 );
			float[] radius = MiniJsonHelper.getFloatArray( dict, "radius" );
			float[] waveLength = MiniJsonHelper.getFloatArray( dict, "waveLength" );
			float[] amplitude = MiniJsonHelper.getFloatArray( dict, "amplitude" );
			float[] peakedPower = MiniJsonHelper.getFloatArray( dict, "peakedPower" );
			float[] phase = MiniJsonHelper.getFloatArray( dict, "phase" );
			float[] sec = MiniJsonHelper.getFloatArray( dict, "sec" );

			// 生成
			var rand = new Random();
			for ( int i = 0; i < num; ++i ) {
				var wave = new SinRippleWave();
				float rad = ( float )( rand.NextDouble() * 2.0 * Math.PI );
				float r = frand( radius, rand );
				Vector2 center = new Vector2();
				wave.Center = circleRrand( radius, rand, ref center );
				wave.WaveLength = frand( waveLength, rand );
				wave.Amplitude = frand( amplitude, rand );
				wave.PeakedPower = frand( peakedPower, rand );
				wave.PhaseSpeed = frand( phase, rand );
				wave.setTime( frand( sec, rand ) );
				waves.Add( wave );
			}

			return true;
		}
	}
}