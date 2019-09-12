using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    PaiGenerator paiGenerator_;
    public PaiGenerator PaiGenerator { get { return paiGenerator_; } }

    [SerializeField]
    Field field_;
    public Field Field { get { return field_; } }

    [SerializeField]
    TehaiSetManager tehaiSetManager_;

    [SerializeField]
    NextPaiManager nextPaiManager_;
    public NextPaiManager NextPaiManager { get { return nextPaiManager_; } }

    [SerializeField]
    YakuViewer yakuViewer_;

    [SerializeField]
    TextMesh scoreText_;

    [SerializeField]
    Controller controller_;

    [SerializeField]
    TextMesh messageText_;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_;

    public static GameManager getInstance() {
        return manager_g;
    }

    // スコアを追加
    public void addScore(int score, int rensa) {
        score_ += score;
        scoreText_.text = string.Format( "{0}", score_ );
        Debug.Log( "rensa: " + rensa + ", score: " + score_ );
    }

    // 面子追加
    public void addMentsu(List<MenzenSet> menzens) {
        foreach ( var m in menzens ) {
            // 対子？
            if ( m.PaiGroup.getType() == Mahjang.PaiGroup.Type.Toitsu ) {
                if ( toitsu_ == null ) {
                    toitsu_ = m;
                    curTehaiSet_.addToitsu( m );
                    checkYaku();
                }
                continue;
            }
            // 面子に空きが無くて対子のみ必要で、手が刻子の場合は対子として採用し直す
            if ( menzens_.Count == 4 && toitsu_ == null && m.PaiGroup.isSamePai() == true ) {
                toitsu_ = m;
                curTehaiSet_.addToitsu( m );
                checkYaku();
                continue;
            }
            // 面子に空きがあれば挿入
            if ( menzens_.Count < 4 ) {
                menzens_.Add( m );
                curTehaiSet_.addMentsu( m );
                checkYaku();
                continue;
            }
            // 入れる要素が無いのでメンゼン手をストックしておく
            //  TODO: 後で使おう
            stockMenzens_.Add( m );
        }
    }

    // 役をチェック
    void checkYaku() {
        if ( toitsu_ == null || menzens_.Count < 4 ) {
            return; // まだ揃っていない
        }
        // 役判定
        var ankous = new List<Mahjang.Pai>();
        var minkous = new List<Mahjang.PaiGroup>();
        // 対子
        ankous.Add( toitsu_.PaiGroup.getPais()[ 0 ] );
        ankous.Add( toitsu_.PaiGroup.getPais()[ 1 ] );
        foreach ( var m in menzens_ ) {
            if ( m.PaiGroup.getType() == Mahjang.PaiGroup.Type.Kantsu ) {
                minkous.Add( m.PaiGroup );
            } else {
                foreach ( var p in m.PaiGroup.getPais() ) {
                    ankous.Add( p );
                }
            }
        }
        var pais = new Mahjang.Pais();
        if ( pais.setPais( ankous, minkous ) == true ) {
            // 牌は整っている
            var paisetList = pais.getPaiSetList();
            var baState = new Mahjang.MahjangScoreCalculator.BaState();
            int highScore = 0;
            int highScoreIdx = -1;
            int highHan = 0;
            List<Mahjang.MahjangScoreCalculator.YakuData> yakuDataList = null;
            Mahjang.MahjangScoreCalculator.YakuData highScoreYaku = null;
            foreach ( var ps in paisetList ) {
                var calc = new Mahjang.MahjangScoreCalculator();
                yakuDataList = calc.analyze( ps.ankouGroup_, ps.minkoGroup_, baState );
                if ( yakuDataList.Count > 0 ) {
                    for ( int i = 0; i < yakuDataList.Count; ++i ) {
                        bool isYakuman = false;
                        int han = 0;
                        int score = calc.calcScore( yakuDataList[ i ], out han, out isYakuman );
                        if ( score > highScore ) {
                            highScoreIdx = i;
                            highScore = score;
                            highScoreYaku = yakuDataList[ i ];
                            highHan = han;
                        }
                    }
                }
            }
            if ( highScoreIdx >= 0 ) {
                // 最高点数役出来た！
                string yakuStrs = "";
                foreach ( var yaku in yakuDataList[ highScoreIdx ].yakuList_ ) {
                    yakuStrs += yaku.yaku_.ToString() + ", ";
                }
                Debug.Log( yakuStrs );
                Debug.Log( "Han: " + highHan + ", Score: " + highScore );
            }

            // 門前手揃ったので役判定＋表現を別タスクへ
            TehaiSet tehaiSet = curTehaiSet_;
            yakuViewer_.start( tehaiSet, highScoreIdx >= 0 ? yakuDataList[ highScoreIdx ] : null, highScore, highHan, () => {
                Destroy( tehaiSet.gameObject );
            } );

            addScore( highScore, 1 );
            curTehaiSet_ = tehaiSetManager_.createNewTehaiSet();
            menzens_.Clear();
            toitsu_ = null;
        }
    }

    private void Awake() {
        manager_g = this;
        curTehaiSet_ = tehaiSetManager_.createNewTehaiSet();
        scoreText_.text = "0";
    }

    private void OnDestroy() {
        manager_g = null;
    }

    // Start is called before the first frame update
    void Start() {
        state_ = new Intro( this );
    }

    // Update is called once per frame
    void Update() {
        if ( state_ != null )
            state_ = state_.update();
    }

    static GameManager manager_g = null;

    int score_ = 0;
    List<MenzenSet> menzens_ = new List<MenzenSet>();
    List<MenzenSet> stockMenzens_ = new List<MenzenSet>();
    MenzenSet toitsu_ = null;
    TehaiSet curTehaiSet_;
    State state_;

    class Intro : State<GameManager> {
        public Intro(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.messageText_.text = "Press [Z] to start";
            return null;
        }
        protected override State innerUpdate() {
            // [Z]ボタンを押したらスタート
            if ( Input.GetKeyDown( KeyCode.Z ) == true ) {
                parent_.messageText_.text = "";
                return new GameStart( parent_ );
            }
            return this;
        }
    }

    class GameStart : State<GameManager> {
        public GameStart(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            parent_.controller_.toStart();
            parent_.field_.FinishCallback = () => {
                setNextState( new GameOver( parent_ ) );
            };
            return this;
        }
    }

    class GameOver : State<GameManager> {
        public GameOver(GameManager parent) : base( parent ) { }
        protected override State innerInit() {
            // GameOver表記
            parent_.messageText_.text = "Game Over";
            GlobalState.wait( 4.0f, () => {
                parent_.finishCallback_();
                return false;
            } );
            return null;
        }
    }
}
