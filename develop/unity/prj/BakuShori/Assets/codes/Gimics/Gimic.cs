using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ギミック
//
//  ギミックボックスに格納される。
//  子エンティティは持てない

public class Gimic : Entity {

    // パラメータを設定
    virtual public void setParam( int randomNumber, GimicSpec gimicSpec )
    {

    }

    // Entityを登録
    override public bool setEntity(int index, Entity entity)
    {
        //  子エンティティは持てない
        return false;
    }

    // 答えを取得
    public Answer getAnswer()
    {
        return answer_;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected Answer answer_ = null;
}
