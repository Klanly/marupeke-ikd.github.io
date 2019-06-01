using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreateTest : MonoBehaviour {

    [SerializeField]
    GameObject roomPrefab_;

    [SerializeField]
    MazeMesh mazeMesh_;

    [SerializeField]
    int levelNum_ = 2;

	// Use this for initialization
	void Start () {
        var param = new MazeCreator.Parameter();
        param.level_ = levelNum_;
        MazeCreator.create( ref param );
        mazeMesh_.setParam( param );

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
