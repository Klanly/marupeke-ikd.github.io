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

    [SerializeField]
    OnAction[] buttons_;

    [SerializeField]
    TextMesh[] buttonTexts_;

    [SerializeField]
    MeshRenderer[] buttonBGs_;

    [SerializeField]
    MeshRenderer redLamp_;

    [SerializeField]
    MeshRenderer greenLamp_;

    [SerializeField]
    Material offMaterial_;

    [SerializeField]
    bool debugDiactive_ = false;

    override public int Index {
        set {
            index_ = value;
            answer_.Index = index_;
        }
        get {
            return index_;
        }
    }

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

        // ボタンナンバー列作成
        createButtons( randomNumber, digit_ );
        resetButtonSet( 0 );

        // アンサー作成
        gimicAnswer_ = Instantiate<NineNumberBtnGimicAnswer>( answerPrefab_ );
        gimicAnswer_.setup( Index, answerNumbers_ );
        answer_ = gimicAnswer_;

        // ボタンアクション登録
        for ( int i = 0; i < buttons_.Length; ++i ) {
            int e = i;
            buttons_[ i ].ActionCallback = ( caller, eventStr ) => {
                if ( curDigit_ >= digit_ )
                    return;

                // 押されたボタンのナンバー
                var number = buttonNumbers_[ curDigit_ ][ e ];
                var ans = answerNumbers_[ curDigit_ ];
                if ( 
                    ans.number_ != number.number_ ||
                    ans.fontColor_ != number.fontColor_ ||
                    ans.bgColor_ != number.bgColor_
                ) {
                    // 不正解
                    failureCallback_();
                    return;
                }

                // 正解
                // 押されたボタンの押し下げ動作
                var pos = buttons_[ e ].transform.localPosition;
                float z = pos.z;
                GlobalState.time( 0.25f, (sec, t) => {
                    var p = pos;
                    p.z = Mathf.Lerp( z, 0.0f, t );
                    buttons_[ e ].transform.localPosition = p;
                    return true;
                } ).nextTime( 0.25f, (sec, t) => {
                    var p = pos;
                    p.z = Mathf.Lerp( 0.0f, z, t );
                    buttons_[ e ].transform.localPosition = p;
                    return true;
                }).finish( () => {
                    resetButtonSet( curDigit_ );
                } );
            
                //  最後のボタンだったらギミック解除
                if ( curDigit_ + 1 == digit_ ) {
                    diactiveGimic();
                }

                // 次のボタンへ
                curDigit_++;
            };
        }
    }

    // ギミック解除
    void diactiveGimic()
    {
        // ランプの色を青色に
        greenLamp_.material = greenLampMat_;
        redLamp_.material = offMaterial_;
        successCallback_();
        Debug.Log( "NineNumberGimic was diacgived !" );
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

    // 色を取得
    static public Color getColor( EColor color )
    {
        switch ( color ) {
            case EColor.Red:
                return new Color( ( int )0xD2 / 255.0f, ( int )0x3C / 255.0f, ( int )0x1A / 255.0f );
            case EColor.Green:
                return new Color( ( int )0x5B / 255.0f, ( int )0xD2 / 255.0f, ( int )0x1A / 255.0f );
            case EColor.Blue:
                return new Color( ( int )0x1A / 255.0f, ( int )0x7D / 255.0f, ( int )0xD2 / 255.0f );
            case EColor.Yellow:
                return new Color( ( int )0xD0 / 255.0f, ( int )0xD2 / 255.0f, ( int )0x1A / 255.0f );
            case EColor.White:
                return new Color( ( int )0xED / 255.0f, ( int )0xED / 255.0f, ( int )0xED / 255.0f );
        }
        return Color.black;
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

    // ボタンセットを変更
    void resetButtonSet( int digit )
    {
        if ( digit >= digit_ )
            return;
        var numbers = buttonNumbers_[ digit ];
        for ( int i = 0; i < numbers.Count; ++i ) {
            var n = numbers[ i ];
            buttonTexts_[ i ].text = string.Format( "<color=#{0}>{1}</color>", getColorStr( n.fontColor_ ), n.number_ );

            // ボタン背景
            var mat = buttonBGs_[ i ].materials[ 1 ];
            var bg = getColor( n.bgColor_ );
            mat.color = bg;
            buttonBGs_[ i ].materials[ 1 ] = mat;
        }
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
    void createButtons( int seed, int digit )
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
            ListUtil.shuffle<Number>( ref list );
            buttonNumbers_.Add( list );
        }
    }

    // Use this for initialization
    void Start () {
        // Greenランプをオフに
        greenLampMat_ = greenLamp_.material;
        greenLamp_.material = offMaterial_;

    }
	
	// Update is called once per frame
	void Update () {
		if ( debugDiactive_ == true ) {
            debugDiactive_ = false;
            diactiveGimic();
        }
	}

    System.Random random_;
    List<Number> answerNumbers_;
    List< List<Number> > buttonNumbers_ = new List<List<Number>>();
    NineNumberBtnGimicAnswer gimicAnswer_;
    int digit_;
    int curDigit_ = 0;
    Material greenLampMat_;
}
