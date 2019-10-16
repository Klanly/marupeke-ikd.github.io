using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フェーダー

class FaderManager {
	static public FaderManager getInstance() {
		return instance_;
	}
	static public Fader Fader {
		get {
			// フェーダが無い場合にGlobalFaderの作成を試みる
			// CommonResourcesがプロジェクトにあればロード可能
			if ( fader_ == null ) {
				fader_ = PrefabUtil.createInstance<Fader>( "GlobalFader" );
			}
			return fader_;
		}
	}
	public void setFader( Fader fader ) {
		fader_ = fader;
	}
	static Fader fader_;
	static FaderManager instance_ = new FaderManager();
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent( typeof(Camera) )]
public class Fader : MonoBehaviour {

	[SerializeField]
	Color initColor_ = Color.black;

	[SerializeField]
	Color fadeVal_;

	[SerializeField, HideInInspector]
	Camera camera_;

	[SerializeField, HideInInspector]
	SpriteRenderer renderer_;

	private void Reset() {
		camera_ = GetComponent<Camera>();
		renderer_ = GetComponent<SpriteRenderer>();

		gameObject.layer = 31;

		camera_.orthographic = true;
		camera_.clearFlags = CameraClearFlags.Nothing;
		camera_.backgroundColor = Color.black;
		camera_.cullingMask = 1 << gameObject.layer;
		camera_.depth = 100;
		camera_.nearClipPlane = 0.0f;
		camera_.farClipPlane = 10.0f;
		camera_.orthographicSize = 0.1f;

		// スプライト
		Texture2D tex = new Texture2D( 64, 64,TextureFormat.RGBA32, false );
		Color[] colors = tex.GetPixels();
		for ( int i = 0; i < colors.Length; i++ ) {
			colors[ i ] = Color.white;
			colors[ i ].a = 1.0f;
		}
		tex.SetPixels( colors );
		Sprite sprite = Sprite.Create( tex, new Rect( 0.0f, 0.0f, 64.0f, 64.0f ), new Vector3( 0.5f, 0.5f, 0.0f ) );
		renderer_.sprite = sprite;
		var c = renderer_.color;
		c.a = 0.0f;
		renderer_.color = c;
	}

	private void Awake() {
		FaderManager.getInstance().setFader( this );
		renderer_.color = initColor_;
		rate_.setAim( initColor_, 0.0f );
	}

	// フェード先を設定
	public void to( Color color, float fadeSec, System.Action finishCallback = null ) {
		rate_.setSec( fadeSec );
		rate_.setAim( color );
		if ( finishCallback  != null ) {
			GlobalState.time( fadeSec, (sec, t) => {
				return true;
			} ).finish( finishCallback );
		}
	}

	// フェード先を設定（アルファ値のみ変更）
	public void to(float alpha, float fadeSec, System.Action finishCallback = null) {
		var color = rate_.getAim();
		color.a = alpha;
		to( color, fadeSec, finishCallback );
	}

	// カラーを変更（フェーダーが動いていない時のみ有効）
	public void setColor( Color color, float alpha = -1.0f ) {
		if ( alpha >= 0.0f ) {
			color.a = alpha;
		}
		rate_.setAim( color, 0.0f );
	}

	void Update () {
		fadeVal_ = rate_.getCurVal();
		if ( rate_.getCurVal().a < 0.001f ) {
			// 透明とみなしカメラを切る
			camera_.enabled = false;
		} else {
			camera_.enabled = true;
			camera_.gameObject.SetActive( true );
			renderer_.color = fadeVal_;
		}
	}

	MoveValueColor rate_ = new MoveValueColor( Color.white, 1.0f );
}
