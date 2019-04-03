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

	public void setColors( Color nearColor, Color farColor ) {
		var mat = renderer_.material;
		mat.SetColor( "_NearColor", nearColor );
		mat.SetColor( "DeepColor", farColor );
		renderer_.material = mat;
	}

	public void setColorTransparentRate( float rate ) {
		var mat = renderer_.material;
		mat.SetFloat( "_ColorTransRate", rate );
		renderer_.material = mat;
	}

	public void setDiffuseRange( float range ) {
		var mat = renderer_.material;
		mat.SetFloat( "_DiffuseRange", range );
		renderer_.material = mat;
	}

	public void setAmplitudeRate( float amp ) {
		var mat = renderer_.material;
		mat.SetFloat( "_AmpRate", amp );
		renderer_.material = mat;
	}

	public void setHeightTextureScales( float[] scales ) {
		var mat = renderer_.material;
		mat.SetFloat( "_HeightTexScale0", scales[ 0 ] );
		mat.SetFloat( "_HeightTexScale1", scales[ 1 ] );
		mat.SetFloat( "_HeightTexScale2", scales[ 2 ] );
		renderer_.material = mat;
	}

	public void setVertexWaveLengthes( float[] lengthes ) {
		var mat = renderer_.material;
		mat.SetVector( "_VertexWaveLength0_3", new Vector4( lengthes[ 0 ], lengthes[ 1 ], lengthes[ 2 ], lengthes[ 3 ] ) );
		mat.SetVector( "_VertexWaveLength4_7", new Vector4( lengthes[ 4 ], lengthes[ 5 ], lengthes[ 6 ], lengthes[ 7 ] ) );
		renderer_.material = mat;
	}

	public void setEnvironmentalColorRate( float envColorRate ) {
		var mat = renderer_.material;
		mat.SetFloat( "_EnvRate", envColorRate );
		renderer_.material = mat;
	}

	public void setSkybox( Cubemap cubemap ) {
		var mat = renderer_.material;
		mat.SetTexture( "_EnvTex", cubemap );
		renderer_.material = mat;
	}

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
