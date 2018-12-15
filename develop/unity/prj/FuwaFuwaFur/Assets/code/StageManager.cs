using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    [SerializeField]
    FurModel furModel_;

    [SerializeField]
    Bee beePrefab_;

    [SerializeField]
    Camera mainCamera_;


    public void addBlowDirect( Vector3 direction )
    {
        furModel_.addBlow( direction );
    }

    // Use this for initialization
    void Start () {
        beeFactory_.beePrefab = beePrefab_;

        float mesureUnit = 0.001f;  // ワールドの1ユニット1mm

        // 5m位から100mまで、幅-2mから2mの範囲に適当にハチを散りばめてみる
        float lowHeightM = 0.0f;
        float hiHeightM = 100.0f;
        float leftM = -0.2f;
        float rightM = 0.2f;
        float density = 50.0f; // 密度。1m^2当たりの蜂の数
        int beeNum = ( int )( ( hiHeightM - lowHeightM ) * ( rightM - leftM ) * density );
        for ( int i = 0; i < beeNum; ++i ) {
            var bee = beeFactory_.create( Vector3.zero, 3.0f, 8.0f, 40.0f, 20.0f, 30.0f );
            bee.transform.localPosition = new Vector3( Random.Range( leftM, rightM ) / mesureUnit, Random.Range( lowHeightM, hiHeightM ) / mesureUnit, 0.0f );
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    BeeFactory beeFactory_ = new BeeFactory();

}
