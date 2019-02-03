using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックレイアウト生成
//
//  爆弾ボックスの周りに配置するギミック、ギミックボックス、ボックスギミックネジ、解答の
//  配置を生成します。

public class GimicLayoutGenerator : MonoBehaviour {

    [SerializeField]
    BombBoxFactory bombBoxFactory_;

    [SerializeField]
    GimicBoxFactory gimicBoxFactory_;

    [SerializeField]
    GimicFactory gimicFactory_;

    // ギミック配置作成
    public bool create( LayoutSpec spec, GimicSpec gimicSpec, out BombBox outBombBox )
    {
        outBombBox = null;

        var bombBox = bombBoxFactory_.create( spec );
        if ( bombBox == null ) {
            Debug.LogAssertion( "Failed to create BombBox." );
            return false;
        }

        var gimicBoxes = gimicBoxFactory_.create( spec );
        if ( gimicBoxes == null ) {
            Debug.LogAssertion( "Failed to create GimicBox." );
            return false;
        }

        var gimics = gimicFactory_.create( spec, gimicSpec );
        if ( gimics == null || gimics.Count != gimicBoxes.Count ) {
            Debug.LogAssertion( "Failed to create Gimic." );
            return false;
        }

        // BombBoxに最初のギミックボックスの答えを登録
        var answer = gimicBoxes[ 0 ].getTrapAnswer();
        if ( answer == null ) {
            Debug.LogAssertion( "GimicLayoutGenerator: error: no answer in GimicBox." );
            return false;
        }
        bombBox.setEntity( answer );
        answer.setEntity( gimicBoxes[ 0 ] );        // 対応するギミックボックスを答えの下に
        gimicBoxes[ 0 ].setGimic( gimics[ 0 ] );    // ギミックボックスにギミックを登録

        for ( int i = 1; i < gimicBoxes.Count; ++i ) {
            // i番目のギミックボックスの答えを全ストックのどこかに設定
            var stocks = bombBox.getEmptyStocks( true );
            if ( stocks.Count == 0 ) {
                Debug.LogAssertion( "GimicLayoutGenerator: error: no stock under BombBox." );
                return false;
            }
            answer = gimicBoxes[ i ].getTrapAnswer();
            var stock = stocks[ Random.Range( 0, stocks.Count ) ];
            stock.setEntity( answer );

            // アンサーに対応するギミックボックスをアンサーの子に
            answer.setEntity( gimicBoxes[ i ] );

            // ギミックボックスにギミックをセット
            gimicBoxes[ i ].setGimic( gimics[ i ] );
        }

        // ギミックのアンサーを設定
        // ギミックが格納されているギミックボックス内及びその子のストックには登録できない（解決出来ない状態になるため）
        for ( int i = 0; i < gimics.Count; ++i ) {
            // i番目のギミックのアンサーが登録可能なストックを列挙
            var stocks = gimicBoxes[ i ].getParentAndTheOtherEmptyStocks();
            if ( stocks.Count == 0 ) {
                Debug.LogAssertion( "GimicLayoutGenerator: error: no stock for Answer under BombBox." );
                return false;
            }
            answer = gimics[ i ].getAnswer();
            if ( answer == null ) {
                Debug.LogAssertion( "GimicLayoutGenerator: error: Gimic has no Answer." );
                return false;
            }
            var stock = stocks[ Random.Range( 0, stocks.Count ) ];
            stock.setEntity( answer );
        }

        // BombBox内のギミックネジの答えを登録
        {
            var screws = bombBox.getGimicScrewes();
            var stocks = bombBox.getEmptyStocks( true );
            if ( stocks.Count < screws.Count ) {
                Debug.LogAssertion( "GimicLayoutGenerator: error: no stock for ScrewAnswer on BombBox." );
                return false;
            }
            for ( int i = 0; i < screws.Count; ++i ) {
                answer = screws[ i ].getAnswer();
                var stock = stocks[ Random.Range( 0, stocks.Count ) ];
                stock.setEntity( answer );
                stocks.Remove( stock );
            }
        }

        bombBox.buildBox();
        outBombBox = bombBox;

        return true;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
