﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    PlayerUpDown upDown_;

    [SerializeField]
    MazeMesh mezeMesh_;

    [SerializeField]
    Light torch_;

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
            noUpDown();
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
            noUpDown();
        }
    }

    void noUpDown() {
        // 松明を追加
        var ray = Camera.main.ScreenPointToRay( new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f ) );
        RaycastHit hit;
        if ( Physics.Raycast( ray, out hit, 0.7f ) == true ) {
            var mazeMesh = hit.collider.GetComponent<MazeMesh>();
            if ( mazeMesh != null ) {
                // 松明を付ける
                var p = hit.point + hit.normal * 0.05f;
                var torch = Instantiate<Light>( torch_ );
                torch.transform.localPosition = p;
                torch.gameObject.SetActive( true );
            } else {
                var torch = hit.collider.GetComponent<Torch>();
                if ( torch != null ) {
                    // 松明を消す
                    Destroy( torch.gameObject );
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        upDown_.FallCallback = fall;
        upDown_.JumpCallback = jump;
        upDown_.NoUpDownCallback = noUpDown;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
