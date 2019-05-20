using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 機密ファイル作成ミッション
public class ConfidentialFileCreateState : GameState {

	// Use this for initialization
	void Start () {
        // メモ発見
        var findMemoState = PrefabUtil.createInstance< FindMemoState >( "Prefabs/FindMemoState", transform );
        findMemoState.CompleteCallback = () => {
            // 機密ファイル保持
            var stockConfidentialFilesState = PrefabUtil.createInstance< StockConfidentialFilesState >( "Prefabs/StockConfidentialFilesState", transform );
            Destroy( findMemoState.gameObject );
            stockConfidentialFilesState.CompleteCallback = () => {
                // 機密ファイル合成フェーズ
                var confidentialFilePazzleState = PrefabUtil.createInstance< ConfidentialFilePazzleState >( "Prefabs/ConfidentialFilePazzleState", transform );
                Destroy( stockConfidentialFilesState.gameObject );
                confidentialFilePazzleState.CompleteCallback = () => {
                    complete();
                    Destroy( confidentialFilePazzleState.gameObject );
                };
            };
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
