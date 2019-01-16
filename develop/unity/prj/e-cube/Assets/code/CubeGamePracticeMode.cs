using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 練習モード管理人

public class CubeGamePracticeMode : CubeGameManager {

    [SerializeField]
    string practiceDataName_;

    [SerializeField]
    UnityEngine.UI.Text solveText_;

    [SerializeField]
    UnityEngine.UI.Image solveTextBG_;

    [SerializeField]
    GameObject retryButtonSet_;

    [SerializeField]
    UnityEngine.UI.Button retryButton_;

    [SerializeField]
    UnityEngine.UI.Button finishButton_;

    [SerializeField]
    CubeMissMark missMark_;


    // 練習データを読み込み
    public void loadPracticeData( string dataName, System.Action<bool> callback )
    {
        practiceData_ = new CubePracticeData();
        practiceData_.load( dataName, ( _res ) => {
            if ( _res == true )
                practiceDataName_ = dataName;
            callback( _res );
        } );
    }

    // 初期化
    override protected void initialize()
    {
        if ( bInitialized_ == true )
            return;

        bInitialized_ = true;

        if ( practiceData_ == null ) {
            if ( practiceDataName_ != "" ) {
                loadPracticeData( practiceDataName_, ( _res ) => {
                    if ( _res == false ) {
                        bError_ = true;
                        return;
                    }
                    N = practiceData_.getN();
                    base.initialize();

                    var cube = getCube();
                    practiceData_.setPiecesOnCube( cube );

                    // 回転テキスト
                    rotateText_.setup( practiceData_, solveText_, solveTextBG_ );

                    solve_ = practiceData_.getRotateList();

                    bReady_ = true;
                } );
            } else
                return;
        }

        // 状態初期化
        //  練習データを読み終えたら開始
        state_ = new WaitDataLoading( this );

        // ミスマークを消す
        missMark_.hide();
    }

    // コントローラ設定
    override protected void initializeControllers(CubeControllerManager controllerManager)
    {
        mouseCont_ = new CubeMouseController( getCube(), getCubeCamera() );
        mouseCont_.setActive( false );
        mouseCont_.setActiveRotationOp( false );    // 回転操作は解答時にONに
        controllerManager.joinController( mouseCont_ );
        controllerManager.setActive( true );
        controllerManager.setAutoNonActiveWhenComplete( false );
    }

    // 再練習
    void retry()
    {
        state_ = new Practice( this );
    }

    // 練習終了
    void finishPractice()
    {

    }

    private void Awake()
    {
        retryButtonSet_.SetActive( false );
    }
    // Use this for initialization
    void Start () {
        initialize();
	}
	
	// Update is called once per frame
	void Update () {
        ResourceLoader.getInstance().update();
        if ( bError_ == false && bReady_ == true )
            innerUpdate();	
	}

    override protected void innerUpdate()
    {
        base.innerUpdate();

        if ( state_ != null )
            state_ = state_.update();
    }


    class InnerState : State
    {
        public InnerState(CubeGamePracticeMode parent )
        {
            parent_ = parent;
        }
        protected CubeGamePracticeMode parent_;
    }

    // 練習データ読み込み待ち
    class WaitDataLoading : InnerState
    {
        public WaitDataLoading( CubeGamePracticeMode parent ) : base( parent )
        {

        }
        // 内部状態
        override protected State innerUpdate()
        {
            if ( parent_.bReady_ == true && parent_.bError_ == false )
                return new IntroShowSolve( parent_ );
            return this;
        }
    }

    // イントロ（問題→解答表示）
    class IntroShowSolve : InnerState
    {
        public IntroShowSolve(CubeGamePracticeMode parent) : base( parent )
        {

        }
        // 内部初期化
        override protected void innerInit()
        {
            // 問題表示（ユーザクリック待機）
            //  →解答までオート進行
            //  →解答表示（ユーザクリック待機）
            //  →問題まで戻す

            parent_.mouseCont_.setActive( true );
            CubeAutoOperator autoOp = null;
            GlobalState.start( () => {
                // 問題表示（ユーザクリック待機）
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    return false;
                }
                return true;
            } ).next( () => { autoOp = new CubeAutoOperator( parent_.getCube(), parent_.practiceData_ ); }, () => {
                // 解答までオート進行
                if ( autoOp.update() == false )
                    return false;
                return true;
            } ).next( () => {
                // 解答表示（ユーザクリック待機）
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    return false;
                }
                return true;
            } ).next( () => { autoOp = new CubeAutoOperator( parent_.getCube(), parent_.practiceData_, true ); }, () => {
                // 問題まで戻す
                if ( autoOp.update() == false ) {
                    // 解答開始につき回転操作をON
                    parent_.mouseCont_.setActiveRotationOp( true );
                    return false;
                }
                return true;
            } ).finish( () => {
                bFinish_ = true;
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true )
                return new Practice( parent_ );
            return this;
        }

        bool bFinish_ = false;
    }

    // 練習中
    class Practice : InnerState
    {
        public Practice(CubeGamePracticeMode parent) : base( parent )
        {

        }
        // 内部初期化
        override protected void innerInit()
        {
            // 練習パターンを改めて設定
            parent_.practiceData_.setPiecesOnCube( parent_.getCube() );

            // 解答開始につき回転操作をON
            parent_.mouseCont_.setActiveRotationOp( true );

            // ミスマーク消す
            parent_.missMark_.hide();

            parent_.rotateText_.setIdx( idx_ );
            parent_.mouseCont_.setRotateCallback( (axis, rotType, colIndices) => {
                if ( checkRotate( axis, rotType, colIndices ) == true ) {
                    // 正しい手順
                    idx_++;
                    if ( parent_.rotateText_.setIdx( idx_ ) == false ) {
                        bFinish_ = true;
                    }
                } else {
                    // 間違った手順
                    //  キューブを元に戻す
                    float rotDegPerFrame = parent_.getCube().RotDegPerFrame;
                    parent_.getCube().RotDegPerFrame = 360.0f;
                    parent_.getCube().onRotation( axis, colIndices, CubeRotateUtil.Util.getInvRotType( rotType ), () => {
                        parent_.getCube().RotDegPerFrame = rotDegPerFrame;
                    } );

                    // ミスマーク表記
                    parent_.missMark_.show();

                    // 終了へ
                    bFinish_ = true;
                }
            } );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true )
                return new PracticeFinish( parent_ );
            return this;
        }

        // 正しい回転？
        bool checkRotate( AxisType axis, CubeRotationType rotType, int[] colIndices )
        {
            return parent_.solve_[ idx_ ].isCorrect( parent_.getCube().getN(), axis, rotType, colIndices );
        }
        bool bFinish_ = false;
        int idx_ = 0;
    }

    // 練習終了
    class PracticeFinish : InnerState
    {
        public PracticeFinish(CubeGamePracticeMode parent) : base( parent )
        {

        }
        // 内部初期化
        override protected void innerInit()
        {
            parent_.mouseCont_.setActiveRotationOp( false );    // 回転終了
            parent_.retryButtonSet_.SetActive( true );          // リトライボタン表示

            parent_.retryButton_.onClick.AddListener( () => {
                // リトライ
                bFinish_ = true;
                parent_.retryButton_.onClick.RemoveAllListeners();
                parent_.retryButtonSet_.SetActive( false );
                parent_.retry();
            } );
            parent_.finishButton_.onClick.AddListener( () => {
                // 終了
                bFinish_ = true;
                parent_.retryButton_.onClick.RemoveAllListeners();
                parent_.retryButtonSet_.SetActive( false );
                parent_.finishPractice();
            });
        }
        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                parent_.retryButtonSet_.SetActive( false );
                return null;
            }
            return this;
        }
        bool bFinish_ = false;
    }

    bool bInitialized_ = false;
    bool bReady_ = false;
    bool bError_ = false;
    CubePracticeData practiceData_;
    List<CubePracticeData.RotateUnit> solve_;
    State state_;
    CubeMouseController mouseCont_;
    CubeRotateTexts rotateText_ = new CubeRotateTexts();
}
