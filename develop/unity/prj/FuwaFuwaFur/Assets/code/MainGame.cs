using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {

    [SerializeField]
    StageManager stageManager_;

    public bool isFinish()
    {
        return stageManager_.isFinish();
    }
}
