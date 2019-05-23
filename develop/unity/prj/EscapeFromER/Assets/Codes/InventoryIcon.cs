using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// インベントリー用アイコン
//  表示してボタン押せてアクションする
public class InventoryIcon : MonoBehaviour {

    [SerializeField]
    string name_;

    [SerializeField]
    UnityEngine.UI.Button btn_;

    [SerializeField]
    UnityEngine.UI.Image image_;

    public string Name { get { return name_; } }

    private void Awake() {
    }

    public UnityEngine.UI.Button getButton() {
        return btn_;
    }

    void Start () {
	}
	
	void Update () {		
	}
}
