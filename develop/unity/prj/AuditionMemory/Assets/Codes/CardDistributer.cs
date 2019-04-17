using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カード分配者
//  カードなどを平面に対してある程度の間隔で分配していく

public class CardDistributer {

    // 分配位置を作成
    //  minRadiusからmaxRadiusの間をランダムに
    //  num      : 生成数
    //  minRadius: 最小半径
    //  maxRadius: 最大半径
    //  stepUnit : 探索する時のステップ長
    public Vector3[] create( int num, float minRadius, float maxRadius, float stepUnit ) {
        if ( num <= 0 )
            return new Vector3[ 0 ];
        placeAry_ = new List< Vector3 >();
        placeAry_.Add( Vector3.zero );

        float curRadius = Random.Range( minRadius, maxRadius );
        radiusAry_ = new List<float>();
        radiusAry_.Add( curRadius );

        // 衝突判定
        //  中心からの半径が[要素番号 + 1]×[maxRadius * 2]のカテゴリーで分類
        collisions_ = new List<List<int>>();
        var colAry = new List< int >();
        colAry.Add( 0 );
        collisions_.Add( colAry );

        // 右、上、左、下の繰り返しで
        // 2ターンごとに1つ増やすといい感じ
        var dirs = new Vector3[] {
            new Vector3( stepUnit, 0.0f, 0.0f ),
            new Vector3( 0.0f, 0.0f, stepUnit ),
            new Vector3( -stepUnit, 0.0f, 0.0f ),
            new Vector3( 0.0f, 0.0f, -stepUnit )
        };
        int step = 1;
        int turn = 0;
        int len = 1;
        int curDir = 0;
        var curPos = Vector3.zero;
        int limitCount = 0;
        while ( placeAry_.Count < num && limitCount < 100000 ) {
            limitCount++;
            var dir = dirs[ curDir % 4 ];
            curPos += dir;
            // 検索場所が置ける場所だったら記録して次の半径を設定
            if ( isEnablePlace( curPos, curRadius, maxRadius )) {
                placeAry_.Add( curPos );
                radiusAry_.Add( curRadius );
                int elem = ( int )( ( curPos.magnitude + curRadius ) / ( 2.0f * maxRadius ) );
                if ( elem >= collisions_.Count ) {
                    for ( int i = collisions_.Count; i <= elem; ++i ) {
                        var ca = new List<int>();
                        collisions_.Add( ca );
                    }
                }
                collisions_[ collisions_.Count - 1 ].Add( placeAry_.Count - 1 );
                curRadius = Random.Range( minRadius, maxRadius );
            }
            step--;
            if ( step == 0 ) {
                curDir++;
                step = len;
                turn = ( turn + 1 ) % 2;
                len += turn;
            }
        }
        return placeAry_.ToArray();
    }

    bool isEnablePlace( Vector3 pos, float radius, float maxRadius ) {
        // collisionsの外側から検索
        float r = ( pos.magnitude - radius ) / ( 2.0f * maxRadius );
        if ( r <= 0.0f ) {
            return false;
        }
        int startElem = (int)( r );
        if ( startElem >= collisions_.Count ) {
            return true;
        }
        for ( int i = collisions_.Count - 1; i >= startElem; --i ) {
            var indices = collisions_[ i ];
            for ( int j = 0; j < indices.Count; ++j ) {
                int oe = indices[ j ];
                var otherPos = placeAry_[ oe ];
                var otherR = radiusAry_[ oe ];
                float dist = ( otherPos - pos ).magnitude;
                if ( dist <= radius + otherR ) {
                    // ぶつかっているのでダメ
                    return false;
                }
            }
        }
        // 衝突者無し
        return true;
    }

    List<Vector3> placeAry_;
    List<float> radiusAry_;
    List<List<int>> collisions_;
}
