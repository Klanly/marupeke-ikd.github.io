using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstLine : MonoBehaviour {

    [SerializeField]
    Transform rot_;

    [SerializeField]
    GameObject line_;

    public void setLine( Vector3 start, Vector3 end )
    {
        // Y軸はstart-endライン
        Vector3 y = ( end - start ).normalized;

        // 仮Zはstart-end中点と原点
        Vector3 pos = ( start + end ) * 0.5f;
        Vector3 z = -pos.normalized;

        // X軸
        Vector3 x = Vector3.Cross( y, z ).normalized;

        // Z軸
        z = Vector3.Cross( x, y ).normalized;

        // 位置と回転とスケールを適用
        float scale = ( end - start ).magnitude * 0.5f;
        var q = Quaternion.LookRotation( z, y );
        transform.localPosition = pos;
        rot_.transform.localRotation = q;
        line_.transform.localScale = new Vector3( 1.0f, scale, 1.0f );
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
