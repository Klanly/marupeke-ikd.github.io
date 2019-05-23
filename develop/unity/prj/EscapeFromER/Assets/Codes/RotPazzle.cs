using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPazzle : MonoBehaviour {

    [SerializeField]
    List< RotPazzleFrame > frames_;

    public System.Action ClearCallback { set { clearCallback_ = value; } }
    public System.Action clearCallback_;

    public void setCamera( Camera camera ) {
        foreach ( var f in frames_ ) {
            f.setCamera( camera );
        }
    }

    private void Awake() {
            
    }

    // Use this for initialization
    void Start () {
        GlobalState.start( () => {
            bool bOK = true;
            foreach ( var f in frames_ ) {
                if ( f.isOK() == false )
                    bOK = false;
            }
            if ( bOK == true ) {
                Debug.Log( "OK!" );
                foreach ( var f in frames_ ) {
                    f.lockRot();
                }
                if ( clearCallback_ != null )
                    clearCallback_();
                return false;
            }
            return true;
        } );
	}
	
	// Update is called once per frame
	void Update () {
	}
}
