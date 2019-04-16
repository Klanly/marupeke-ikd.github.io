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
    }

    void Start () {
        // SEデータの一括読み込み
        var soundDataNum = Sound_data.getInstance().getRowNum();
        for ( int i = 0; i < soundDataNum; ++i ) {
            var param = Sound_data.getInstance().getParamFromIndex( i );
            if ( param.group_ == groupIdx_ ) {
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
	}

    class Setup : State< GameManager > {
        public Setup( GameManager parent ) : base( parent ) {

        }
        protected override State innerInit() {
            // スピーカーをSEの数×2個散りばめる
            var soundDataNum = Sound_data.getInstance().getRowNum();
            var cd = new CardDistributer();
            var poses = cd.create( soundDataNum * 2, 0.17f, 0.22f, 0.05f );

            int e = 0;
            for ( int i = 0; i < soundDataNum; ++i ) {
                var param = Sound_data.getInstance().getParamFromIndex( i );
                if ( param.group_ != parent_.groupIdx_ )
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
            return this;
        }
    }

    State state_;
    Dictionary<string, Speaker[]> speakerSet_ = new Dictionary<string, Speaker[]>();
    int groupIdx_ = 0;
    MoveValueLong score_ = new MoveValueLong( 0, 1.3f );
}
