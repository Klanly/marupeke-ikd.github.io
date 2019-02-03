using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ネジギミックアンサー
public class ScrewTrapAnswer : Answer {

    private void Awake()
    {
        ObjectType = EObjectType.ScrewAnswer;    
    }

    // 答え設定
    public void setAnswer( int randomNumber, ScrewTrap.Rotate rotate, int rotNum )
    {
        rotate_ = rotate;
        rotNum_ = rotNum;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    ScrewTrap.Rotate rotate_ = ScrewTrap.Rotate.Left;
    int rotNum_ = 1;
}
