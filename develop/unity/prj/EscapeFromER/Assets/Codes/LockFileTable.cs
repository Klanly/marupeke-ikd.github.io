using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFileTable : MonoBehaviour {

    [SerializeField]
    List<GimicTableButton> btns_;

    [SerializeField]
    int[] answer_;

    [SerializeField]
    GameObject lockFile_;

    private void Awake() {
        gameObject.SetActive( false );
        lockFile_.SetActive( false );
    }

    // ボタンプッシュ
    void push( string name ) {
        if ( bLockFile_ == true )
            return;

        var dict = new Dictionary<string, int> {
            { "Hart", 1 },
            { "Cross", 2 },
            { "Club", 3 },
            { "Circle", 4 },
            { "Diamond", 5 },
            { "Spade", 6 }
        };
        int curTrueNumber = answer_[ pushCount_ ];
        if ( dict.ContainsKey( name ) == true ) {
            int num = dict[ name ];
            if ( num == curTrueNumber ) {
                pushCount_++;
                if ( pushCount_ >= answer_.Length ) {
                    // 全部正しく押せた
                    showLockFile();
                }
            } else {
                // 間違ったので元に戻す
                pushCount_ = 0;
            }
        }
    }

    // ロックファイル出現
    void showLockFile() {
        Debug.Log( "Show Lock File!" );
        lockFile_.SetActive( true );
        bLockFile_ = true;
    }

    // Use this for initialization
    void Start () {
		foreach ( var b in btns_ ) {
            b.OnClick = (name) => {
                push( name );
            };
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    int pushCount_ = 0; // 正しく押した回数
    bool bLockFile_ = false;
}
