using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEmitter : MonoBehaviour {

    [SerializeField]
    FireBar fireBar_;

    // 炎を指定方向へ伸長する
    public void fire( FireBar.FireDirection dir, Vector3 pos, float speed, float dist )
    {
        FireBar obj = Instantiate<FireBar>( fireBar_ );
        obj.fire( dir, pos, speed, dist );
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }
}
