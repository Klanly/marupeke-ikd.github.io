using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {
    [SerializeField]
    Robot robot_;

    [SerializeField]
    RobotBoss boss_;

    public Robot createRobot()
    {
        return Instantiate<Robot>( robot_ );
    }

    public RobotBoss createBoss()
    {
        return Instantiate<RobotBoss>( boss_ );
    }
}
