using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ディスプレイ
public class Display : MonoBehaviour {

    [SerializeField]
    List<MonitorButton> numButtons_;

    [SerializeField]
    GameObject buttonRoot_;

    [SerializeField]
    string correctLockPass_;

    [SerializeField]
    TextMesh lockPassText_;

    [SerializeField]
    TextMesh lockPassDeactivateText_;

    public System.Action LockPasswardCompCallback { set { lockPasswardCompCallback_ = value; } }
    System.Action lockPasswardCompCallback_;

    // ロックパスワード入力画面にする
    public void showLockPassward() {
        buttonRoot_.SetActive( true );
        foreach ( var nb in numButtons_ ) {
            nb.OnClick = (str) => {
                addLockPassward( str );
            };
        }
    }

    // ロックパスワード追加
    void addLockPassward( string s ) {
        curLockPass_ += s;
        if ( curLockPass_ == correctLockPass_ ) {
            if ( lockPasswardCompCallback_ != null )
                lockPasswardCompCallback_();
            foreach ( var nb in numButtons_ ) {
                nb.OnClick = null;
            }
            lockPassDeactivateText_.gameObject.SetActive( true );
        }
        if ( curLockPass_.Length >= correctLockPass_.Length ) {
            // 失敗やり直し
            curLockPass_ = "";
        }

        // 表示
        lockPassText_.text = curLockPass_;
    }

    private void Awake() {
        buttonRoot_.SetActive( false );
        lockPassText_.text = "";
        lockPassDeactivateText_.gameObject.SetActive( false );
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    string curLockPass_ = "";
}
