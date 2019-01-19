using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject objectRoot_;

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
        human_.setSpeed( 10.0f );
        human_.setAction( Human.ActionState.ActionState_Run );

        camera_.transform.parent = human_.transform;
        camera_.transform.localPosition = new Vector3( 0.0f, 25.0f, -20.0f );
        camera_.transform.localRotation = Quaternion.LookRotation( -camera_.transform.localPosition + new Vector3( 0.0f, 0.0f, 10.0f ) );
    }
	
	// Update is called once per frame
	void Update () {
 	}

    Human human_;
    SphereField field_;
}
