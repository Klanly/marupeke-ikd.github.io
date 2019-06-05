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

    [SerializeField]
    Item keyPrefab_;

    public void setup( int level ) {
        levelNum_ = level;
    }

    public MazeCreator.Parameter getParam() {
        return mazeMesh_.getParam();
    }

	// Use this for initialization
	void Start () {
        var param = new MazeCreator.Parameter();
        param.level_ = levelNum_;
        MazeCreator.create( ref param );
        mazeMesh_.setParam( param );

        // 鍵を設置
        {
            var key = PrefabUtil.createInstance<Item>( keyPrefab_, transform );
            int level = Random.Range( 0, levelNum_ - 1 );
            int x = Random.Range( 0, levelNum_ - level );
            int z = Random.Range( 0, levelNum_ - level );
            var cell = param.cellLevel_[ level ].cells_[ z, x ];
            key.transform.localPosition = cell.localPos_;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
