using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDollDistributor : MonoBehaviour
{
    [SerializeField]
    GameObject waraDollPictPrefab_;

    [SerializeField]
    float width_ = 100;

    [SerializeField]
    float height_ = 100;

    [SerializeField]
    bool bDebug_ = false;

    [SerializeField]
    int debugNum_ = 10;

    [SerializeField]
    TextMesh resultText_;

    System.Action finishCallback_;

    // ばら撒き開始
    public void start( int num, System.Action finishCallback ) {
        counter_ = 0;
        num_ = num;
        finishCallback_ = finishCallback;
        resultText_.gameObject.SetActive( true );
        textStart_ = true;

        if ( num_ == 0 ) {
            finishCallback();
            return;
        }

        float interval = 0.2f;
        for ( int i = 0; i < num; ++i ) {
            var obj = PrefabUtil.createInstance( waraDollPictPrefab_, transform );
            drop( obj, interval * i );
        }
    }

    void drop( GameObject obj, float delay ) {
        obj.SetActive( false );
        Vector3 ep = Randoms.Vec3.valueCenterXY();
        ep.x *= width_;
        ep.y *= height_;
        Vector3 sp = ep;
        sp.z -= 21.0f;
        obj.transform.localPosition = sp;
        float deg = 720.0f * Randoms.Float.valueCenter();
        Quaternion sq = Quaternion.identity;
        Quaternion eq = Quaternion.Euler( 0.0f, 0.0f, deg );

        GlobalState.wait( delay, () => {
            obj.SetActive( true );
            return false;
        } ).nextTime( 0.5f, (sec, t) => {
            obj.transform.localPosition = Lerps.Vec3.easeIn( sp, ep, t );
            obj.transform.localRotation = Quaternion.Lerp( sq, eq, t );
            return true;
        } ).finish( ()=> {
            counter_++;
            if ( counter_ == num_ ) {
                GlobalState.wait( 0.6f, () => {
                    if ( finishCallback_ != null )
                        finishCallback_();
                    return false;
                } );
            }
        } );
    }

    private void Awake() {
        resultText_.gameObject.SetActive( false );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( bDebug_ == true ) {
            bDebug_ = false;
            start( debugNum_, null );
        }

        if ( textStart_ == true ) {
            resultText_.text = string.Format( "{0}人を呪殺！", counter_ );
        }
    }

    int num_ = 0;
    int counter_ = 0;
    bool textStart_ = false;
}
