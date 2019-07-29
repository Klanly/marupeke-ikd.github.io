using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロック管理者
//
//  指定のメモリブロックに定義されている情報から
//  指定座標のブロックの情報を取得及び設定する。
public class BlockManager
{
	// セットアップ
	//  param         : ブロックフィールドのパラメータ
	//  dist          : ブロック分配アルゴリズム
	//  finishCallback: 終了コールバック
	void setup( BlockFieldParameter param, BlockDistributer dist, System.Action finishCallback ) {
		param_ = param;

		/* 作成作業 */
		blocks_ = dist.createField( param );

		// new Block[ param.sepX_, param.sepY_ ];

		if ( finishCallback != null )
			finishCallback();
	}

	// 指定実座標のブロックを取得
	//  範囲外だった場合は壁ブロック（Lock_Wall）を返す
	bool getBlock( Vector2 coord, out Block block ) {
		return getBlock( coord.x, coord.y, out block );
	}

	bool getBlock( float x, float y, out Block block) {
		if (
			x < param_.regionMin_.x ||
			x > param_.regionMax_.x ||
			y < param_.regionMin_.y ||
			y > param_.regionMax_.y
		) {
			block = nullBlock_;
			return false;
		}
		int ix = ( int )( ( x - param_.regionMin_.x ) / ( param_.regionMax_.x - param_.regionMin_.x ) );
		int iy = ( int )( ( y - param_.regionMin_.y ) / ( param_.regionMax_.y - param_.regionMin_.y ) );
		ix = ( ix >= param_.sepX_ ? param_.sepX_ - 1 : ix );
		iy = ( iy >= param_.sepY_ ? param_.sepY_ - 1 : iy );

		block = blocks_[ ix, iy ];

		return true;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	BlockFieldParameter param_;
	Block[,] blocks_;
	Block nullBlock_ = new Block( Block.Type.Lock_Wall, 0, 0 );
}
