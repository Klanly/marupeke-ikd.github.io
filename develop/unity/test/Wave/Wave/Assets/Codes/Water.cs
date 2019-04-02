using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	[SerializeField]
	MeshRenderer renderer_;

	[SerializeField]
	Vector2 offsetSpeed0_;

	[SerializeField]
	Vector2 offsetSpeed1_;

	[SerializeField]
	Vector2 offsetSpeed2_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		t_ += Time.deltaTime;
		var mat = renderer_.material;
		mat.SetTextureOffset( "_HeightTex0", offsetSpeed0_ * t_ );

		mat.SetTextureOffset( "_HeightTex1", offsetSpeed1_ * t_ );

		mat.SetTextureOffset( "_HeightTex2", offsetSpeed2_ * t_ );

		mat.SetFloat( "_t", t_ );
	}

	float t_ = 0.0f;
}
