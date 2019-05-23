using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ロックファイル作成監視
public class CreateLockFileState : GameState {

	// Use this for initialization
	void Start () {
        // ロックファイル保持チェック
        var gm = GameManager.getInstance();
        var inv = gm.getInventory();
        GlobalState.start( () => {
            var list = inv.getItemList();
            foreach ( var item in list ) {
                if ( item.Name == "LockFile" ) {
                    complete();
                    return false;
                }
            }
            return true;
        } );
    }

    // Update is called once per frame
    void Update () {
		
	}
}
