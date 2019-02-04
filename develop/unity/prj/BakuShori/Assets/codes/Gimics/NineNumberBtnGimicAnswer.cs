using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 9ナンバーボタンギミックアンサー

public class NineNumberBtnGimicAnswer : Answer {

    [SerializeField]
    TextMesh numberText_;

    public void setup( List<NineNumberBtnGimic.Number> numbers )
    {
        ObjectType = EObjectType.GimicAnswer;
        string numStr = "";
        foreach ( var n in numbers ) {
            numStr += string.Format( "<color=#{1}>{0}</color>", n.number_, NineNumberBtnGimic.getColorStr( n.fontColor_) );
        }
        numberText_.text = numStr;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
