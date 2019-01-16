using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour {

    [SerializeField]
    protected float speed_ = 0.0f;


    public enum Type {
        Human_Walk,
        Human_Run,
        Ship
    }

    public System.Action<int> OnMiss { set { onMiss_ = value;  } }

    // 初期化
    public void setup( GameManager manager )
    {
        manager_ = manager;
    }

    // スピード設定
    public void setSpeed( float speed )
    {
        speed_ = speed;
    }

    // スピード取得
    public float getSpeed()
    {
        return speed_;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected GameManager manager_;
    protected System.Action<int> onMiss_;
}
