using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningState : GameState {

    // Use this for initialization
    void Start() {
        // ワーニング発動
        Debug.Log( "Start Warning!" );
        var gm = GameManager.getInstance();
        var room = gm.getRoom();
        room.activeWarning( true );

        // ロックファイル作成台出現
        var table = GameObjectUtil.find<LockFileTable>( "LockFileTable", false );
        table.gameObject.SetActive( true );

        // ロックファイル作成
        var createLockFile = PrefabUtil.createInstance<CreateLockFileState>( "Prefabs/CreateLockFileState", transform );

        createLockFile.CompleteCallback = () => {
            // ディスプレイをONにし
            // ロックパスワード入力モードに
            var display = gm.getDisplay();
            display.showLockPassward();

            // ロックパスワード入力
            var lockPasswordInput = PrefabUtil.createInstance<LockPasswaordInputState>( "Prefabs/LockPasswaordInputState", transform );
            Destroy( createLockFile.gameObject );
            lockPasswordInput.CompleteCallback = () => {
                Destroy( lockPasswordInput.gameObject );
                room.activeWarning( false );
                complete();
            };
        };

        
        // TODO
        // createLockFile.forceComplete();

    }

    // Update is called once per frame
    void Update () {
		
	}
}
