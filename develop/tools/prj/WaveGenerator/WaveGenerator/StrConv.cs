using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 文字列変換ヘルパー
class StrConv
{
	static public float toFloat( string str, float init ) {
		float.TryParse( str, out init );
		return init;
	}
	static public int toInt(string str, int init) {
		int.TryParse( str, out init );
		return init;
	}
}