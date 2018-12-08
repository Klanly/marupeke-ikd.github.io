using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    [SerializeField]
    SphereCollider collider_;

    public void set( int id, int index, System.Action<Missile, LandoltEnemy, bool> resultCallback )
    {
        id_ = id;
        index_ = index;
        resultCallback_ = resultCallback;
    }

    public int getId()
    {
        return id_;
    }

    public int getIndex()
    {
        return index_;

    }
    // 指定方向へショット
    public void shot( Transform cannon, Vector3 dir, float speed )
    {
        dir_ = dir;
        transform.localPosition = cannon.position;
        transform.localRotation = cannon.rotation;
        state_ = new Shot( this, dir, speed );
    }

    public Vector3 getDir()
    {
        return dir_;
    }

    public float getRadius()
    {
        return collider_.radius;
    }

    public void destroyMissile()
    {
        // コアに当たらず爆発した
        resultCallback_( this, null, false );
        GameObject.Destroy( gameObject );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null ) {
            state_ = state_.update();
        }

        // 一定以上の距離を飛んだら消す
        if ( transform.position.magnitude >= 400.0f ) {
            resultCallback_( this, null, false );
            GameObject.Destroy( gameObject );
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.tag == "Core" ) {
            LandoltEnemy enemy = getEnemyObject( other.gameObject );
            resultCallback_( this, enemy, true );
            GameObject.DestroyObject( gameObject );
        }
    }

    LandoltEnemy getEnemyObject( GameObject go ) 
    {
        LandoltEnemy enemy = go.GetComponent< LandoltEnemy >();
        if ( enemy != null )
            return enemy;
        if ( go.transform.parent == null )
            return null;
        return getEnemyObject( go.transform.parent.gameObject );
    }

    Vector3 dir_;
    State state_;
    System.Action<Missile, LandoltEnemy, bool> resultCallback_;
    int id_ = 0;
    int index_ = 0;


    // ショット
    class Shot : State
    {
        public Shot( Missile manager, Vector3 dir, float speed )
        {
            manager_ = manager;
            dir_ = dir.normalized;
            speed_ = speed;
        }
        // 内部初期化
        override protected void innerInit()
        {

        }

        // 内部状態
        override protected State innerUpdate()
        {
            // 1秒間にdir_ * speed_分だけ移動
            Vector3 p = manager_.transform.localPosition;
            p += dir_ * speed_ * Time.deltaTime;
            manager_.transform.localPosition = p;

            speed_ += accUnit_;

            return this;
        }

        Missile manager_;
        Vector3 dir_;
        float speed_;
        float accUnit_ = 11.0f;
        float acc_ = 0.0f;
    }
}
