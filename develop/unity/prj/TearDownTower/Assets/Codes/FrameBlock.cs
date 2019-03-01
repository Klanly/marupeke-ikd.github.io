using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameBlock : MonoBehaviour {

	[SerializeField]
	MeshRenderer renderer_;

	[SerializeField]
	float blockHeight_;

	[SerializeField]
	float blockWidth_;

	public void setFrameColor(Color color) {
		frameColor_ = color;
		var mat = renderer_.materials[ 1 ];
		mat.SetColor( "_EmissionColor", frameColor_ );
		mat.color = frameColor_;
		renderer_.materials[ 1 ] = mat;
	}

	public Color getFrameColor() {
		return frameColor_;
	}

	public Color getBodyColor() {
		return bodyColor_;
	}

	public void setBodyColor( Color color ) {
		bodyColor_ = color;
		var mat = renderer_.materials[ 0 ];
		mat.color = bodyColor_ * 0.75f;
		renderer_.materials[ 0 ] = mat;
	}

	public void setGroupIdx( int idx ) {
		index_ = idx;
	}

	public int getGroupIdx() {
		return index_;
	}

	public float getHeight() {
		return blockHeight_;
	}

	public float getWidth() {
		return blockWidth_;
	}

	public void setTopMark() {
		bTop_ = true;
	}

	public bool isTop() {
		return bTop_;
	}

	// 1段上げる
	public void moveUp() {
		DeltaLerp.Float.easeOutOver( getHeight(), 0.35f, 0.25f, (_sec, _t, _dt, _delta) => {
			if ( this == null )
				return false;
			var pos = transform.localPosition;
			pos.y += _delta;
			transform.localPosition = pos;
			return true;
		} );
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	int index_ = -1;
	Color frameColor_ = Color.white;
	Color bodyColor_ = Color.black;
	bool bTop_ = false;
}
