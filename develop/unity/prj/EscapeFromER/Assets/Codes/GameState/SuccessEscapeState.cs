using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessEscapeState : GameState {

	// Use this for initialization
	void Start () {
        Debug.Log( "Success Escape!" );
        var gm = GameManager.getInstance();
        var room = gm.getRoom();
        room.deactivate( () => {
            GameObjectUtil.remove( new string[] { "BookShelf", "Table", "LockFileTable", "Memo" } );
        } );

        // メインカメラをforwardと逆方向へ移動
        var forward = Camera.main.transform.forward;
        Camera camera = Camera.main;
        GlobalState.start( () => {
            if ( this == null )
                return false;
            var p = camera.transform.position;
            p -= forward * 0.1f;
            camera.transform.position = p;
            return true;
        } );

        // [ESCAPE SUCCESS]を表示
    }

    // Update is called once per frame
    void Update () {
		
	}
}
