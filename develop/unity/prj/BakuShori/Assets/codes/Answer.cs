using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アンサー
//
//  ギミックボックス、ギミック、ギミックネジなどの答えとなる
public class Answer : Entity {

    // 子Entityリストのサイズを設定
    override public bool setChildrenListSize(int size)
    {
        // 1個以外は設定不可
        if ( size != 1 )
            return false;
        return base.setChildrenListSize( size );
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
