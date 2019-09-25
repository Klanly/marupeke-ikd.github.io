using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KugiPoint : MonoBehaviour
{
    [SerializeField]
    string pointName_;

    [SerializeField]
    SpriteButton point_;

    [SerializeField]
    TextMesh countText_;

    [SerializeField]
    GameObject KugiPrefab_;


    // string: 箇所の名前
    // int: 残り規定回数
    // bool: 最初のヒットか？
    public System.Action< string, int, bool > HitCallback { set { hitCallback_ = value; } }
    System.Action< string, int, bool > hitCallback_;

    public void setActive( bool isActive ) {
        point_.setEnable( isActive );
    }

    public int getCount() {
        return kugiCount_;
    }
    // セットトップ
    //  kugiCount: 打ち込む釘の回数
    public void setup( int kugiCount ) {
        kugiCount_ = kugiCount;
        initKugiCount_ = kugiCount;
        setCounter( kugiCount );
    }

    // エクストリーム
    public void extreme(bool isExtreme) {
        if ( isExtreme == true ) {
            Vector3 ss = Vector3.one;
            Vector3 es = ss * 2.0f;
            GlobalState.time( 0.4f, (sec, t) => {
                if ( this == null )
                    return false;
                transform.localScale = Lerps.Vec3.easeOut( ss, es, t );
                return true;
            } );
        } else {
            Vector3 ss = transform.localScale;
            Vector3 es = Vector3.one;
            GlobalState.time( 0.4f, (sec, t) => {
                if ( this == null )
                    return false;
                transform.localScale = Lerps.Vec3.easeOut( ss, es, t );
                return true;
            } );
        }
    }

    void setCounter( int kugiCount ) {
        string[] countTexts = new string[] {
            "呪", "壱", "弐", "参", "四", "五", "六", "七", "八", "九", "多"
        };
        if ( kugiCount < 0 ) {
            countText_.text = "危";
        } else if ( kugiCount >= 0 && kugiCount <= 10 ) {
            countText_.text = countTexts[ kugiCount ];
        } else {
            countText_.text = "多";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 押し下げ時処理
        point_.OnDecide = ( btnName ) => {
            kugiCount_--;
            setCounter( kugiCount_ );
            if ( initKugiCount_ > 0 ) {
                if ( isFirstHit_ == true ) {
                    // 釘を刺す
                    kugi_ = PrefabUtil.createInstance( KugiPrefab_, transform );
                }
                // 釘を打ち込む
                if ( initKugiCount_ > 0 && kugiCount_ >= -1 ) {
                    kugi_.transform.localPosition = new Vector3( 0.0f, 0.0f, kugiInsertDist_ * ( 1.0f - ( float )kugiCount_ / initKugiCount_ ) );
                }
            }

            // 火花バシーン
            var p = GameManager.getInstance().ParticleEmitter.emit( "KugiHitPt" );
            p.transform.SetParent( transform );
            p.transform.localPosition = Vector3.zero;

            // 釘カウンター処理
            if ( hitCallback_ != null ) {
                hitCallback_( pointName_, kugiCount_, isFirstHit_ );
                isFirstHit_ = false;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int initKugiCount_ = 1;
    int kugiCount_ = 1;
    bool isFirstHit_ = true;
    GameObject kugi_;
    float kugiInsertDist_ = 1.5f;
}
