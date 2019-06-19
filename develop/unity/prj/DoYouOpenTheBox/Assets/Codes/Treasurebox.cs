using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasurebox : MonoBehaviour
{
    [SerializeField, Range( 0.0f, 190.0f )]
    float angle_;

    [SerializeField]
    Transform hinge_;

    [SerializeField]
    bool bValidateOpen_ = true;

    public System.Action<Treasurebox> ClickCallback { set { clickCallback_ = value;  } }

    // ミミック？
    public bool isMimic() {
        return bMimic_;
    }

    public void setFlapAngle(float angle) {
        angle_ = angle;
    }

    public void onClick() {
        if ( bOpen_ == true || bValidateOpen_ == false )
            return;
        clickCallback_( this );
    }

    // 箱開いてる？
    public bool isOpen() {
        return bOpen_;
    }

    // 箱を開く
    public void open( float sec ) {
        if ( bValidateOpen_ == false )
            return;

        bOpen_ = true;
        if ( collider_ != null ) {
            collider_.enabled = false;
        }
        openMotion( sec );
    }

    // 箱よ開け
    virtual protected void openMotion( float sec ) {
        GlobalState.time( sec, (_sec, t) => {
            angle_ = Lerps.Float.easeInOut( 0.0f, 160.0f, t );
            return true;
        } );
    }

    private void OnValidate() {
        setAngle();
    }

    protected void setAngle() {
        hinge_.transform.localRotation = Quaternion.Euler( 0.0f, angle_, 0.0f );
    }

    private void Awake() {
        collider_ = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        setAngle();
    }

    System.Action<Treasurebox> clickCallback_;
    bool bOpen_ = false;
    Collider collider_;
    protected bool bMimic_ = false;
}
