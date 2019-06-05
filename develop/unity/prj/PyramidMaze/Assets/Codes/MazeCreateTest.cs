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

    [SerializeField]
    List<GameObject> stones_;

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

        // 石をばらまく
        var offset = new Vector3( 0.0f, 0.5f, 0.0f );
        for ( int l = 0; l < levelNum_; ++l ) {
            for ( int z = 0; z < levelNum_ - l; ++z ) {
                for ( int x = 0; x < levelNum_ - l; ++x ) {
                    int num = Random.Range( 0, 7 );
                    for ( int n = 0; n < num; ++n ) {
                        var cel = param.cellLevel_[ l ].cells_[ z, x ];
                        var pos = Randoms.Vec3.valueCenterXZ() * 0.5f;
                        if ( cel.existFloor( pos ) == true ) {
                            pos += cel.localPos_ - offset;
                            var scale = Vector3.one * Random.Range( 0.1f, 0.7f );
                            var rot = Quaternion.Euler( 0.0f, Random.Range( 0.0f, 2.0f * Mathf.PI ), 0.0f );
                            var stone = PrefabUtil.createInstance( stones_[ Random.Range( 0, stones_.Count ) ], mazeMesh_.transform );
                            stone.transform.localPosition = pos;
                            stone.transform.localRotation = rot;
                            stone.transform.localScale = scale;
                        }
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    int placeKeyNum_ = 6;
}
