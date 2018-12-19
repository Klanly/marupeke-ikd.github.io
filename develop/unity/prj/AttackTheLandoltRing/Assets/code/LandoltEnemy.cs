using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandoltEnemy : MonoBehaviour {

    [SerializeField]
    Landolt landolt = null;

    [SerializeField]
    EnemyCore core_ = null;

    [SerializeField]
    Detonator detonator_ = null;

    [SerializeField]
    AudioSource audioSource_ = null;

    [SerializeField]
    AudioClip explosionSe_ = null;

    [SerializeField]
    AudioClip explosionSe2_ = null;

    [SerializeField]
    int baseScore_ = 100;


    public System.Action<Transform, float> FieldOverExplodeCallback {
        set { fieldOverExplodeCallback_ = value; }
        get { return fieldOverExplodeCallback_; }
    }

    public System.Action<LandoltEnemy> ExploadCallback {
        set { exploadCallback_ = value; }
        get { return exploadCallback_; }
    }

    // 初期化
    // speed : worldUnit / sec
    public void setup( float radius, float spaceDegree, float dirDegree, float speed, float fieldRadius )
    {
        speed_ = speed;
        fieldRadius_ = fieldRadius;

        Landolt obj = Instantiate<Landolt>( landolt );
        obj.setup( radius, spaceDegree );
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        landolt_ = obj;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, dirDegree );
        float dirRad = dirDegree * Mathf.Deg2Rad;
        dir_ = new Vector3( Mathf.Cos( dirRad ), Mathf.Sin( dirRad ), 0.0f ) * speed_;

        // コア
        enemyCore_ = Instantiate<EnemyCore>( core_ );
        enemyCore_.transform.parent = obj.transform;
        enemyCore_.transform.localPosition = new Vector3( -radius * 0.2f, 0.0f, 0.0f );
        enemyCore_.setup( () => {
            explode();
        } );

        // コライダー
        var collider = GetComponent<SphereCollider>();
        if ( collider != null ) {
            collider.radius = radius + obj.getTickness();
        }

        // 爆発音
        audioSource_.clip = Random.Range( 0, 2 ) == 0 ? explosionSe_ : explosionSe2_;
    }

    public int getBaseScore()
    {
        return baseScore_;
    }

    public float getMoveLen()
    {
        return transform.position.magnitude;
    }

    void explode()
    {
        if ( bExploded_ == false ) {
            audioSource_.Play();
            var collicer = GetComponent<SphereCollider>();
            if ( collicer != null ) {
                GameObject.Destroy( collicer );
            }

            if ( detonator_ != null ) {
                GameObject.Destroy( landolt_.gameObject );
                GameObject.Destroy( gameObject, 5.0f );
                detonator_.Explode();
                bExploded_ = true;

                if ( bOverField_ == false && exploadCallback_ != null )
                    exploadCallback_( this );
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( bExploded_ == false ) {
            var movePos = transform.localPosition + dir_ * Time.deltaTime;
            transform.localPosition = movePos;

            // 移動距離がフィールドをオーバーしたら爆発
            if ( bOverField_ == false && movePos.magnitude > fieldRadius_ ) {
                bOverField_ = true;
                fieldOverExplodeCallback_( transform, landolt_.getRadius() );
                explode();
            }

            // 移動量がGameDefines.quickRate_gまでは青
            // GameDefines.limitRate_gを超えたら赤に
            float lenRate = getMoveLen() / fieldRadius_;
            if ( lenRate <= GameDefines.quickRate_g )
                enemyCore_.setCoreColor( enemyQuiqkColor_ );
            else if ( lenRate >= GameDefines.limitRate_g ) {
                enemyCore_.setCoreColor( enemyLimitColor_ );
                enemyCore_.setLimitAction();
            } else
                enemyCore_.setCoreColor( enemyNormalColor_ );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Missile" ) {
            // ミサイルの飛ぶ方向の延長にコアがある &&
            // ミサイルがランドルド環の隙間の円柱内に収まっている -> コアに当たるミサイル！

            // ミサイルの飛ぶ方向の延長にコアがある？
            Missile m = other.gameObject.GetComponent<Missile>();
            if ( m != null ) {
                var mdir = m.getDir();
                var BC = transform.position - m.transform.position;
                float lbc = BC.magnitude;
                float d = Mathf.Abs( Vector3.Dot( mdir.normalized, BC ) );
                float l = Mathf.Sqrt( lbc * lbc - d * d );
                if ( l > core_.getRadius() + m.getRadius() || Vector3.Dot( dir_, mdir ) > 0.0f ) {
                    m.destroyMissile();
                    return;     // 当たらない
                }

                // 円柱に収まっている？
                float w = landolt_.getSpaceRadius();
                d = Mathf.Abs( Vector3.Dot( dir_.normalized, BC ) );
                l = Mathf.Sqrt( lbc * lbc - d * d );
                if ( w < l + m.getRadius() ) {
                    m.destroyMissile();
                    return;     // 当たらない
                }

                // 当たる可能性があるので素通り
            }
        }
    }

    Landolt landolt_;
    float speed_ = 0.0f;
    float fieldRadius_ = 10.0f;
    Vector3 dir_;
    bool bExploded_ = false;
    System.Action<Transform, float> fieldOverExplodeCallback_;
    System.Action<LandoltEnemy> exploadCallback_;
    bool bOverField_ = false;
    EnemyCore enemyCore_;
    Color enemyQuiqkColor_ = new Color( 0.4f, 0.4f, 0.9f );
    Color enemyNormalColor_ = new Color( 0.7f, 0.4f, 0.3f );
    Color enemyLimitColor_ = new Color( 0.9f, 0.2f, 0.2f );
}
