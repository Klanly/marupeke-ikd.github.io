using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 方向キー等キーに関するヘルパー

public class KeyHelper {
	// 方向キー取得
	//  上下左右方向キーの押し下げチェック
	static public bool getArrow( ref KeyCode outKey ) {
		foreach ( var key in arrows_g ) {
			if ( Input.GetKey( key ) == true ) {
				outKey = key;
				return true;
			}
		}
		return false;
	}

	// 指定方向を座標化
	static public Vector2Int offset( KeyCode arrowKey ) {
		if ( offsets_g.ContainsKey( arrowKey ) == false )
			return Vector2Int.zero;
		return offsets_g[ arrowKey ];
	}

	// 指定方向の逆を返す
	static public KeyCode invKey( KeyCode arrowKey ) {
		if ( isArrowKey( arrowKey ) == false )
			return KeyCode.None;
		return invKeys_g[ arrowKey ];
	}

	// 方向キー？
	static public bool isArrowKey( KeyCode arrowKey ) {
		return ( arrowKey == KeyCode.LeftArrow || arrowKey == KeyCode.RightArrow | arrowKey == KeyCode.DownArrow || arrowKey == KeyCode.UpArrow );
	}

	// 2つの方向キーの間の角度を算出（-180～180)
	//  Left方向を0度とし、180度の開きがある物は+180度で返す
	static public int arrowDeg( KeyCode preArrowKey, KeyCode postArrowKey ) {
		if ( isArrowKey( preArrowKey ) == false || isArrowKey( postArrowKey ) == false ) {
			return 0;
		}
		int preDeg = rotDegrees_g[ preArrowKey ];
		int postDeg = rotDegrees_g[ postArrowKey ];
		int deg = postDeg - preDeg;
		switch ( Mathf.Abs( deg ) ) {
			case 90:
				return deg;
			case 180:
				return 180;
			case 270:
				return deg < 0 ? 90 : -90;
		}
		return deg;
	}

	static KeyCode[] arrows_g = new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow };
	static Dictionary<KeyCode, Vector2Int> offsets_g = new Dictionary<KeyCode, Vector2Int> {
		{ KeyCode.LeftArrow, new Vector2Int( -1, 0 ) },
		{ KeyCode.RightArrow, new Vector2Int( 1, 0 ) },
		{ KeyCode.DownArrow, new Vector2Int( 0, -1 ) },
		{ KeyCode.UpArrow, new Vector2Int( 0, 1 ) }
	};
	static Dictionary<KeyCode, KeyCode> invKeys_g = new Dictionary<KeyCode, KeyCode> {
		{ KeyCode.LeftArrow, KeyCode.RightArrow },
		{ KeyCode.RightArrow, KeyCode.LeftArrow },
		{ KeyCode.DownArrow, KeyCode.UpArrow },
		{ KeyCode.UpArrow, KeyCode.DownArrow }
	};
	static Dictionary<KeyCode, int> rotDegrees_g = new Dictionary<KeyCode, int> {
		{ KeyCode.LeftArrow, 0 },
		{ KeyCode.RightArrow, 180 },
		{ KeyCode.DownArrow, 270 },
		{ KeyCode.UpArrow, 90 }
	};
}
