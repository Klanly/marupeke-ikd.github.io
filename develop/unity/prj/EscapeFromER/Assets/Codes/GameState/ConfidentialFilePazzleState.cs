using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 機密ファイルパズルステート
public class ConfidentialFilePazzleState : GameState {

	// Use this for initialization
	void Start () {
        var inv = GameManager.getInstance().getInventory();
        GlobalState.start( () => {
            var list = inv.getItemList();
            foreach ( var item in list ) {
                if ( item.Name == "ConfFile" ) {
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
