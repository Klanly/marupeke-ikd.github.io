using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMesh : MonoBehaviour {
#if false
    [SerializeField]
    float innerRadius_ = 100.0f;    // 内部半径

    [SerializeField]
    float degree_ = 5.0f;           // 内角度

    [SerializeField]
    float tickness_ = 10.0f;        // 厚み
#endif

    [SerializeField]
    Transform rotZTrans;

    // 壁メッシュ生成
    public void setup( float innerRadius, float degree, float tickness ) {
        // メッシュ作成
        float rad = degree * Mathf.Deg2Rad * 0.5f;
        float r = innerRadius;
        float h = tickness / 2.0f;
        float x0 = r * Mathf.Sin( rad );
        float x1 = ( r + 2 * h ) * Mathf.Sin( rad );
        float z0 = -( r + h ) + r * Mathf.Cos( rad );
        float z1 = ( r + 2 * h ) * Mathf.Cos( rad ) - ( r + h );
        float y = h;

        collitionRadius_ = ( r + h ) * Mathf.Sin( rad );

        // 頂点座標
        Vector3[] vertices = {
            new Vector3( -x0, -y, z0 ),   // 0
            new Vector3( -x0,  y, z0 ),   // 1
            new Vector3(   0, -y, -h ),   // 2
            new Vector3(   0,  y, -h ),   // 3
            new Vector3(  x0, -y, z0 ),   // 4
            new Vector3(  x0,  y, z0 ),   // 5

            new Vector3( -x1, -y, z1 ),   // 6
            new Vector3( -x1,  y, z1 ),   // 7
            new Vector3(   0, -y,  h ),   // 8
            new Vector3(   0,  y,  h ),   // 9
            new Vector3(  x1, -y, z1 ),   // 10
            new Vector3(  x1,  y, z1 ),   // 11
        };

        // インデックス
        int[] indices = {
            0, 1, 2,
            1, 3, 2,
            2, 3, 4,
            3, 5, 4,

            4, 5, 10,
            5, 11, 10,

            10, 11, 8,
            11, 9, 8,
            8, 9, 6,
            9, 7 ,6,

            6, 7, 0,
            7, 1, 0,

            1, 7, 3,
            7, 9, 3,
            3, 9, 5,
            9, 11, 5,

            4, 10, 2,
            10, 8, 2,
            2, 8, 0,
            8, 6, 0,
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();

        var filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }

    public float getCollideRadius()
    {
        return collitionRadius_;
    }

    public void setColor( Color color )
    {
        var renderer = GetComponent<MeshRenderer>();
        if ( renderer == null )
            return;
        renderer.material.color = color;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    float collitionRadius_ = 0.0f;
}
