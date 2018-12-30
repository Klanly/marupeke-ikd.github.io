using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGamePracticeMode : CubeGameManager {

    [SerializeField]
    string practiceDataName_;

    // 練習データを読み込み
    public bool loadPracticeData( string dataName )
    {
        practiceData_ = new CubePracticeData();
        if ( practiceData_.load( dataName ) == true )
            practiceDataName_ = dataName;

        return true;
    }

    // 初期化
    override protected void initialize()
    {
        if ( bInitialized_ == true )
            return;

        if ( practiceData_ == null ) {
            if ( practiceDataName_ != "" ) {
                if ( loadPracticeData( practiceDataName_ ) == false )
                    return;
            } else
                return;
        }

        N = practiceData_.getN();
        base.initialize();

        var cube = getCube();
        practiceData_.setPiecesOnCube( cube );

        bInitialized_ = true;
    }

    // Use this for initialization
    void Start () {
        initialize();
	}
	
	// Update is called once per frame
	void Update () {
        innerUpdate();	
	}

    bool bInitialized_ = false;
    CubePracticeData practiceData_;
}
