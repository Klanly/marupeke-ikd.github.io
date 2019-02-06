using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBLamp : MonoBehaviour {

    [SerializeField]
    MeshRenderer blueLamp_;

    [SerializeField]
    MeshRenderer redLamp_;

    [SerializeField]
    BombTimer timer_;

    [SerializeField]
    Material offMat_;

    // 赤青のタイミングを設定
    public void setup( int randomNumber )
    {
        // 10秒のうち乱数が、
        //  0,1,2 : どちらもオフ
        //  3 : 青をオン
        //  4 : 赤をオン
        var r = new System.Random( randomNumber );
        while( true ) {
            int redCount = 0;
            int blueCount = 0;
            for ( int i = 0; i < 10; ++i ) {
                blueTiming_[ i ] = 0;
                redTiming_[ i ] = 0;
                int color = r.Next() % 5;
                if ( color == 3 ) {
                    blueTiming_[ i ] = 1;
                    blueCount++;
                } else if ( color == 4 ) {
                    redTiming_[ i ] = 1;
                    redCount++;
                }
            }
            if ( blueCount > 0 || redCount > 0 )
                break;
        }
    }

    // 赤青のタイミングを取得
    public int getCurTiming( BombBoxModel.CutLine color )
    {
        int sec = ( int )timer_.getSec();
        int e = sec % 10;
        if ( color == BombBoxModel.CutLine.Red )
            return redTiming_[ e ];
        else if ( color == BombBoxModel.CutLine.Blue )
            return blueTiming_[ e ];
        return 0;
    }

    // Use this for initialization
    void Start () {
        blueMat_ = blueLamp_.material;
        redMat_ = redLamp_.material;
	}
	
	// Update is called once per frame
	void Update () {
        int sec = ( int )timer_.getSec();
        int e = sec % 10;
        if ( blueTiming_[ e ] == 1 ) {
            blueLamp_.material = blueMat_;
        } else {
            blueLamp_.material = offMat_;
        }
        if ( redTiming_[ e ] == 1 ) {
            redLamp_.material = redMat_;
        } else {
            redLamp_.material = offMat_;
        }
	}

    int[] blueTiming_ = new int[ 10 ];
    int[] redTiming_ = new int[ 10 ];
    Material blueMat_;
    Material redMat_;
}
