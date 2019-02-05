using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoxModel : MonoBehaviour {

    [SerializeField]
    GimicBoxCover[] covers_;

    [SerializeField]
    GameObject[] coverNumberPoses_;

    [SerializeField]
    GameObject[] gimicBoxPoses_;

    [SerializeField]
    GameObject[] trapPoses_;

    [SerializeField]
    OnAction redLine_;

    [SerializeField]
    GameObject redLineCut_;

    [SerializeField]
    OnAction blueLine_;

    [SerializeField]
    GameObject blueLineCut_;

    [SerializeField]
    GameObject[] bombBoxAnswerNodes_;

    [SerializeField]
    TextMesh coverNumberPrefab_;

    enum CutLine
    {
        Red,
        Blue
    }

    [System.Serializable]
    public class AnswerNodes
    {
        public GameObject[] nodes_;
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

    // ギミックを格納できるGBのTransformを取得
    public Transform getGimicBoxTrans( int id )
    {
        if ( id >= gimicBoxPoses_.Length )
            return null;
        return gimicBoxPoses_[ id ].transform;
    }

    // ギミックボックスのAnswerノードを取得
    public GameObject getGimicBoxAnswerTrans( int gimicBoxId, int answerId )
    {
        if ( gimicBoxId >= covers_.Length )
            return null;
        if ( answerId >= covers_[ gimicBoxId ].getAnswerNodeNum() )
            return null;
        return covers_[ gimicBoxId ].getAnswerPos( answerId );
    }

    // ギミックボックスのトラップノードを取得
    public GameObject getTrapTrans( int gimicBoxId )
    {
        if ( gimicBoxId >= trapPoses_.Length )
            return null;
        return trapPoses_[ gimicBoxId ];
    }

    // BombBox表面のAnswerノードを取得
    public GameObject getBombBoxAnswerNode( int id )
    {
        if ( id >= bombBoxAnswerNodes_.Length )
            return null;
        return bombBoxAnswerNodes_[ id ];
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

        // 蓋の表面にナンバーを刻印
        for ( int i = 0; i < coverNumberPoses_.Length; ++i ) {
            var cn = Instantiate<TextMesh>( coverNumberPrefab_ );
            var pos = coverNumberPoses_[ i ];
            var to = cn.gameObject.AddComponent<TransObserver>();
            to.setTarget( pos.transform );
            cn.transform.parent = transform;
            cn.text = string.Format( "{0}", i );
        }
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
