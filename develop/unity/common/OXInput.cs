using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OXInput : IOXInput {
	// 決定ボタン押した？
	public override bool decide()
	{
		return Input.GetKey( KeyCode.Z );
	}
}
