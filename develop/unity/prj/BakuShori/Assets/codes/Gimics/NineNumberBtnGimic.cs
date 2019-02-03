using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 9ナンバーボタンギミック
//
//  9つの数字が記されたボタンを正しい順番に押すギミック。
//  ボタンは「数字」「ボタン背景色」「数字の色」の3つの要素があり
//  組み合わせの正しい数字を押さなければならない

public class NineNumberBtnGimic : Gimic {

    [SerializeField]
    NineNumberBtnGimicAnswer answerPrefab_;

    public enum EColor : int
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        White = 4,
        Num = 5
    }

    public class Number
    {
        public EColor fontColor_;
        public EColor bgColor_;
        public int number_;
    }

    // 作成
    override public void setParam( int randomNumber, GimicSpec gimicSpec )
    {
        digit_ = gimicSpec.nineNumberBtn_.digit_;

        // 正解作成
        answerNumbers_ = new List<Number>();
        random_ = new System.Random( randomNumber );
        for ( int i = 0; i < digit_; ++i ) {
            while( true ) {
                var number = createNumber();
                if ( isSame( number, answerNumbers_ ) == false ) {
                    answerNumbers_.Add( number );
                    break;
                }
            }
        }

        // ボタン作成
        createButtons( digit_ );

        // アンサー作成
        gimicAnswer_ = Instantiate<NineNumberBtnGimicAnswer>( answerPrefab_ );
        gimicAnswer_.setup( answerNumbers_ );
        answer_ = gimicAnswer_;
    }

    // 色番号を取得
    static public string getColorStr( EColor color )
    {
        switch ( color ) {
            case EColor.Red:    return "D23C1A";
            case EColor.Green:  return "5BD21A";
            case EColor.Blue:   return "1A7DD2";
            case EColor.Yellow: return "D0D21A";
            case EColor.White:  return "EDEDED";
        }
        return "000000";
    }

    // ボタン作成
    Number createNumber()
    {
        var number = new Number();
        number.number_ = random_.Next() % 10;
        number.fontColor_ = ( EColor )( random_.Next() % ( int )EColor.Num );
        while ( true ) {
            number.bgColor_ = ( EColor )( random_.Next() % ( int )EColor.Num );
            if ( number.bgColor_ != number.fontColor_ )
                break;
        }
        return number;
    }

    // 同じボタンがある？
    bool isSame( Number me, List< Number > numbers )
    {
        foreach ( var n in numbers ) {
            if ( n.number_ == me.number_ && n.fontColor_ == me.fontColor_ && n.bgColor_ == me.bgColor_ ) {
                // 同じ組み合わせが含まれている
                return true;
            }
        }
        return false;
    }

    // 問題ボタン列作成
    void createButtons( int digit )
    {
        for ( int d = 0; d < digit; ++d ) {
            var list = new List<Number>();
            list.Add( answerNumbers_[ d ] );
            for ( int i = 1; i < 9; ++i ) {
                while ( true ) {
                    var number = createNumber();
                    if ( isSame( number, list ) == false ) {
                        list.Add( number );
                        break;
                    }
                }
            }
            buttons_.Add( list );
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    System.Random random_;
    List<Number> answerNumbers_;
    List< List<Number> > buttons_ = new List<List<Number>>();
    NineNumberBtnGimicAnswer gimicAnswer_;
    int digit_;
}
