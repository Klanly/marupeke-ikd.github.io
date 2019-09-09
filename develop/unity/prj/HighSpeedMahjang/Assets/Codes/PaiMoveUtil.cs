using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 牌動作ヘルパー
public class PaiMoveUtil
{
	public float UnitX { set { unitX_ = value; } }
	public float UnitY { set { unitY_ = value; } }
	public float PaiH { set { paiH_ = value; } }

	float unitX_ = 1.0f;
	float unitY_ = 1.0f;
	float paiH_ = 1.0f;	// 牌の高さ（通常unitYと一緒）

	// 牌の位置をインデックスに変換
	public Vector2Int convPosToIdx( Vector3 pos ) {
		return new Vector2Int( (int)Mathf.Floor( pos.x / unitX_ - 0.5f ), ( int )Mathf.Floor( pos.y / unitY_ - 0.5f ) );
	}

    // インデックスをセル中心座標に変換
    public Vector3 convIdxToPos(Vector2 idx) {
		return new Vector3( ( idx.x + 0.5f ) * unitX_, ( idx.y + 0.5f ) * unitY_, 0.0f );
	}

    // 牌中心座標からセル中心座標を算出
    public Vector3 calcCellCenter(Vector3 pos) {
		return convIdxToPos( convPosToIdx( pos ) );
	}

    // 配置可能？
    public bool enablePlace<T>(Vector3 pos, T[,] field) where T : class {
		var idx = convPosToIdx( pos );
		// そもそも位置が不正？
		if (idx.x < 0 || idx.x >= field.GetLength( 0 ) || idx.y < 0 || idx.y >= field.GetLength( 1 )) {
			return false;
		}
		// 牌が所属するidxにすでに牌がある場合はNG
		if (field[ idx.x, idx.y ] != null) {
			return false;
		}
		return true;
	}

    // フォール可能？
    // pos      : 落下前の牌の中心位置
    // fallDist : 落下距離
    // field    : フィールド領域
    public bool enableFall<T>( Vector3 pos, float fallDist, T[,] field ) where T : class {
		return enablePlace<T>( pos - new Vector3( 0.0f, fallDist, 0.0f ), field );
	}

    // 接地の瞬間？
    // pos      : 落下前の牌の中心位置
    // fallDist : 落下距離
    // field    : フィールド領域
    // 戻り値   : posが配置可能位置でfallDist分落下すると接地する場合はtrue
    //            posが配置不能もしくは落下しても接地しない場合はfalse
    public bool isTouchDown<T>(Vector3 pos, float fallDist, T[,] field) where T : class {
		if ( enablePlace<T>( pos, field ) == false ) {
			return false;
		}
		return !enableFall<T>( pos, fallDist, field );
	}

    // 牌が落下可能な最下部インデックスを取得
    //  outIdx: 落下可能最下部インデックス
    //  戻り値: 落下出来る状態ならtrue、pos位置から落下不可能ならfalse
    public bool calcBottomIndex<T>( Vector3 pos, T[,] field, out Vector2Int outIdx ) where T : class {
		outIdx = new Vector2Int();
		if ( enablePlace( pos, field ) == false ) {
			return false;
		}
		outIdx = convPosToIdx( pos );
		for ( int y = outIdx.y; y >= 0; y-- ) {
			outIdx.y = y;
			if ( field[ outIdx.x, y] != null ) {
				return true;
			}
		}
		return true;	// y = 0
	}

    // 牌が落下可能な最下部中心座標を取得
    //  outPos: 落下可能最下部座標
    //  戻り値: 落下出来る状態ならtrue、pos位置から落下不可能ならfalse
    public bool calcBottomIndex<T>(Vector3 pos, T[,] field, out Vector3 outPos) where T : class {
		outPos = Vector3.zero;
		Vector2Int idx;
		if ( calcBottomIndex<T>( pos, field, out idx) == false ) {
			return false;
		}
		outPos = convIdxToPos( idx );
		return true;
	}
}
