using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 9ナンバーボタンギミックアンサー

public class NineNumberBtnGimicAnswer : Answer {

    [SerializeField]
    TextMesh numberText_;

    [SerializeField]
    TextMesh gimicNumberText_;

    [SerializeField]
    TextMesh gimicBGText_;

    override public int Index {
        set {
            index_ = value;
            setGimicIdText( value );
        }
        get {
            return index_;
        }
    }

    public void setup( int gimicId, List<NineNumberBtnGimic.Number> numbers )
    {
        ObjectType = EObjectType.GimicAnswer;
        string numStr = "";
        string bgStr = "";
        foreach ( var n in numbers ) {
            numStr += string.Format( "<color=#{1}>{0}</color>", n.number_, NineNumberBtnGimic.getColorStr( n.fontColor_) );
            bgStr += string.Format( "<color=#{0}>■</color>", NineNumberBtnGimic.getColorStr( n.bgColor_ ) );
        }
        gimicBGText_.text = bgStr;
        numberText_.text = numStr;

        // Idを5ビットで表現
        setGimicIdText( gimicId );
    }

    void setGimicIdText( int gimicId )
    {
        string s = "";
        for ( int i = 0; i < 5; ++i ) {
            s = ( gimicId % 2 == 0 ? "_" : "■" ) + " " + s;
            gimicId >>= 1;
        }
        gimicNumberText_.text = s;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
