using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ビル生成管理人
public class BillCreator : MonoBehaviour
{
    [SerializeField]
    Transform root_;

    [SerializeField]
    InteriorMapping billPrefab_;

    [SerializeField]
    int billNum_ = 10;  // ビルの数

    [SerializeField]
    float radius_ = 1000.0f;    // 生成半径

    [SerializeField]
    float billMaxHeight_ = 280.0f;  // ビル最大高

    [SerializeField]
    float billMinHeight_ = 10.0f;   // ビル最小高


    public void create() {
        removeAll();
        for ( int i = 0; i < billNum_; ++i ) {
            var bill = Instantiate<InteriorMapping>( billPrefab_ );
            bill.transform.SetParent( root_ );

            bill.Height1Floor = Random.Range( 2.7f, 3.3f );
            bill.RoomWidth = Random.Range( 4.0f, 6.0f );
            bill.RoomDepth = Random.Range( 4.0f, 6.0f );

            // ビルの高さと底面を設定
            // 塔状比: 幅と高さの比率。4を超えると塔状建物と言うらしい。6が限界位らしい。
            float height = Random.Range( billMinHeight_, billMaxHeight_ );
            float width = height / Random.Range( 3.5f, 6.0f );
            float depth = width * Random.Range( 0.7f, 1.2f );
            int sepW = ( int )( width / bill.RoomWidth );
            int sepD = ( int )( depth / bill.RoomDepth );
            int sepH = ( int )( height / bill.Height1Floor );
            bill.RoomSep = new Vector4( sepW, sepH, sepD, 0.0f );

            // 窓枠幅比率
            bill.OutWallTickness = new Vector4( Random.Range( 0.7f, 0.95f ), Random.Range( 0.5f, 0.95f ), Random.Range( 0.7f, 0.95f ), 0.0f );

            // 外壁色
            float baseC = Random.Range( 0.3f, 0.85f );
            bill.OutWallColor = new Color( baseC, baseC, baseC, 1.0f );

            // 窓の透過度
            bill.WindowTransRate = Random.Range( 0.5f, 0.85f );

            // 位置
            float r = Mathf.Sqrt( Random.value ) * radius_;
            float angle = Random.value * 2 * Mathf.PI;
            var pos = new Vector3( r * Mathf.Cos( angle ), 0.0f, r * Mathf.Sin( angle ) );
            bill.transform.localPosition = pos;

            bills_.Add( bill.gameObject );
        }
    }

    void removeAll() {
        foreach ( var b in bills_ ) {
            Destroy( b );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        create();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKey( KeyCode.R) ) {
            create();
        }
    }

    List<GameObject> bills_ = new List<GameObject>();
}
