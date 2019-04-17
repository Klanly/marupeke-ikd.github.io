using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Speaker speakerPrefab_;

    [SerializeField]
    Transform root_;

    [SerializeField]
    Player player0_;

    [SerializeField]
    UnityEngine.UI.Text scoreText_;

    [SerializeField]
    UnityEngine.UI.Text addScoreText_;

    [SerializeField]
    GameObject finishImage_;

    [SerializeField]
    bool bDebugGetAll_ = false;


    // 終了コールバック
    public System.Action FinishCallback { set { finishCallback_ = value; } }

    // セットアップ
    public void setup( string level ) {
        level_ = level;
        switch ( level_ ) {
            case "easy":
                groupIdx_ = 0;
                break;
            case "normal":
                groupIdx_ = 1;
                break;
            case "hard":
                groupIdx_ = 2;
                break;
            case "all":
                groupIdx_ = 99;
                break;
        }
    }

    // 合致したスピーカーをゲット
    public void getSpeakers( Player player, Speaker[] speakers ) {
        // スピーカーを取り除く
        string name = speakers[ 0 ].getSEName();
        if ( speakerSet_.ContainsKey( name ) == false )
            return;
        speakerSet_[ name ][ 0 ].removeAction( () => {
            speakerSet_[ name ][ 0 ].gameObject.SetActive( false );
        } );
        speakerSet_[ name ][ 1 ].removeAction( () => {
            speakerSet_[ name ][ 1 ].gameObject.SetActive( false );
        } );

        // 盤面のスピーカーがすべてなくなったらFinishへ
        remainSpeakerNum_--;
        if ( remainSpeakerNum_ == 0 ) {
            state_ = new Finish( this );
        }
    }

    // 相方スピーカーを取得
    public Speaker getPairSpeaker( Speaker firstSpeaker ) {
        var pair = speakerSet_[ firstSpeaker.getSEName() ];
        if ( pair[ 0 ] == firstSpeaker )
            return pair[ 1 ];
        return pair[ 0 ];
    }

    // スコアを更新
    public void updateScore( int baseScore, int comboCount, int totalScore, bool isIncrement ) {
        score_.setAim( totalScore );
        score_.Value = (val) => {
            scoreText_.text = string.Format( "{0}", val );
        };
        var color = addScoreText_.color;
        if ( baseScore != 0 ) {
            addScoreText_.text = string.Format( "{0} x {1}", baseScore, comboCount );
            GlobalState.time( 2.0f, (sec, t) => {
                color.a = 1.0f - Lerps.Float.easeIn01( t );
                addScoreText_.color = color;
                return true;
            } );
        }
    }

    private void Awake() {
        var color = addScoreText_.color;
        color.a = 0.0f;
        addScoreText_.color = color;
        finishImage_.gameObject.SetActive( false );
    }

    void Start () {
        // SEデータの一括読み込み
        var soundDataNum = Sound_data.getInstance().getRowNum();
        for ( int i = 0; i < soundDataNum; ++i ) {
            var param = Sound_data.getInstance().getParamFromIndex( i );
            if ( groupIdx_ == 99 || param.group_ == groupIdx_ ) {
                SoundAccessor.getInstance().loadSE( "Sounds/" + param.filename_, param.name_ );
            }  else {
                SoundAccessor.getInstance().removeSE( param.name_ );
            }
        }

        // プレイヤー設定
        player0_.setup( this );

        state_ = new Setup( this );
	}
	
	// Update is called once per frame
	void Update () {
        if ( state_ != null )
            state_ = state_.update();
        if ( bDebugGetAll_ == true ) {
            bDebugGetAll_ = false;
            var speakers = new List<Speaker>();
            foreach ( var obj in speakerSet_ ) {
                speakers.Add( obj.Value[ 0 ] );
                speakers.Add( obj.Value[ 1 ] );
            }
            for ( int i = 0; i < speakers.Count / 2; ++i ) {
                getSpeakers( null, new Speaker[] { speakers[ 2 * i ], speakers[ 2 * i + 1 ] } );
            }
        }
    }

    class Setup : State< GameManager > {
        public Setup( GameManager parent ) : base( parent ) {

        }
        protected override State innerInit() {
            // スピーカーをSEの数×2個散りばめる
            var soundDataNum = Sound_data.getInstance().getRowNum();
            var cd = new CardDistributer();
            var poses = cd.create( soundDataNum * ( parent_.groupIdx_ == 99 ? 4 : 2 ), 0.2f, 0.3f, 0.07f );
            ListUtil.shuffle( ref poses );

            int e = 0;
            for ( int i = 0; i < soundDataNum; ++i ) {
                var param = Sound_data.getInstance().getParamFromIndex( i );
                if ( parent_.groupIdx_ != 99 && param.group_ != parent_.groupIdx_ )
                    continue;
                Speaker[] speakers = new Speaker[ 2 ];
                for ( int j = 0; j < 2; ++j ) {
                    var speaker = Instantiate<Speaker>( parent_.speakerPrefab_ );
                    speaker.transform.parent = parent_.root_;
                    speaker.transform.localPosition = poses[ e ];
                    speaker.transform.localRotation = Quaternion.Euler( 0.0f, Random.Range( 0.0f, 360.0f ), 0.0f );
                    speaker.setSE( param.name_ );
                    speakers[ j ] = speaker;
                    e++;
                }
                parent_.speakerSet_[ param.name_ ] = speakers;
            }
            parent_.remainSpeakerNum_ = parent_.speakerSet_.Count;

            // フェードイン
            FaderManager.Fader.to( 0.0f, 1.0f );
            return this;
        }
    }

    class Finish : State< GameManager > {
        public Finish( GameManager parent ) : base( parent ) {

        }
        protected override State innerInit() {
            GlobalState.wait( 1.0f, () => {
                parent_.finishImage_.gameObject.SetActive( true );
                GlobalState.wait( 4.0f, () => {
                    setNextState( new FadeOut( parent_ ) );
                    return false;
                } );
                return false;
            } );
            return this;
        }
    }

    class FadeOut : State< GameManager > {
        public FadeOut(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            FaderManager.Fader.to( 1.0f, 3.0f, () => {
                parent_.finishCallback_();
            } );
            return null;
        }
    }

    State state_;
    string level_;
    Dictionary<string, Speaker[]> speakerSet_ = new Dictionary<string, Speaker[]>();
    int groupIdx_ = 0;
    MoveValueLong score_ = new MoveValueLong( 0, 1.3f );
    int remainSpeakerNum_ = 0;
    System.Action finishCallback_;
}
