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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
