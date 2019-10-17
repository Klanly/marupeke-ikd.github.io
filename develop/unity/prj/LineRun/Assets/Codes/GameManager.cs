using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase
{
	[SerializeField]
	SpriteRenderer renderer_;

	public DrawableTexture getLineRecordTexture()
	{
		return dt_;
	}

	// Start is called before the first frame update
	private void Awake()
	{
		dt_.setup( 64, 64, Color.white );
		renderer_.sprite = dt_.getSprite();
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		stateUpdate();        
    }

	DrawableTexture dt_ = new DrawableTexture();
}
