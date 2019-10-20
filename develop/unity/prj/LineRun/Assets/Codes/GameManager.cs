using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase
{
	public DrawableTexture getLineRecordTexture()
	{
		return dt_;
	}

	// Start is called before the first frame update
	private void Awake()
	{
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
