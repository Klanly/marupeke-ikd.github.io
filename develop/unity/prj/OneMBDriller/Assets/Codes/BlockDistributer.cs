using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDistributer
{
	protected class BlockInfo
	{
		public Block block_;
		public bool bUpdateLock_ = false;	// 上書きして良い？
	}

	// フィールド生成
	public Block[,] createField( BlockFieldParameter param ) {

		// 生成
		BlockInfo[,] info = new BlockInfo[ param.sepX_ , param.sepY_ ];
		create( param, ref info );

		// 出力
		Block[,] blocks = new Block[ param.sepX_, param.sepY_ ];
		for ( int y = 0; y < param.sepY_; ++y ) {
			for ( int x = 0; x < param.sepX_; ++x ) {
				blocks[ x, y ] = info[ x, y ].block_;
			}
		}
		return blocks;
	}

	// フィールド生成
	protected virtual void create( BlockFieldParameter param, ref BlockInfo[,] info ) {
        // ダイヤモンドを配置
        //  プレイヤーの位置から一定以上の距離を保ち且つ各ダイヤモンド間も一定距離を保って配置
        {
            var diamondPoses = new List<Vector2Int>();
            var p = Vector2Int.zero;
            for ( int i = 0; i < param.diamondNum_; ++i ) {
                for ( int j = 0; j < 100; ++j ) {
                    float th = Mathf.PI * 2.0f * Random.value;
                    p.x = Random.Range( 0, param.sepX_ );
                    p.y = Random.Range( 0, param.sepY_ );
                }
            }
        }

    }
}
