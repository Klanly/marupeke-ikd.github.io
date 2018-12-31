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
    UnityEngine.UI.Button[] selectColorButtons_;

    [SerializeField]
    UnityEngine.UI.InputField fileName_;

    [SerializeField]
    string directoryPath_;

    // 練習データをセーブ
    public void savePracticeData( string filePath )
    {
        var solve = new List<CubePracticeData.RotateUnit>();
        solve.Add( new CubePracticeData.RotateUnit(FaceType.FaceType_Left, CubeRotationType.CRT_Minus_90 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Left, CubeRotationType.CRT_Minus_180 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Left, CubeRotationType.CRT_Minus_270 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Left, CubeRotationType.CRT_Plus_90 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Left, CubeRotationType.CRT_Plus_180 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Left, CubeRotationType.CRT_Plus_270 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Right, CubeRotationType.CRT_Minus_90 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Right, CubeRotationType.CRT_Minus_180 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Right, CubeRotationType.CRT_Minus_270 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Right, CubeRotationType.CRT_Plus_90 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Right, CubeRotationType.CRT_Plus_180 ) );
        solve.Add( new CubePracticeData.RotateUnit( FaceType.FaceType_Right, CubeRotationType.CRT_Plus_270 ) );
        string data = CubePracticeData.createDataStrFromCube( getCube(), solve );
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
        selectColorButtons_[ 0 ].onClick.AddListener( onColorLeft );
        selectColorButtons_[ 1 ].onClick.AddListener( onColorRight );
        selectColorButtons_[ 2 ].onClick.AddListener( onColorDown );
        selectColorButtons_[ 3 ].onClick.AddListener( onColorUp );
        selectColorButtons_[ 4 ].onClick.AddListener( onColorFront );
        selectColorButtons_[ 5 ].onClick.AddListener( onColorBack );
        selectColorButtons_[ 6 ].onClick.AddListener( onColorNone );
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
                cube.setPieceFace( face, idx, curSelectFace_ );
            }
        }
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

    override protected void initializeControllers(CubeControllerManager controllerManager)
    {
        editCont_ = new CubeEditController( getCube(), getCubeCamera() );
        controllerManager.joinController( editCont_ );
        controllerManager.setActive( true );
        controllerManager.setAutoNonActiveWhenComplete( false );
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
    }

    bool bInitialized_ = false;
    CubeEditController editCont_;
    FaceType curSelectFace_ = FaceType.FaceType_Down;
}
