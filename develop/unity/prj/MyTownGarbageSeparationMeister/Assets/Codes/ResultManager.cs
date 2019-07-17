using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    GameObject finish_;

    [SerializeField]
    GameObject result_;

    [SerializeField]
    TextMesh good_;

    [SerializeField]
    TextMesh no_;

    [SerializeField]
    GameObject[] ranks_;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_ = null;

    public void setup( int goodNum, int noNum ) {
        goodNum_ = goodNum;
        noNum_ = noNum;
    }

    private void Awake() {
        finish_.SetActive( false );
        result_.SetActive( false );
        good_.gameObject.SetActive( false );
        no_.gameObject.SetActive( false );
        foreach ( var r in ranks_ ) {
            r.gameObject.SetActive( false );
        }
    }

    void Start()
    {
        good_.text = string.Format( "正解：{0}", goodNum_ );
        no_.text = string.Format( "不正解：{0}", noNum_ );
        GlobalState.start( () => {
            finish_.SetActive( true );
            return false;
        } ).wait( 3.0f )
        .oneFrame(()=> {
            finish_.SetActive( false );
        } ).wait( 1.0f )
        .oneFrame( () => {
            result_.SetActive( true );
        } ).wait( 1.2f )
        .oneFrame( () => {
            good_.gameObject.SetActive( true );
        } ).wait( 1.2f )
        .oneFrame( ()=> {
            no_.gameObject.SetActive( true );
        } ).wait( 1.5f )
        .oneFrame( () => {
            float rate = ( float )goodNum_ / ( goodNum_ + noNum_ );
            if ( rate >= 0.99f ) {
                ranks_[ 0 ].SetActive( true );
            } else if ( rate >= 0.80f ) {
                ranks_[ 1 ].SetActive( true );
            } else if ( rate >= 0.70f ) {
                ranks_[ 2 ].SetActive( true );
            } else if ( rate >= 0.50f ) {
                ranks_[ 3 ].SetActive( true );
            } else if ( rate >= 0.30f ) {
                ranks_[ 4 ].SetActive( true );
            } else {
                ranks_[ 5 ].SetActive( true );
            }
        } ). wait( 3.0f )
        .finish( () => {
            if ( finishCallback_ != null )
                finishCallback_();
        } );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int goodNum_ = 2;
    int noNum_ = 8;
}
