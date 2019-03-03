﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	GameCore corePrefab_;

	[SerializeField]
	ScoreManager scoreManager_;

	// セットアップ
	public void setup( GameCore.Param coreParam ) {
		core_ = Instantiate<GameCore>( corePrefab_ );
		core_.setup( coreParam, scoreManager_ );
	}

	// Use this for initialization
	void Start () {
		var coreParam = new GameCore.Param();
		var towerParam = new Tower.Param();
		coreParam.playerParam_.transSec_ = 0.1f;
		coreParam.towerParam_ = towerParam;
		setup( coreParam );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameCore core_;
}
