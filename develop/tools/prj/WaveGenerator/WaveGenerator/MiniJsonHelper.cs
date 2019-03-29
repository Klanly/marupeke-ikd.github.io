using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator {
	class MiniJsonHelper {
		static public int getInt( Dictionary< string, object > dict, string name, int init ) {
			if ( dict.ContainsKey( name ) == false )
				return init;
			return StrConv.toInt( dict[ name ].ToString(), init );
		}
		static public float getFloat(Dictionary<string, object> dict, string name, float init) {
			if ( dict.ContainsKey( name ) == false )
				return init;
			return StrConv.toFloat( dict[ name ].ToString(), init );
		}
		static public string getString(Dictionary<string, object> dict, string name, string init) {
			if ( dict.ContainsKey( name ) == false )
				return init;
			return dict[ name ].ToString();
		}
		static public Dictionary<string, object> getChild(Dictionary<string, object> dict, string name ) {
			if ( dict.ContainsKey( name ) == false )
				return null;
			return dict[ name ] as Dictionary<string, object>;
		}
		static public List<object> getList(Dictionary<string, object> dict, string name ) {
			if ( dict.ContainsKey( name ) == false )
				return null;
			return dict[ name ] as List<object>;
		}
		static public int[] getIntArray(Dictionary<string, object> dict, string name) {
			if ( dict.ContainsKey( name ) == false )
				return null;
			var array = dict[ name ] as List<object>;
			if ( array == null )
				return new int[ 0 ];
			int[] outArray = new int[ array.Count ];
			for ( int i = 0; i < array.Count; ++i ) {
				outArray[ i ] = StrConv.toInt( array[ i ].ToString(), 0 );
			}
			return outArray;
		}
		static public float[] getFloatArray(Dictionary<string, object> dict, string name) {
			if ( dict.ContainsKey( name ) == false )
				return null;
			var array = dict[ name ] as List<object>;
			if ( array == null )
				return new float[ 0 ];
			float[] outArray = new float[ array.Count ];
			for ( int i = 0; i < array.Count; ++i ) {
				outArray[ i ] = StrConv.toFloat( array[ i ].ToString(), 0.0f );
			}
			return outArray;
		}
		static public string[] getStringArray(Dictionary<string, object> dict, string name) {
			if ( dict.ContainsKey( name ) == false )
				return null;
			var array = dict[ name ] as List<object>;
			if ( array == null )
				return new string[ 0 ];
			string[] outArray = new string[ array.Count ];
			for ( int i = 0; i < array.Count; ++i ) {
				outArray[ i ] = array[ i ].ToString();
			}
			return outArray;
		}
	}
}
