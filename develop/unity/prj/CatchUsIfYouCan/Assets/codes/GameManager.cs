using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject objectRoot_;

    [SerializeField]
    BulletFactory bulletFactory_;

    [SerializeField]
    FieldFactory fieldFactory_;

    [SerializeField]
    SphereField fieldPrefab_;

    [SerializeField]
    Human humanPrefab_;

    [SerializeField]
    float fieldRadius_ = 300.0f;

    [SerializeField]
    Camera camera_;

	// Use this for initialization
	void Start () {
        field_ = Instantiate<SphereField>( fieldPrefab_ );
        field_.transform.parent = transform;
        field_.transform.localPosition = Vector3.zero;
        field_.setRadius( fieldRadius_ );

        human_ = Instantiate<Human>( humanPrefab_ );
        human_.transform.parent = objectRoot_.transform;
        human_.transform.localPosition = Vector3.zero;
        human_.setup( field_.getRadius(), new Vector3( 0.0f, 0.0f, -1.0f ), new Vector3( 0.0f, 1.0f, 0.0f ) );
        human_.setAction( Human.ActionState.ActionState_Run );

        camera_.transform.parent = human_.transform;
        camera_.transform.localPosition = new Vector3( 0.0f, 25.0f, -20.0f );
        camera_.transform.localRotation = Quaternion.LookRotation( -camera_.transform.localPosition + new Vector3( 0.0f, 0.0f, 10.0f ) );

        // 弾テスト
        //  適当に50発位あちこちに
        int num = 150;
        for ( int i = 0; i < num; ++i ) {
            var bullet = bulletFactory_.create();
            bullet.transform.parent = objectRoot_.transform;
            bullet.transform.parent = objectRoot_.transform;
            bullet.transform.localPosition = Vector3.zero;
            var bpos = SphereSurfUtil.randomPos( Random.value, Random.value );
            var v = SphereSurfUtil.randomPos( Random.value, Random.value );
            bullet.setup( field_.getRadius(), bpos, v );
        }
    }
	
	// Update is called once per frame
	void Update () {
 	}

    Human human_;
    SphereField field_;
}
