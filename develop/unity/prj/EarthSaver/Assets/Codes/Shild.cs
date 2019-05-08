using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour {

    [SerializeField]
    GameObject model_;

    [SerializeField]
    float rotSpeed_;

    [SerializeField]
    GameObject explosionPt_;

    [SerializeField]
    float avePower_ = 100.0f;  // 落下物に与えるパワーの平均値

    [SerializeField]
    float varPower_ = 10.0f;   // 落下物に与えるパワーの分散

    public float Power { get { return power_; } }

    public void explosion() {
        explosionPt_.gameObject.SetActive( true );
        Destroy( model_, 0.15f );
        Destroy( gameObject, 4.0f );
    }

    private void Awake() {
        power_ = avePower_ + varPower_ * Randoms.Float.valueCenter();        
    }

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if ( model_ == null )
            return;
        var q = model_.transform.localRotation;
        model_.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, Time.deltaTime * rotSpeed_ ) * q;
    }

    float power_ = 0.0f;
}
