using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour {

    [SerializeField]
    GameObject body_;

    [SerializeField]
    ParticleSystem particle_;

    // メテオ破壊
    public void destroy() {
        // 親子関係を切って惰性噴出後削除
        transform.parent = null;
        particle_.Stop( true );
        Destroy( body_ );
        Destroy( gameObject, 6.0f );
    }

    // メテオ地上到達
    public void reachToEarth() {
        // 親子関係を切ってパーティクルのみ出し続ける
        transform.parent = null;
        Destroy( body_ );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
