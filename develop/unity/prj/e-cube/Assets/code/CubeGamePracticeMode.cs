using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 練習モード管理人

public class CubeGamePracticeMode : CubeGameManager {

    [SerializeField]
    string practiceDataName_;

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

                    bReady_ = true;
                } );
            } else
                return;
        }
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

    bool bInitialized_ = false;
    bool bReady_ = false;
    bool bError_ = false;
    CubePracticeData practiceData_;
}
