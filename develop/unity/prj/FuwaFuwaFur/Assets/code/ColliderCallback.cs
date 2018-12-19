using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCallback : MonoBehaviour {

    public System.Action<Collider> onTriggerEnter_ = null;
    public System.Action<Collider> onTriggerStay_ = null;
    public System.Action<Collider> onTriggerExit_ = null;
    public System.Action<Collision> onCollisionEnter_ = null;
    public System.Action<Collision> onCollisionStay_ = null;
    public System.Action<Collision> onCollisionExit_ = null;

    private void OnTriggerEnter(Collider collision)
    {
        if ( onTriggerEnter_ != null )
            onTriggerEnter_( collision );
    }

    private void OnTriggerStay(Collider collision)
    {
        if ( onTriggerStay_ != null )
            onTriggerStay_( collision );
    }

    private void OnTriggerExit(Collider collision)
    {
        if ( onTriggerExit_ != null )
            onTriggerExit_( collision );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( onCollisionEnter_ != null )
            onCollisionEnter_( collision );
    }

    private void OnCollisionStay(Collision collision)
    {
        if ( onCollisionStay_ != null )
            onCollisionStay_( collision );
    }

    private void OnCollisionExit(Collision collision)
    {
        if ( onCollisionExit_ != null )
            onCollisionExit_( collision );
    }
}
