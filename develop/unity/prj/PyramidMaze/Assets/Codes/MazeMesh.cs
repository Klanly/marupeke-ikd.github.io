using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]
public class MazeMesh : MonoBehaviour {

    [SerializeField]
    Material sourceMaterial_;

    //[SerializeField]
    //Light light_;

    public void setParam(MazeCreator.Parameter param ) {
        var vertices = new List<Vector3>();
        var indices = new List<int>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        int idx = 0;
        foreach ( var cl in param.cellLevel_ ) {
            for ( int z = 0; z < cl.cells_.GetLength( 1 ); ++z ) {
                for ( int x = 0; x < cl.cells_.GetLength( 0 ); ++x ) {
                    var cell = cl.cells_[ z, x ];
                    foreach ( var w in cell.wallMeshes_ ) {
                        int num = w.appendVertices( ref vertices, ref normals, ref uvs );
                        for ( int i = 0; i < num; ++i ) {
                            indices.Add( idx );
                            idx++;
                        }
                    }
                    // ライト
                    //var lt = Instantiate<Light>( light_ );
                    //lt.transform.localPosition = cell.localPos_;
                }
            }
        }
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        filter_.mesh = mesh;
        renderer_.material = material_;

        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        collider_ = gameObject.AddComponent<MeshCollider>();

        param_ = param;
    }

    public MazeCreator.Parameter getParam() {
        return param_;
    }

    public Collider getCollider() {
        return collider_;
    }

    public Cell getCellFromPosition( Vector3 position ) {
        if ( param_ == null )
            return null;
        // ピラミッドの構造からLevelとXZを算出
        float len = param_.roomWidthX_;
        int level = ( int )( position.y - 0.5 * len );
        if ( level >= param_.cellLevel_.Count )
            return null;

        float xf = position.x / len + 0.5f - level * 0.5f;
        int x = ( int )xf;
        float zf = position.z / len + 0.5f - level * 0.5f;
        int z = ( int )zf;

        if ( x < 0.0f || x >= param_.cellLevel_[ level ].cells_.GetLength( 0 ) )
            return null;
        if ( z < 0.0f || z >= param_.cellLevel_[ level ].cells_.GetLength( 1 ) )
            return null;

        return param_.cellLevel_[ level ].cells_[ z, x ];
    }

    private void Awake() {
        renderer_ = GetComponent<MeshRenderer>();
        filter_ = GetComponent<MeshFilter>();
        material_ = new Material( sourceMaterial_ );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    MeshRenderer renderer_;
    MeshFilter filter_;
    Material material_;
    MazeCreator.Parameter param_;
    MeshCollider collider_;
}
