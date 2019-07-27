using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDistributer
{
	protected class BlockInfo
	{
		public Block block_ = new Block();
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
                if ( info[ x, y ] != null )
                    blocks[ x, y ] = info[ x, y ].block_;
                else {
                    blocks[ x, y ] = new Block();
                }
            }
		}
		return blocks;
	}

	// フィールド生成
	protected virtual void create( BlockFieldParameter param, ref BlockInfo[,] info ) {
        // ダイヤモンドを配置
        //  プレイヤーの位置( 0, 0 )から一定以上の距離を保ち且つ各ダイヤモンド間も一定距離を保って配置
        {
            // 指定数の3倍くらい作成して調整
            var playerIgnore = new RandomPlace.IgnoreCircle( param.playerPos_, param.diamond_.intervalForPlayer_ );
            var ignoreList = new List< RandomPlace.IgnoreShape >() { playerIgnore };
            var poses = RandomPlace.distanceBase( param.regionMin_, param.regionMax_, param.diamond_.interval_, param.diamond_.num_ * 3, ignoreList );
            var diamondPoses = new List<Vector2Int>();
            int count = 0;
            foreach ( var p in poses ) {
                int x = ( int )p.x;
                int y = ( int )p.y;
                var bi = new BlockInfo();
                bi.bUpdateLock_ = false;
                bi.block_.hp_ = param.diamond_.HP_;
                bi.block_.type_ = Block.Type.Juel0;
                info[ x, y ] = bi;
                count++;
                if ( count >= param.diamond_.num_ )
                    break;
            }
        }

        // サファイヤを配置
        {
            var poses = RandomPlace.distanceBase( param.regionMin_, param.regionMax_, param.sapphire_.interval_, param.sapphire_.num_ );
            var diamondPoses = new List<Vector2Int>();
            int count = 0;
            foreach ( var p in poses ) {
                int x = ( int )p.x;
                int y = ( int )p.y;
                var bi = new BlockInfo();
                bi.bUpdateLock_ = false;
                bi.block_.hp_ = param.sapphire_.HP_;
                bi.block_.type_ = Block.Type.Juel1;
                info[ x, y ] = bi;
                count++;
                if ( count >= param.sapphire_.num_ )
                    break;
            }
        }
    }
}
