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
        int[] keyNums = new int[] {
            1, 1, 1, 2, 2, 2, 3, 3, 4, 5
        };
        if ( levelNum_ <= keyNums.Length ) {
            placeKeyNum_ = keyNums[ levelNum_ - 1 ];
        }
    }

    public MazeCreator.Parameter getParam() {
        return mazeMesh_.getParam();
    }

    public int getKeyNum() {
        return placeKeyNum_;
    }

    // Use this for initialization
    void Start () {
        var param = new MazeCreator.Parameter();
        levelNum_ = levelNum_ <= 0 ? 1 : levelNum_;
        param.level_ = levelNum_;
        MazeCreator.create( ref param );
        mazeMesh_.setParam( param );

        // 鍵を設置
        var hash = new HashSet<Vector3Int>();
        for ( int i = 0; i < placeKeyNum_; ++i ) {
            int x = 0;
            int z = 0;
            int level = 0;
            var val = Vector3Int.zero;
            while ( true ) {
                level = Random.Range( 0, levelNum_ - 1 );
                x = Random.Range( 0, levelNum_ - level );
                z = Random.Range( 0, levelNum_ - level );
                val.x = x;
                val.y = level;
                val.z = z;
                if ( hash.Contains( val ) == false  ) {
                    hash.Add( val );
                    break;
                }
            }
            var key = PrefabUtil.createInstance<Item>( keyPrefab_, transform );
            var cell = param.cellLevel_[ level ].cells_[ z, x ];
            key.transform.localPosition = cell.localPos_;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    int placeKeyNum_ = 6;
}
