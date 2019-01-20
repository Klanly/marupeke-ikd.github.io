using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {
    [SerializeField]
    Robot robot_;

    public Robot createRobot()
    {
        return Instantiate<Robot>( robot_ );
    }
}
