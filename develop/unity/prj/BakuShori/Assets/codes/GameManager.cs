using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム管理人
public class GameManager : MonoBehaviour {

    [SerializeField]
    GimicLayoutGenerator generator_ = null;

	// Use this for initialization
	void Start () {
        BombBox bombBox;
        LayoutSpec spec = new LayoutSpec();
        generator_.create( spec, out bombBox );
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
