using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLLine : MonoBehaviour {
	// 描画（GLLinesから呼ばれる）
	virtual public bool draw() {
        return true;
    }
}