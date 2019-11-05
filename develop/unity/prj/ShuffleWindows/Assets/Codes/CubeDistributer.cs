using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDistributer : MonoBehaviour
{
	[SerializeField]
	RotCube cubePrefab_;

	[SerializeField]
	int num_ = 10;

	[SerializeField]
	float radius_ = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
		for ( int i = 0; i < num_; ++i ) {
			var obj = PrefabUtil.createInstance( cubePrefab_, transform, Vector3.zero );
			obj.modelRoot_.transform.localPosition = SphereSurfUtil.randomPos( Random.value, Random.value ) * radius_;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
