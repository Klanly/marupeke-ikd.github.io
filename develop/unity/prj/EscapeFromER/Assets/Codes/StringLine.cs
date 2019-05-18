using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringLine : MonoBehaviour {

    [SerializeField]
    TextMesh text_;

    [SerializeField]
    int strLenMin_ = 1;

    [SerializeField]
    int strLenMax_ = 16;

    [SerializeField]
    float speed_ = 10.0f;

    [SerializeField]
    float maxLifeTime_ = 20.0f;

    public Vector3 Pos { set { initPos_ = value; } }
    public Vector3 Dir { set { dir_ = value.normalized * speed_; } }
    public float LifeTime { set { lifeTime_ = value; } }

    Vector3 initPos_ = Vector3.zero;
    Vector3 dir_ = new Vector3( 1.0f / 60.0f, 0.0f, 0.0f );
    float lifeTime_ = 10.0f;

    private void Awake() {
        // 適当な01を並べる
        int bitLen = Random.Range( strLenMin_, strLenMax_ + 1 ) * 8;
        string text = "";
        for ( int i = 0; i < bitLen; ++i ) {
            text += ( Random.Range( 0, 2 ) % 2 == 0 ? "0" : "1" );
        }
        text_.text = text;
    }

    // Use this for initialization
    void Start () {
        dir_ = dir_.normalized * speed_;
        transform.localRotation = Quaternion.LookRotation( dir_ );
        lifeTime_ = Random.Range( maxLifeTime_ * 0.5f, maxLifeTime_ );
        transform.localPosition = initPos_;
        t_ = 0.0f;
    }

    // Update is called once per frame
    void Update () {
        t_ += Time.deltaTime;
        if ( t_ >= lifeTime_ ) {
            lifeTime_ = Random.Range( maxLifeTime_ * 0.5f, maxLifeTime_ );
            transform.localPosition = initPos_;
            t_ = 0.0f;
        }

        // 移動
        var p = transform.localPosition;
        p += dir_;
        transform.localPosition = p;

    }
    float t_ = 0.0f;
}
