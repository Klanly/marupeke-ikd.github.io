using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

    [SerializeField]
    Light light_;

    [SerializeField]
    float speed_;

    private void Awake() {
        initIntensity_ = light_.intensity;
        for ( int i = 0; i < ts_.Length; ++i ) {
            ts_[ i ] = Random.value * 256.0f;
        }
    }

    float calcNoiseValue( ref float t, float times ) {
        // ライト強度を揺らして炎っぽく
        t += Time.deltaTime * speed_ * times;
        t = Mathf.Repeat( t, 256.0f );
        return Mathf.PerlinNoise( t, 0.0f );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float power = 0.0f;
        for ( int i = 0; i < ts_.Length; ++i ) {
            float times = 1 << i;
            power += calcNoiseValue( ref ts_[ i ], 1 << i ) / times;
        }
        light_.intensity = initIntensity_ + 1.0f * ( power - 0.5f );
	}
    float initIntensity_ = 1.0f;
    float[] ts_ = new float[ 4 ];
}
