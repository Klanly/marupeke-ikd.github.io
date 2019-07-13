using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabageInfoWindow : MonoBehaviour
{
    [SerializeField]
    MoveWindowFrame frame_;

    [SerializeField]
    MultiMeshText terms_;

    [SerializeField]
    MultiMeshText infos_;

    [SerializeField]
    List<string> termStrs_;

    [SerializeField]
    List<string> infoStrs_;

    [SerializeField]
    float height_ = 0.0f;

    [SerializeField]
    float infoMergin_ = 1.0f;

    [SerializeField]
    StretchLine indicateLine1_;

    [SerializeField]
    StretchLine indicateLine2_;

    [SerializeField]
    float lineStretchSec_ = 1.0f;

    [ SerializeField]
    bool debugStart_;

    public void start() {
        terms_.clear();
        infos_.clear();
        for ( int i = 0; i < termStrs_.Count; ++i )
            terms_.setStr( i, termStrs_[ i ] );
        for ( int i = 0; i < infoStrs_.Count; ++i )
            infos_.setStr( i, infoStrs_[ i ] );
        terms_.setHeight( height_ );
        infos_.setHeight( height_ );
        terms_.updateAll();
        infos_.updateAll();
        var pos = infos_.transform.localPosition;
        pos.x = terms_.transform.localPosition.x + terms_.getRegion().x + infoMergin_;
        pos.y = terms_.transform.localPosition.y;
        infos_.transform.localPosition = pos;

        frame_.gameObject.SetActive( false );
        terms_.gameObject.SetActive( false );
        infos_.gameObject.SetActive( false );

        // ラインを伸ばす
        float L1 = indicateLine1_.getLength();
        float L2 = indicateLine2_.getLength();
        float L = L1 + L2;
        float u = L / lineStretchSec_;
        indicateLine1_.setRate( 0.0f );
        indicateLine2_.setRate( 0.0f );

        GlobalState.time( lineStretchSec_, (sec, t) => {
            float Lc = sec * u;
            float r1 = Lc / L1;
            float r2 = ( Lc - L1 ) / L2;
            indicateLine1_.setRate( r1 );
            indicateLine2_.setRate( r2 );
            return true;
        } ).next( () => {
            // フレーム動作開始
            frame_.gameObject.SetActive( true );
            frame_.start( (res) => {
                if ( res == false )
                    return;
                terms_.gameObject.SetActive( true );
                infos_.gameObject.SetActive( true );
            } );
            return false;
        } );
    }

    private void Awake() {
        frame_.gameObject.SetActive( false );
        terms_.gameObject.SetActive( false );
        infos_.gameObject.SetActive( false );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( debugStart_ == true ) {
            debugStart_ = false;
            start();
        }        
    }
}
