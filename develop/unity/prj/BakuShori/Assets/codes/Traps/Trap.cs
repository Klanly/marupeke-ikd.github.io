using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// トラップ
//
//  爆弾箱及びギミックボックスの蓋にアタッチできるギミック
//  Entity群には含まれない

public class Trap : MonoBehaviour {

    // セットアップ指示
    public virtual void setup( int randomNumber, bool forGimicBox )
    {

    }

    // 答えを取得
    public Answer getAnswer()
    {
        if ( answer_ == null )
            Debug.LogWarning( "Trap: warning: no answer." );

        return answer_;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected Answer answer_;
}
