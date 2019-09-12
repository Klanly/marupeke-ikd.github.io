using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TehaiSet : MonoBehaviour
{
    [SerializeField]
    GameObject nullPaiPrefab_;

    // セットアップ
    //  spaceRate: 面子間のスペース。0で1牌分、1で2牌分
    public void setup( float paiWidth, float paiHeight, float spaceRate ) {
        w_ = paiWidth;
        h_ = paiHeight;
        space_ = ( spaceRate + 1.0f ) * w_ * 0.5f;
        unit_ = w_ * 1.5f + space_;

        createNullPais();
    }

    // 対子をセット
    public void addToitsu( MenzenSet toitsu ) {
        var list = toitsu.Pais;
        float c = 9.0f * unit_ - w_ * 0.5f;
        for ( int i = 0; i < 2; ++i ) {
            toitus_[ i ] = GameManager.getInstance().PaiGenerator.create( list[ i ].getPai().PaiType );
            toitus_[ i ].transform.SetParent( transform );
        }
        toitus_[ 0 ].transform.localPosition = new Vector3( c - w_ * 0.5f, h_ * 0.5f, 0.0f );
        toitus_[ 1 ].transform.localPosition = new Vector3( c + w_ * 0.5f, h_ * 0.5f, 0.0f );

        foreach( var p in nullPais[ 4 ] ) {
            Destroy( p.gameObject );
        }
    }

    // 面子をセット
    public void addMentsu(MenzenSet mentsu ) {
        var list = mentsu.Pais;
        float c = ( 1.0f + 2.0f * mentsuList_.Count ) * unit_;
        if ( list.Count == 3 ) {
            var pais = new PaiObject[ 3 ];
            for ( int i = 0; i < 3; ++i ) {
                pais[ i ] = GameManager.getInstance().PaiGenerator.create( list[ i ].getPai().PaiType );
                pais[ i ].transform.SetParent( transform );
            }
            pais[ 0 ].transform.localPosition = new Vector3( c - w_, h_ * 0.5f, 0.0f );
            pais[ 1 ].transform.localPosition = new Vector3( c     , h_ * 0.5f, 0.0f );
            pais[ 2 ].transform.localPosition = new Vector3( c + w_, h_ * 0.5f, 0.0f );
            mentsuList_.Add( pais );
        } else if ( list.Count == 4 ) {
            var pais = new PaiObject[ 4 ];
            for ( int i = 0; i < 4; ++i ) {
                pais[ i ] = GameManager.getInstance().PaiGenerator.create( list[ i ].getPai().PaiType );
                pais[ i ].transform.SetParent( transform );
            }
            pais[ 0 ].transform.localPosition = new Vector3( c - w_ * 1.5f, h_ * 0.5f, 0.0f );
            pais[ 1 ].transform.localPosition = new Vector3( c - w_ * 0.5f, h_ * 0.5f, 0.0f );
            pais[ 2 ].transform.localPosition = new Vector3( c + w_ * 0.5f, h_ * 0.5f, 0.0f );
            pais[ 3 ].transform.localPosition = new Vector3( c + w_ * 1.5f, h_ * 0.5f, 0.0f );
            mentsuList_.Add( pais );
        }
        foreach ( var p in nullPais[ mentsuList_.Count - 1 ] ) {
            Destroy( p.gameObject );
        }
    }

    void createNullPais() {
        float c = 0.0f;
        GameObject[] pais = null;
        for ( int u = 0; u < 4; ++u ) {
            pais = new GameObject[ 3 ];
            for ( int i = 0; i < 3; ++i ) {
                pais[ i ] = PrefabUtil.createInstance<GameObject>( nullPaiPrefab_, transform );
                pais[ i ].transform.SetParent( transform );
            }
            c = ( 1.0f + 2.0f * u ) * unit_;
            pais[ 0 ].transform.localPosition = new Vector3( c - w_, h_ * 0.5f, 0.0f );
            pais[ 1 ].transform.localPosition = new Vector3( c, h_ * 0.5f, 0.0f );
            pais[ 2 ].transform.localPosition = new Vector3( c + w_, h_ * 0.5f, 0.0f );
            nullPais.Add( pais );
        }
        pais = new GameObject[ 2 ];
        for ( int i = 0; i < 2; ++i ) {
            pais[ i ] = PrefabUtil.createInstance<GameObject>( nullPaiPrefab_, transform );
            pais[ i ].transform.SetParent( transform );
        }
        c = 9.0f * unit_ - w_ * 0.5f;
        pais[ 0 ].transform.localPosition = new Vector3( c - w_ * 0.5f, h_ * 0.5f, 0.0f );
        pais[ 1 ].transform.localPosition = new Vector3( c + w_ * 0.5f, h_ * 0.5f, 0.0f );
        nullPais.Add( pais );
    }

    private void Awake() {
        space_ = w_ * 0.6f;
        unit_ = w_ * 1.5f + space_;

        // NullPai作成
        createNullPais();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float w_ = 2.0f;
    float h_ = 2.35f;
    float unit_ = 0.0f;
    float space_ = 0.0f;

    PaiObject[] toitus_ = new PaiObject[ 2 ];
    List<PaiObject[]> mentsuList_ = new List<PaiObject[]>();
    List<GameObject[]> nullPais = new List<GameObject[]>();
}
