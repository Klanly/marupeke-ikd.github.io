using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メモを発見する
public class FindMemoState : GameState {

	// Use this for initialization
	void Start () {
        // インベントリーにメモが格納されたらOK
        var gm = GameManager.getInstance();
        var inv = gm.getInventory();
        GlobalState.start( () => {
            var list = inv.getItemList();
            foreach ( var item in list ) {
                if ( item.Name == "Memo" ) {
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
