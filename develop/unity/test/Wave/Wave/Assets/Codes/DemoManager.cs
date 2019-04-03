using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour {

	[SerializeField]
	Water water_;

	[SerializeField]
	Light light_;

	[SerializeField]
	Transform plate_;

	[SerializeField]
	float cameraYRotSpeed_ = 0.3f;

	[SerializeField]
	List<Cubemap> cubemaps_ = new List<Cubemap>();

	[SerializeField]
	int curSceneIdx_ = 0;

	[SerializeField]
	Material skyboxMaterial_;


	class Param {
		public Color nearColor;
		public Color farColor;
		public float colorTransparentRate = 0.12f;
		public float diffuseRange = 0.0f;
		public float amplitudeRate = 0.12f;
		public float[] heightTextureScales = new float[] { 1.0f, 2.0f, 4.0f };
		public float[] vertexWaveLengthes = new float[] {1, 1, 2, 2, 4, 4, 8, 8 };
		public Vector3 directionalLightRot = new Vector3( 0.0f, 0.0f, 0.0f );
		public int cubeMapIdx = 0;
		public float environmentalColorRate = 0.12f;
		public Vector3 cameraPos = new Vector3( -3.13f, 0.54f, -3.27f );
		public float cameraXRot = 28.13f;
		public float plateScale = 1.0f;
	}

	Color convColor( uint v ) {
		return new Color(
			( ( v >> 24 ) & 0xff ) / 255.0f,
			( ( v >> 16 ) & 0xff ) / 255.0f,
			( ( v >> 8 ) & 0xff ) / 255.0f,
			( ( v >> 0 ) & 0xff ) / 255.0f
		);
	}
	// Use this for initialization
	void Start () {
		camera_ = Camera.main;
		state_ = new SetParam( this );

		// シーンパラメータ群
		{
			var param = new Param();
			param.nearColor = convColor( 0x3147D8FF );
			param.farColor = convColor( 0x00435EFF );
			param.colorTransparentRate = 0.12f;
			param.diffuseRange = 0.0f;
			param.amplitudeRate = 0.12f;
			param.heightTextureScales = new float[] { 1.0f, 2.0f, 2.0f };
			param.vertexWaveLengthes = new float[] { 1.0f, 1.0f, 2.0f, 2.0f, 4.0f, 4.0f, 8.0f, 8.0f };
			param.directionalLightRot = new Vector3( 9.1f, -24.83f, 0.0f );
			param.cubeMapIdx = 0;
			param.environmentalColorRate = 0.3f;
			param.cameraPos = new Vector3( -3.13f, 0.54f, -3.27f );
			param.cameraXRot = 28.13f;
			param.plateScale = 1.0f;
			params_.Add( param );
		}
		{
			var param = new Param();
			param.nearColor = convColor( 0x00FFFFFF );
			param.farColor = convColor( 0xFFFFFFFF );
			param.colorTransparentRate = 0.0f;
			param.diffuseRange = 0.4f;
			param.amplitudeRate = 0.05f;
			param.heightTextureScales = new float[] { 2.0f, 12.0f, 24.0f };
			param.vertexWaveLengthes = new float[] { 1.0f, 1.0f, 2.0f, 2.0f, 2.0f, 4.0f, 4.0f, 4.0f };
			param.directionalLightRot = new Vector3( 35.0f, -24.83f, 0.0f );
			param.cubeMapIdx = 0;
			param.environmentalColorRate = 0.3f;
			param.cameraPos = new Vector3( -3.13f, 0.54f, -3.27f );
			param.cameraXRot = 28.13f;
			param.plateScale = 1.0f;
			params_.Add( param );
		}
		{
			var param = new Param();
			param.nearColor = convColor( 0x3147D8FF );
			param.farColor = convColor( 0x00435EFF );
			param.colorTransparentRate = 0.12f;
			param.diffuseRange = 0.0f;
			param.amplitudeRate = 0.12f;
			param.heightTextureScales = new float[] { 1.0f, 2.0f, 2.0f };
			param.vertexWaveLengthes = new float[] { 1.0f, 1.0f, 2.0f, 2.0f, 4.0f, 4.0f, 8.0f, 8.0f };
			param.directionalLightRot = new Vector3( 2.0f, -24.83f, 0.0f );
			param.cubeMapIdx = 1;
			param.environmentalColorRate = 0.3f;
			param.cameraPos = new Vector3( -3.13f, 29.0f, -3.27f );
			param.cameraXRot = 13.2f;
			param.plateScale = 50.0f;
			params_.Add( param );
		}
		{
			var param = new Param();
			param.nearColor = convColor( 0x004CFFFF );
			param.farColor = convColor( 0x000000FF );
			param.colorTransparentRate = 0.109f;
			param.diffuseRange = 0.4f;
			param.amplitudeRate = 0.05f;
			param.heightTextureScales = new float[] { 2.0f, 12.0f, 24.0f };
			param.vertexWaveLengthes = new float[] { 1.0f, 1.0f, 2.0f, 2.0f, 2.0f, 4.0f, 4.0f, 4.0f };
			param.directionalLightRot = new Vector3( 35.0f, -24.83f, 0.0f );
			param.cubeMapIdx = 2;
			param.environmentalColorRate = 0.12f;
			param.cameraPos = new Vector3( -3.13f, 29.0f, -3.27f );
			param.cameraXRot = 13.2f;
			param.plateScale = 50.0f;
			params_.Add( param );
		}
	}

	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();

		var rot = camera_.transform.rotation;
		camera_.transform.rotation = Quaternion.Euler( 0.0f, cameraYRotSpeed_ * Time.deltaTime, 0.0f ) * rot;
	}

	class SetParam : State< DemoManager > {
		public SetParam( DemoManager parent ) : base( parent ) {
		}
		protected override State innerInit() {
			var param = parent_.params_[ parent_.curSceneIdx_ % parent_.params_.Count ];
			parent_.water_.setColors( param.nearColor, param.farColor );
			parent_.water_.setColorTransparentRate( param.colorTransparentRate );
			parent_.water_.setDiffuseRange( param.diffuseRange );
			parent_.water_.setAmplitudeRate( param.amplitudeRate );
			parent_.water_.setHeightTextureScales( param.heightTextureScales );
			parent_.water_.setVertexWaveLengthes( param.vertexWaveLengthes );
			parent_.light_.transform.rotation = Quaternion.Euler( param.directionalLightRot );
			parent_.water_.setEnvironmentalColorRate( param.environmentalColorRate );

			parent_.water_.setSkybox( parent_.cubemaps_[ param.cubeMapIdx ] );
			var skyboxMat = new Material( parent_.skyboxMaterial_ );
			if ( parent_.skyboxMaterial_ != null ) {
				int skyboxID = Shader.PropertyToID( "_Tex" );
				skyboxMat.SetTexture( skyboxID, parent_.cubemaps_[ param.cubeMapIdx ] );
				RenderSettings.skybox = skyboxMat;
			}

			parent_.camera_.transform.position = param.cameraPos;
			var cameraRot = parent_.camera_.transform.rotation.eulerAngles;
			cameraRot.x = param.cameraXRot;
			parent_.camera_.transform.rotation = Quaternion.Euler( cameraRot );

			parent_.plate_.localScale = Vector3.one * param.plateScale;

			return new FadeIn( parent_ );
		}
	}

	class FadeIn : State< DemoManager > {
		public FadeIn( DemoManager parent ) : base( parent ) {
		}
		protected override State innerInit() {
			FaderManager.Fader.to( 0.0f, 3.0f, () => {
				setNextState( new Idle( parent_ ) );
			} );
			return this;
		}
	}

	class Idle : State< DemoManager > {
		public Idle( DemoManager parent ) : base( parent ) { }
		protected override State innerInit() {
			GlobalState.time( 10.0f, (sec, t) => {
				if ( Input.GetMouseButtonDown( 0 ) == true )
					return false;
				return true;
			} ).finish(()=> {
				setNextState( new FadeOut( parent_ ) );
			} );
			return this;
		}
	}

	class FadeOut : State< DemoManager > {
		public FadeOut(DemoManager parent) : base( parent ) { }
		protected override State innerInit() {
			FaderManager.Fader.to( 1.0f, 3.0f, () => {
				parent_.curSceneIdx_++;
				setNextState( new SetParam( parent_ ) );
			} );
			return this;
		}
	}

	State state_;
	Camera camera_;
	List<Param> params_ = new List<Param>();
}
