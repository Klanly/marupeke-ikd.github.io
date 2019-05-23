using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 流れる文字列生成機
public class StringLineGenerator : MonoBehaviour {

    [SerializeField]
    StringLine stringLinePrefab_;

    [SerializeField]
    int num_ = 50;

    [SerializeField]
    Vector3 ignoreDist_ = new Vector3( 30.0f, 30.0f, 30.0f );

    [SerializeField]
    Vector3 maxDist_ = new Vector3( 500.0f, 500.0f, 500.0f );

    [SerializeField]
    Transform root_;

	// Use this for initialization
	void Start () {
		for ( int i = 0; i < num_; ++i ) {
            var obj = Instantiate<StringLine>( stringLinePrefab_ );
            obj.transform.SetParent( root_ );
            var p = ignoreDist_ + ( Randoms.Vec3.value( maxDist_ ) );
            p.x *= ( Random.Range( 0, 2 ) % 2 == 0 ? 1 : -1 );
            p.y *= ( Random.Range( 0, 2 ) % 2 == 0 ? 1 : -1 );
            p.z *= ( Random.Range( 0, 2 ) % 2 == 0 ? 1 : -1 );
            obj.Pos = p;
            float dirVal = Random.Range( 0, 2 ) % 2 == 0 ? 1 : -1;
            var dir = Vector3.zero;
            switch ( Random.Range( 0, 3 ) ) {
                case 0: dir.x = dirVal; break;
                case 1: dir.y = dirVal; break;
                case 2: dir.z = dirVal; break;
            }
            obj.Dir = dir;
            lines_.Add( obj );
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    List<StringLine> lines_ = new List<StringLine>();
}
