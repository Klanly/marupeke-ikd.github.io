using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 機密ファイル作成ミッション
public class ConfidentialFileCreateState : GameState {

	// Use this for initialization
	void Start () {
        // メモ発見
        var findMemoState = PrefabUtil.createInstance< FindMemoState >( "FindMemoState" );
        findMemoState.CompleteCallback = () => {
            // 機密ファイル保持
            var stockConfidentialFilesState = PrefabUtil.createInstance< StockConfidentialFilesState >( "StockConfidentialFilesState", transform );
            Destroy( findMemoState );
            stockConfidentialFilesState.CompleteCallback = () => {
                // 機密ファイル合成フェーズ
                var confidentialFilePazzleState = PrefabUtil.createInstance< ConfidentialFilePazzleState >( "ConfidentialFilePazzleState", transform );
                Destroy( stockConfidentialFilesState );
                confidentialFilePazzleState.CompleteCallback = () => {
                    complete();
                    Destroy( confidentialFilePazzleState );
                };
            };
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
