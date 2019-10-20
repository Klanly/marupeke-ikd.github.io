using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField]
	GameObject block_;

	// エミット
	public void emit( float len, float width, float height, float deg )
	{
		block_.transform.localScale = new Vector3( len, width, height );
		block_.transform.rotation = Quaternion.Euler( 1.0f, 1.0f, deg );
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
