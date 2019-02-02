using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// トラップ
//
//  爆弾箱及びギミックボックスの蓋にアタッチできるギミック
//  Entity群には含まれない

public class Trap : MonoBehaviour {

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

    Answer answer_;
}
