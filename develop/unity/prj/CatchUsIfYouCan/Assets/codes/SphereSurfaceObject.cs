using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSurfaceObject : MonoBehaviour {

    [SerializeField]
    protected float speed_ = 0.0f;

    [SerializeField]
    protected float lrSpeed_ = 1.0f;

    virtual public void setup(float r, Vector3 initPos, Vector3 initDir)
    {
        cont_.setRadius( r );
        cont_.setPosDirect( initPos.normalized * r );
        cont_.setDir( initDir );
        transform.position = cont_.getPos();
        transform.rotation = Quaternion.LookRotation( cont_.getForward(), cont_.getUp() );
        setSpeed( speed_ );
    }

    // 速さを変更
    virtual public void setSpeed(float speed)
    {
        cont_.setSpeed( speed );
        speed_ = speed;
    }

    // 左右に曲がるスピードを変更
    virtual public void setLRSpeed(float speed)
    {
        lrSpeed_ = speed;
    }

    virtual protected void innerUpdate()
    {
        cont_.update();
        transform.position = cont_.getPos();
        if ( cont_.getForward().magnitude > 0.0f )
            transform.rotation = Quaternion.LookRotation( cont_.getForward(), cont_.getUp() );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected SphereSurfaceController cont_ = new SphereSurfaceController();
}
