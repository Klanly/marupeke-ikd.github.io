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

    // name: 箇所の名前
    // int: 残り規定回数
    public System.Action< string, int > HitCallback { set { hitCallback_ = value; } }
    System.Action< string, int > hitCallback_;

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
        setCounter( kugiCount );
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
            // 火花バシーン
            var p = GameManager.getInstance().ParticleEmitter.emit( "KugiHitPt" );
            p.transform.SetParent( transform );
            p.transform.localPosition = Vector3.zero;

            // 釘カウンター処理
            kugiCount_--;
            setCounter( kugiCount_ );
            if ( hitCallback_ != null )
                hitCallback_( pointName_, kugiCount_ );
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int kugiCount_ = 1;
}
