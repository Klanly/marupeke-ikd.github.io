using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ロックパスワード入力
public class LockPasswaordInputState : GameState {

	// Use this for initialization
	void Start () {
        // ディスプレイからのコールバック待ち
        var gm = GameManager.getInstance();
        var display = gm.getDisplay();
        display.LockPasswardCompCallback = () => {
            complete();
        };
    }

    // Update is called once per frame
    void Update () {
		
	}
}
