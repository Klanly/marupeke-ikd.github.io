using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField]
	GameCore corePrefab_;

	[SerializeField]
	ScoreManager scoreManager_;

	[SerializeField]
	int initLevel_ = 1;

	[SerializeField]
	StageTextEffect stageTextEffect_;

	// セットアップ
	public void setup( GameCore.Param coreParam ) {
		core_ = Instantiate<GameCore>( corePrefab_ );
		core_.setup( coreParam, scoreManager_, stageTextEffect_, initLevel_ );
	}

	// Use this for initialization
	void Start () {
		var coreParam = new GameCore.Param();
		var towerParam = new Tower.Param();
		coreParam.playerParam_.transSec_ = 0.1f;
		setup( coreParam );
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameCore core_;
}
