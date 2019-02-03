using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoxModel : MonoBehaviour {

    [SerializeField]
    GimicBoxCover[] covers_;

    [SerializeField]
    OnAction redLine_;

    [SerializeField]
    GameObject redLineCut_;

    [SerializeField]
    OnAction blueLine_;

    [SerializeField]
    GameObject blueLineCut_;

    enum CutLine
    {
        Red,
        Blue
    }

    // ギミックを格納できる場所の数を取得
    public int getGimicBoxPlaceNum()
    {
        return covers_.Length;
    }

    // 蓋をオープン
    public bool openCover( int id )
    {
        if ( id >= covers_.Length )
            return false;
        covers_[ id ].open();
        return true;
    }

	// Use this for initialization
	void Start () {
        redLineCut_.SetActive( false );
        blueLineCut_.SetActive( false );

        // 赤青ラインカットイベント
        redLine_.ActionCallback = (obj, eventName) => {
            cutRB( CutLine.Red );
        };
        blueLine_.ActionCallback = (obj, eventName) => {
            cutRB( CutLine.Blue );
        };
    }

    // Update is called once per frame
    void Update () {
		
	}

    // 赤青ラインカット
    void cutRB( CutLine color )
    {
        // カットされた方を切れたモデルに変更
        if ( color == CutLine.Blue ) {
            blueLine_.gameObject.SetActive( false );
            blueLineCut_.SetActive( true );
        } else {
            redLine_.gameObject.SetActive( false );
            redLineCut_.SetActive( true );
        }
    }
}
