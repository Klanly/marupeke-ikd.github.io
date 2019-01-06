using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CubePracticeEditManager : CubeGameManager {

    [SerializeField]
    string practiceDataName_;

    [SerializeField]
    UnityEngine.UI.Button saveButton_;

    [SerializeField]
    UnityEngine.UI.Button fillButton_;

    [SerializeField]
    UnityEngine.UI.Button solveButton_;

    [SerializeField]
    UnityEngine.UI.Button undoButton_;

    [SerializeField]
    UnityEngine.UI.Button testButton_;

    [SerializeField]
    UnityEngine.UI.Button[] selectColorButtons_;

    [SerializeField]
    UnityEngine.UI.InputField fileName_;

    [SerializeField]
    UnityEngine.UI.InputField solveCode_;

    [SerializeField]
    string directoryPath_;

    // 練習データをセーブ
    public void savePracticeData( string filePath )
    {
        string data = CubePracticeData.createDataStrFromCube( getCube(), solve_ );
        if ( data != "" ) {
            File.WriteAllText( filePath, data );
        }
    }

    // 初期化
    override protected void initialize()
    {
        if ( bInitialized_ == true )
            return;

        bInitialized_ = true;
        base.initialize();

        // ボタン設定
        saveButton_.onClick.AddListener( onSave );
        fillButton_.onClick.AddListener( onFill );
        solveButton_.onClick.AddListener( onSolve );
        undoButton_.onClick.AddListener( onUndo );
        testButton_.onClick.AddListener( onTest );
        selectColorButtons_[ 0 ].onClick.AddListener( onColorLeft );
        selectColorButtons_[ 1 ].onClick.AddListener( onColorRight );
        selectColorButtons_[ 2 ].onClick.AddListener( onColorDown );
        selectColorButtons_[ 3 ].onClick.AddListener( onColorUp );
        selectColorButtons_[ 4 ].onClick.AddListener( onColorFront );
        selectColorButtons_[ 5 ].onClick.AddListener( onColorBack );
        selectColorButtons_[ 6 ].onClick.AddListener( onColorNone );

        testButtonText_ = testButton_.GetComponentInChildren<UnityEngine.UI.Text>();
    }

    override protected void initializeControllers(CubeControllerManager controllerManager)
    {
        editCont_ = new CubeEditController( getCube(), getCubeCamera() );
        solveCont_ = new CubeMouseController( getCube(), getCubeCamera() );
        solveCont_.setActive( false );
        controllerManager.joinController( editCont_ );
        controllerManager.joinController( solveCont_ );
        controllerManager.setActive( true );
        controllerManager.setAutoNonActiveWhenComplete( false );
    }

    // セーブ処理
    void onSave()
    {
        if ( fileName_.text == "" )
            return;

        // 同名のファイルが既にある場合は許可しない
        string filePath = createFilePath( fileName_.text );
        if ( checkFileName( filePath ) == false ) {
            return;
        }

        savePracticeData( filePath );
    }

    // セーブ用のファイルパスを作成
    string createFilePath( string fileName )
    {
        if ( directoryPath_ == "" )
            return fileName + ".json.bytes";
        return directoryPath_ + "/" + fileName + ".json.bytes";
    }

    // ファイルをチェック
    bool checkFileName( string filePath )
    {
        return !File.Exists( filePath );
    }

    // 選択色で全面塗りつぶし処理
    void onFill()
    {
        var cube = getCube();
        var faceTypes = new FaceType[ 6 ] {
            FaceType.FaceType_Left,
            FaceType.FaceType_Right,
            FaceType.FaceType_Down,
            FaceType.FaceType_Up,
            FaceType.FaceType_Front,
            FaceType.FaceType_Back,
        };
        int faceNum = cube.getN() * cube.getN();
        foreach ( var face in faceTypes ) {
            for ( int idx = 0; idx < faceNum; ++idx ) {
                cube.setFaceColor( face, idx, curSelectFace_ );
            }
        }
    }

    // 解答作成
    void onSolve()
    {
        if ( bSolveMode_ == false ) {
            bSolveMode_ = true;
            // 回転コントローラをONに、選択コントローラをOFFにする
            solveCont_.setActive( true );
            editCont_.setActive( false );
            var text = solveButton_.GetComponentInChildren< UnityEngine.UI.Text >();
            text.text = "Edit";
            solveCode_.text = "";
            solve_.Clear();

            solveCont_.setRotateCallback( ( axis, rotType, colIndices ) => {
                int n = getCube().getN();
                var rotateUnit = CubePracticeData.convAxisRotateToRotUnit( n, axis, rotType, colIndices );
                solve_.Add( rotateUnit );
                solveCode_.text = createSolveText();
            } );

            // Cubeバックアップ
            cubeDataBackUp_ = getCube().dataBackUp();

        } else {
            bSolveMode_ = false;
            // 回転コントローラをOFFに、選択コントローラをONにする
            solveCont_.setActive( false );
            editCont_.setActive( true );
            var text = solveButton_.GetComponentInChildren< UnityEngine.UI.Text >();
            text.text = "Solve";

            // バックアップで復元
            getCube().setFromBackUp( cubeDataBackUp_ );
        }
    }

    // Solve時Undoボタン
    void onUndo()
    {
        if ( bSolveMode_ == false )
            return;
        if ( solve_.Count == 0 )
            return;
        var undoUnit = solve_[ solve_.Count - 1 ];
        solve_.RemoveAt( solve_.Count - 1 );

        bool bFoward = true;
        AxisType axis = AxisType.AxisType_X;
        switch ( undoUnit.face_ ) {
            case FaceType.FaceType_Left:
                axis = AxisType.AxisType_X;
                break;
            case FaceType.FaceType_Right:
                axis = AxisType.AxisType_X;
                bFoward = false;
                break;
            case FaceType.FaceType_Down:
                axis = AxisType.AxisType_Y;
                break;
            case FaceType.FaceType_Up:
                axis = AxisType.AxisType_Y;
                bFoward = false;
                break;
            case FaceType.FaceType_Front:
                axis = AxisType.AxisType_Z;
                break;
            case FaceType.FaceType_Back:
                axis = AxisType.AxisType_Z;
                bFoward = false;
                break;
        }
        getCube().onRotation(
            axis,
            bFoward ? undoUnit.colIndices_.ToArray() : undoUnit.getInvColIndices( getCube().getN() ),
            CubeRotateUtil.Util.getInvRotType( undoUnit.getRotType() )
        );
        solveCode_.text = createSolveText();
    }

    // 動作テスト
    void onTest()
    {
        var cube = getCube();
        float defRotSpeed = cube.RotDegPerFrame;
        cube.RotDegPerFrame = 4.0f;
        testButton_.enabled = false;
        testButtonText_.text = "Testing";
        var backUpData = getCube().dataBackUp();
        task_ = new SolveTest( this, solve_, () => {
            GlobalState.start( () => {
                // どこかクリックしたらテスト終わり
                if ( Input.GetMouseButtonDown( 0 ) == true ) {
                    cube.setFromBackUp( backUpData );
                    cube.RotDegPerFrame = defRotSpeed;
                    testButton_.enabled = true;
                    testButtonText_.text = "Test";
                    return false;
                }
                return true;
            } );
        });
    }

    // LeftColor選択
    void onColorLeft()
    {
        curSelectFace_ = FaceType.FaceType_Left;
        editCont_.setPasteColor( curSelectFace_ );
    }
    // RightColor選択
    void onColorRight()
    {
        curSelectFace_ = FaceType.FaceType_Right;
        editCont_.setPasteColor( curSelectFace_ );
    }
    // DownColor選択
    void onColorDown()
    {
        curSelectFace_ = FaceType.FaceType_Down;
        editCont_.setPasteColor( curSelectFace_ );
    }
    // UpColor選択
    void onColorUp()
    {
        curSelectFace_ = FaceType.FaceType_Up;
        editCont_.setPasteColor( curSelectFace_ );
    }
    // FrontColor選択
    void onColorFront()
    {
        curSelectFace_ = FaceType.FaceType_Front;
        editCont_.setPasteColor( curSelectFace_ );
    }
    // BackColor選択
    void onColorBack()
    {
        curSelectFace_ = FaceType.FaceType_Back;
        editCont_.setPasteColor( curSelectFace_ );
    }
    // NoneColor選択
    void onColorNone()
    {
        curSelectFace_ = FaceType.FaceType_None;
        editCont_.setPasteColor( curSelectFace_ );
    }

    // Solveテキスト作成
    string createSolveText()
    {
        string text = "";
        for ( int i = 0; i < solve_.Count; ++i ) {
            text += solve_[ i ].getRotateCode();
            if ( i + 1 != solve_.Count )
                text += ", ";
        }
        return text;
    }

    // Use this for initialization
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        innerUpdate();

        if ( task_ != null ) {
            task_ = task_.update();
        }
    }


    // 解答テスト
    //  指定の手順で自動的に回す
    class SolveTest : State
    {
        public SolveTest( CubePracticeEditManager parent, List<CubePracticeData.RotateUnit> solve, System.Action testFinishCallback )
        {
            parent_ = parent;
            solve_ = solve;
            testFinishCallback_ = testFinishCallback;
        }

        // 内部初期化
        override protected void innerInit()
        {
            if ( solve_.Count == 0 ) {
                bFinish_ = true;
                return;
            }

            var axis = solve_[ 0 ].getRotateAxis();
            bool bInv = ( solve_[ 0 ].face_ == FaceType.FaceType_Right || solve_[ 0 ].face_ == FaceType.FaceType_Up || solve_[ 0 ].face_ == FaceType.FaceType_Back );
            parent_.getCube().onRotation(
                axis,
                bInv ? solve_[ 0 ].getInvColIndices( parent_.getCube().getN() ) : solve_[ 0 ].colIndices_.ToArray(),
                solve_[ 0 ].getRotType(),
                rotateFinishCallback
            );
        }

        // 内部状態
        override protected State innerUpdate()
        {
            if ( bFinish_ == true ) {
                if ( testFinishCallback_ != null )
                    testFinishCallback_();
                return null;
            }
            return this;
        }

        // 次の回転を設定
        void rotateFinishCallback()
        {
            i++;
            if ( i >= solve_.Count ) {
                bFinish_ = true;
                return;
            }
            // ちょっとだけ間を入れて次の回転へ
            GlobalState.wait( 0.5f, () => {
                var axis = solve_[ i ].getRotateAxis();
                bool bInv = ( solve_[ i ].face_ == FaceType.FaceType_Right || solve_[ i ].face_ == FaceType.FaceType_Up || solve_[ i ].face_ == FaceType.FaceType_Back );
                parent_.getCube().onRotation(
                    axis,
                    bInv ? solve_[ i ].getInvColIndices( parent_.getCube().getN() ) : solve_[ i ].colIndices_.ToArray(),
                    solve_[ i ].getRotType(),
                    rotateFinishCallback
                );
                return false;
            });
        }

        CubePracticeEditManager parent_;
        List<CubePracticeData.RotateUnit> solve_;
        int i = 0;
        bool bFinish_ = false;
        System.Action testFinishCallback_;
    }

    bool bInitialized_ = false;
    CubeEditController editCont_;
    CubeMouseController solveCont_;
    FaceType curSelectFace_ = FaceType.FaceType_Down;
    bool bSolveMode_ = false;
    List<CubePracticeData.RotateUnit> solve_ = new List<CubePracticeData.RotateUnit>();
    CubeData cubeDataBackUp_;    // Cubeバックアップ
    State task_ = null;
    UnityEngine.UI.Text testButtonText_;
}
