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

    [SerializeField]
    GameObject frontPanel_;

    [SerializeField]
    bool bDebugOpenFrontPanel_;

    [SerializeField]
    RBLamp rbLamp_;

    [SerializeField]
    BombTimer[] bombTimers_;

    // RBカット通知コールバック
    public System.Action<CutLine, int> CutRBCallback { set { cutRBCallback_ = value; } }

    public enum CutLine
    {
        Red,
        Blue
    }

    [System.Serializable]
    public class AnswerNodes
    {
        public GameObject[] nodes_;
    }

    // タイマー取得
    public BombTimer getTimer()
    {
        // 最初の渡す
        return bombTimers_[ 0 ]; 
    }

    // フロントパネル開いた？
    public bool isOpenFrontPanel()
    {
        return bOpenFrontPanel_;
    }

    // タイマーを急激に減少
    public void advanceTimer(System.Action notifyZero)
    {
        foreach ( var t in bombTimers_ ) {
            t.advanceTimer( notifyZero );
            notifyZero = null;
        }
    }

    // タイマーをストップ
    public void stopTimer()
    {
        foreach ( var t in bombTimers_ ) {
            t.stopTimer();
        }
    }

    // 全面パネルをオープン
    public void openFrontPanel()
    {
        var startPos = frontPanel_.transform.localPosition;
        var endPos = new Vector3( -0.02447f, -0.013f, 0.0f );
        var startQ = frontPanel_.transform.localRotation;
        var endQ = Quaternion.Euler( 30.69f, -90.0f, 0.0f );

        GlobalState.time( 0.75f, (sec, t) => {
            var p = Vector3.Lerp( startPos, endPos, t );
            var q = Quaternion.Lerp( startQ, endQ, t );
            frontPanel_.transform.localPosition = p;
            frontPanel_.transform.localRotation = q;
            return true;
        });

        bOpenFrontPanel_ = true;
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

    // 赤青ランプを取得
    public RBLamp getRBLamp()
    {
        return rbLamp_;
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
		if ( bDebugOpenFrontPanel_ == true ) {
            bDebugOpenFrontPanel_ = false;
            openFrontPanel();
        }
	}

    // 赤青ラインカット
    void cutRB( CutLine color )
    {
        // カットされた方を切れたモデルに変更
        if ( bCutBlue_ == false && color == CutLine.Blue ) {
            blueLine_.gameObject.SetActive( false );
            blueLineCut_.SetActive( true );
            bCutBlue_ = true;
            cutRBCallback_( color, rbLamp_.getCurTiming( color ) );
        } else if ( bCutRed_ == false && color == CutLine.Red ) {
            redLine_.gameObject.SetActive( false );
            redLineCut_.SetActive( true );
            bCutRed_ = true;
            cutRBCallback_( color, rbLamp_.getCurTiming( color ) );
        }
    }

    bool bOpenFrontPanel_ = false;
    bool bCutBlue_ = false;
    bool bCutRed_ = false;
    System.Action<CutLine, int> cutRBCallback_;
}
