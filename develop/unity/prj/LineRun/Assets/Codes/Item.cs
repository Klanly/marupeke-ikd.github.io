using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	// 取得モーション
	public void getMotion()
	{
		var rigidBody = GetComponent<Rigidbody>();
		if ( rigidBody != null ) {
			Destroy( rigidBody );
		}
		float tm = LerpAction.jump( g:19.6f, calcFinishTime: true, buildUpTime: 0.05f, sinkDist: 0.05f, getDownHeight: 1.0f, overDist: 0.9f );
		GlobalState.time( tm, (sec, t) => {
			if (this == null)
				return false;
			var p = transform.position;
			p.z = -LerpAction.jump( g: 19.6f, buildUpTime: 0.05f, sinkDist: 0.05f, getDownHeight: 1.0f, overDist: 0.9f, t: sec * 2.0f );
			transform.position = p;
			if ( t >= 0.5f ) {
				Destroy( gameObject );
				return false;
			}
			return true;
		} ).finish(()=> {
		} );
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
