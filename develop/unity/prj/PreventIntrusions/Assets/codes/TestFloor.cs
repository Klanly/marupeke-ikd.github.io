using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloor : MonoBehaviour {

	[SerializeField]
	MeshRenderer renderer_;

	public void setColor( Color color ) {
		var mat = renderer_.material;
		mat.color = color;
		renderer_.material = mat;
	}
}
