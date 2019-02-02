using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミックネジトラップ
//
//  ギミック：
//   右回し及び左回し
//   回転数(1～3)

public class ScrewTrap : Trap {

    [SerializeField]
    Rotate rotate_ = Rotate.Left;

    [SerializeField]
    int rotNum_ = 1;

    [SerializeField]
    ScrewTrapAnswer answerPrefab_;

    public enum Rotate : int
    {
        Left = 0,
        Right = 1
    }
    // セットアップ指示
    public override void setup( int randomNumber )
    {
        rotate_ = (Rotate)( randomNumber % 2 );
        rotNum_ = ( randomNumber % 3 ) + 1;
        createAnswer( randomNumber );
    }

    // 答え作成
    void createAnswer( int randomNumber )
    {
        var obj =Instantiate<ScrewTrapAnswer>( answerPrefab_ );
        obj.setAnswer( randomNumber, rotate_, rotNum_ );
        answer_ = obj;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
