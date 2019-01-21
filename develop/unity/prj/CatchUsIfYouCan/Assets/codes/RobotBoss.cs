using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : Robot {

    [SerializeField]
    float relativeSpeedToHuman_ = 0.92f;


	// Use this for initialization
	void Start () {
        normalSpeed_ = getSpeed();
        initialize();		
	}
	
	// Update is called once per frame
	void Update () {
        innerUpdate();
	}

    protected override void innerUpdate()
    {
        // 人の速度との相対速度をキープ
        speed_ = human_.getSpeed() * relativeSpeedToHuman_;

        // 人の速度が自分よりも遅い場合は自分のノーマル速度をキープ
        if ( speed_ < normalSpeed_ )
            speed_ = normalSpeed_;
        escapeSpeed_ = speed_;

        base.innerUpdate();
    }

    float normalSpeed_ = 0.0f;
}
