using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPazzleFrame : MonoBehaviour {

    [SerializeField]
    CubeColor cubeColor_;
    
    [SerializeField]
    GameObject cubePrefab_;

    [SerializeField]
    Collider knob_;

    [SerializeField]
    Camera camera_;

    public enum CubeColor {
        Red,
        Green,
        Blue
    }

    public void setCamera( Camera camera ) {
        camera_ = camera;
    }

    public bool isOK() {
        return okVal[ ( int )cubeColor_ ] == rotCount_;
    }

    public void lockRot() {
        knob_.enabled = false;
    }

    // Use this for initialization
    void Start () {
        if ( camera_ == null )
            camera_ = Camera.main;

        // 各色の配置データ
        var redList = new Vector2Int[,] {
            {
                new Vector2Int( 2, 3 ),
                new Vector2Int( 0, 1 ),
                new Vector2Int( 1, 0 ),
                new Vector2Int( 3, 3 ),
                new Vector2Int( 2, 0 ),
                new Vector2Int( 3, 0 ),
                new Vector2Int( 2, 2 ),
                new Vector2Int( 3, 2 ),
            },
            {
                new Vector2Int( 0, 2 ),
                new Vector2Int( 1, 3 ),
                new Vector2Int( 4, 0 ),
                new Vector2Int( 2, 4 ),
                new Vector2Int( 2, 0 ),
                new Vector2Int( 3, 3 ),
                new Vector2Int( 0, 0 ),
                new Vector2Int( 2, 2 )
            },
            {
                // 正解(2)
                new Vector2Int( 1, 0 ),
                new Vector2Int( 4, 0 ),
                new Vector2Int( 2, 1 ),
                new Vector2Int( 0, 2 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 1, 3 ),
                new Vector2Int( 4, 3 ),
                new Vector2Int( 2, 4 )
            },
            {
                new Vector2Int( 3, 3 ),
                new Vector2Int( 3, 1 ),
                new Vector2Int( 3, 0 ),
                new Vector2Int( 3, 4 ),
                new Vector2Int( 4, 2 ),
                new Vector2Int( 2, 3 ),
                new Vector2Int( 0, 0 ),
                new Vector2Int( 2, 2 )
            }
        };
        var greenList = new Vector2Int[,] {
            {
                new Vector2Int( 3, 2 ),
                new Vector2Int( 2, 2 ),
                new Vector2Int( 4, 0 ),
                new Vector2Int( 3, 0 ),
                new Vector2Int( 2, 0 ),
                new Vector2Int( 0, 0 ),
                new Vector2Int( 1, 0 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 3, 1 )
            },
            {
                new Vector2Int( 3, 3 ),
                new Vector2Int( 2, 3 ),
                new Vector2Int( 0, 1 ),
                new Vector2Int( 4, 2 ),
                new Vector2Int( 4, 4 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 4, 3 ),
                new Vector2Int( 1, 2 ),
                new Vector2Int( 1, 1 )
            },
            {
                new Vector2Int( 0, 4 ),
                new Vector2Int( 1, 4 ),
                new Vector2Int( 0, 2 ),
                new Vector2Int( 3, 3 ),
                new Vector2Int( 4, 4 ),
                new Vector2Int( 1, 2 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 3, 1 ),
                new Vector2Int( 4, 1 )
            },
            {
                // 正解(3)
                new Vector2Int( 3, 0 ), 
                new Vector2Int( 0, 1 ),
                new Vector2Int( 3, 1 ),
                new Vector2Int( 2, 2 ),
                new Vector2Int( 4, 2 ),
                new Vector2Int( 1, 3 ),
                new Vector2Int( 3, 3 ),
                new Vector2Int( 0, 4 ),
                new Vector2Int( 2, 4 )
            }
        };
        var blueList = new Vector2Int[,] {
/*            {
                new Vector2Int( 4, 3 ),
                new Vector2Int( 3, 4 ),
                new Vector2Int( 1, 4 ),
                new Vector2Int( 2, 3 ),
                new Vector2Int( 0, 0 ),
                new Vector2Int( 1, 1 ),
                new Vector2Int( 3, 2 ),
                new Vector2Int( 0, 1 )
            },
 */           {
                // 正解(1)
                new Vector2Int( 1, 0 ),
                new Vector2Int( 4, 0 ),
                new Vector2Int( 2, 1 ),
                new Vector2Int( 1, 2 ),
                new Vector2Int( 3, 2 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 1, 4 ),
                new Vector2Int( 4, 4 )
            },
            {
                // 正解(1)
                new Vector2Int( 1, 0 ),
                new Vector2Int( 0, 1 ),
                new Vector2Int( 2, 1 ),
                new Vector2Int( 4, 1 ),
                new Vector2Int( 3, 2 ),
                new Vector2Int( 2, 3 ),
                new Vector2Int( 0, 4 ),
                new Vector2Int( 4, 4 )
            },
            {
                // 正解(1)
                new Vector2Int( 1, 0 ),
                new Vector2Int( 4, 0 ),
                new Vector2Int( 2, 1 ),
                new Vector2Int( 1, 2 ),
                new Vector2Int( 3, 2 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 1, 4 ),
                new Vector2Int( 4, 4 )
            },
            {
                // 正解(1)
                new Vector2Int( 1, 0 ),
                new Vector2Int( 4, 0 ),
                new Vector2Int( 2, 1 ),
                new Vector2Int( 1, 2 ),
                new Vector2Int( 3, 2 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 1, 4 ),
                new Vector2Int( 4, 4 )
            },
 /*           {
                new Vector2Int( 2, 4 ),
                new Vector2Int( 4, 4 ),
                new Vector2Int( 4, 1 ),
                new Vector2Int( 1, 2 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 1, 3 ),
                new Vector2Int( 1, 1 ),
                new Vector2Int( 2, 3 )
            },
            {
                new Vector2Int( 1, 0 ),
                new Vector2Int( 2, 3 ),
                new Vector2Int( 4, 2 ),
                new Vector2Int( 2, 2 ),
                new Vector2Int( 1, 3 ),
                new Vector2Int( 0, 0 ),
                new Vector2Int( 0, 3 ),
                new Vector2Int( 0, 2 )
            }
 */       };
        colorList_.Add( redList );
        colorList_.Add( greenList );
        colorList_.Add( blueList );

        // Cube作成
        int[] nums = { 8, 9, 8 };
        for ( int i = 0; i < nums[ ( int )cubeColor_ ]; ++i ) {
            var c = PrefabUtil.createInstance<GameObject>( cubePrefab_, transform );
            cubes_.Add( c );
        }

        rotCubes( 0, 0, 0.0f );
    }

    // 各キューブの配置換え
    void rotCubes(int pre, int cur, float t) {
        var list = colorList_[ ( int )cubeColor_ ];
        var one = Vector3.one;
        one.y = 0.0f;
        for ( int i = 0; i < list.GetLength( 1 ); ++i ) {
            var preP2 = list[ pre, i ];
            var curP2 = list[ cur, i ];
            var preP = ( new Vector3( preP2.x, 0.0f, preP2.y ) - ( one * 2.0f ) ) * 1.2f;
            var curP = ( new Vector3( curP2.x, 0.0f, curP2.y ) - ( one * 2.0f ) ) * 1.2f;
            cubes_[ i ].transform.localPosition = Vector3.Lerp( preP, curP, t );
        }
    }

    // Update is called once per frame
    void Update () {
		if ( Input.GetMouseButtonDown( 0 ) == true ) {
            Ray ray = camera_.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( knob_.Raycast( ray, out hit, 100.0f ) == true ) {
                int preRotCount = rotCount_;
                rotCount_ = ( rotCount_ + 1 ) % 4; 
                knob_.enabled = true;
                var s = transform.localRotation;
                var e = Quaternion.Euler( 0.0f, 90.0f, 0.0f ) * s;
                GlobalState.time( 0.5f, (sec, t) => {
                    transform.localRotation = Quaternion.Lerp( s, e, t );
                    rotCubes( preRotCount, rotCount_, t );
                    return true;
                } ).finish(()=> {
                    knob_.enabled = true;
                } );
            }
        }
	}

    List<Vector2Int[,]> colorList_ = new List<Vector2Int[,]>();
    int rotCount_ = 0;
    List<GameObject> cubes_ = new List<GameObject>();
    int[] okVal = new int[] { 2, 3, 1 };
}
