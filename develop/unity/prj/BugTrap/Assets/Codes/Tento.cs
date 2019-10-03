using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tento : Bug
{
	private void Awake()
	{
		state_ = new StartIdle( this );
	}

	void Update()
	{
		updateEntry();
	}
}
