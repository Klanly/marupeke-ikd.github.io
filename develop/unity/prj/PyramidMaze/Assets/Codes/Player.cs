using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    PlayerUpDown upDown_;

    [SerializeField]
    MazeMesh mezeMesh_;

    // 落下
    void fall( System.Action finishCallback ) {
        // 足元に床が無い場合に落下成立
        var mazeCollider = mezeMesh_.getCollider();
        var ray = new Ray( transform.position, Vector3.down );
        RaycastHit hit;
        if ( mazeCollider.Raycast( ray, out hit, 0.7f ) == false ) {
            // 落下
            Debug.Log( "Faaaal!!" );
            float totalTime = LerpAction.jumpDown( 9.8f, 0.15f, 0.05f, -1.0f, 0.1f, 0.05f, 0.0f, true );
            var curPos = transform.localPosition;
            var def = Vector3.zero;
            GlobalState.time( totalTime, (sec, t) => {
                def.y = LerpAction.jumpDown( 9.8f, 0.15f, 0.05f, -1.0f, 0.1f, 0.05f, sec );
                transform.localPosition = curPos + def;
                return true;
            } ).finish( () => {
                finishCallback();
            } );
        } else {
            finishCallback();
        }
    }

    void jump(System.Action finishCallback) {
        // 上に天井が無い場合に上昇成立
        var mazeCollider = mezeMesh_.getCollider();
        var ray = new Ray( transform.position, Vector3.up );
        RaycastHit hit;
        if ( mazeCollider.Raycast( ray, out hit, 0.7f ) == false ) {
            // 上昇
            Debug.Log( "Riseeeee!!" );
            float totalTime = LerpAction.jump( 9.8f, 0.15f, 0.05f, 1.0f, 0.1f, 0.05f, 0.0f, true );
            var curPos = transform.localPosition;
            var def = Vector3.zero;
            GlobalState.time( totalTime, (sec, t) => {
                def.y = LerpAction.jump( 9.8f, 0.15f, 0.05f, 1.0f, 0.1f, 0.05f, sec );
                transform.localPosition = curPos + def;
                return true;
            } ).finish(() => {
                finishCallback();
            } );
        } else {
            finishCallback();
        }
    }

    // Use this for initialization
    void Start () {
        upDown_.FallCallback = fall;
        upDown_.JumpCallback = jump;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
