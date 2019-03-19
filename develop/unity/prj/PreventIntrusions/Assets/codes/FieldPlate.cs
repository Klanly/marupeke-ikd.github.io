using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPlate : MonoBehaviour {

	[SerializeField]
	Material[] fieldMaterials_;

	[SerializeField]
	MeshRenderer mesh_;

	public enum FieldType {
		Conclete
	}

	public void setup( FieldType fieldType, int index ) {
		switch ( fieldType ) {
			case FieldType.Conclete:
				mesh_.material = fieldMaterials_[ index ];
				break;
		}
	}

	// 色を設定
	public void setColor( Color color ) {
		var mat = mesh_.material;
		mat.color = color;
		mesh_.material = mat;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
