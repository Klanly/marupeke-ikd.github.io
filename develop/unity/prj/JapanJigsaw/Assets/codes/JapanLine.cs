using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JapanLine : MonoBehaviour {

    [SerializeField]
    Material material_;

    [SerializeField]
    Color color_ = Color.white;

    [SerializeField]
    int lineIndex = 200000;

    private void Awake()
    {
        ResourceLoader.getInstance().loadAsync<TextAsset>( "JapanMap.dat", ( res, textAsset ) => {
            byte[] bytes = textAsset.bytes;
            int pos = 0;
            centerLongi_ = System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
            centerLat_ = System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
            minX_ = System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
            minY_ = System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
            maxX_ = System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
            maxY_ = System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
            partSize_ = System.BitConverter.ToUInt32( bytes, pos ); pos += sizeof( uint );

            for ( uint part = 0; part < partSize_; ++part ) {
                uint partNumber = System.BitConverter.ToUInt32( bytes, pos ); pos += sizeof( uint );
                uint pointNum = System.BitConverter.ToUInt32( bytes, pos ); pos += sizeof( uint );
                var point = new Vector3( 0.0f, 0.0f, 0.0f );
                point.x = (float)System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
                point.z = (float)System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
                points.Add( point );
                for ( uint i = 0; i < pointNum - 1; ++i ) {
                    var p = new Vector3( 0.0f, 0.0f, 0.0f );
                    p.x = (float)System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
                    p.z = (float)System.BitConverter.ToDouble( bytes, pos ); pos += sizeof( double );
                    p.y = 0.0f;
                    points.Add( p );
                    points.Add( p );
                }
                points.Add( point );
            }
            bReady_ = true;
        } );
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( bReady_ == false )
            return;
	}

    bool bReady_ = false;
    double centerLongi_ = 0.0;
    double centerLat_ = 0.0;
    double minX_ = 0.0;
    double minY_ = 0.0;
    double maxX_ = 0.0;
    double maxY_ = 0.0;
    uint partSize_ = 0;
    List<Vector3> points = new List<Vector3>();

    void OnRenderObject()
    {
        if ( bReady_ == true )
            drawLine();
    }

    // 線分描画
    void drawLine()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Matrix4x4 trans = Matrix4x4.Translate( pos );
        Matrix4x4 rotMat = Matrix4x4.Rotate( rot );
        Matrix4x4 mtx = trans * rotMat;
        material_.SetPass( 0 );

        GL.PushMatrix();
        GL.MultMatrix( mtx );

        GL.Begin( GL.LINES );
        GL.Color( color_ );

        if ( lineIndex > points.Count / 2 )
            lineIndex = points.Count / 2;

        for ( int i = 0; i < points.Count / 2 - lineIndex; ++i ) {
            GL.Vertex( points[ i * 2 + 0 ] );
            GL.Vertex( points[ i * 2 + 1 ] );
        }
        GL.End();
        GL.PopMatrix();
    }
}
