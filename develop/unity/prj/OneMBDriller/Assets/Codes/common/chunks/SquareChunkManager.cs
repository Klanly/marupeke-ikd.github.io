using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 正方形座標チャンク管理者
//
//  一定サイズの正方形をチャンク図形とするチャンク管理。
//  整数座標をIDとする
public class SquareChunkManager : ChunkManager<Vector2Int>
{
	public enum PlaneType
	{
		XY,
		XZ,
		YZ
	}

	// セットアップ
	//  設定後直ちにChangeChunkCallbackに最初のアクティブ情報が返ります。
	//  chunkSize      : 正方形チャンクの辺の長さ
	//  activeLayerNum : ターゲットがいるチャンクを中心とした時のアクティブ化するチャンクの層数
	//  planeType      : 平面指定
	//  idx0ChunkPos   : (0, 0, 0)に該当するチャンクの左下座標
	//  initTargetPos  : 初期ターゲットポジション
	public void setup(
		float chunkSize,
		int activeLayerNum,
		PlaneType planeType,
		Vector3 idx0ChunkPos,
		Vector3 initTargetPos
	) {
		size_ = chunkSize;
		offset_ = idx0ChunkPos;
		activeLayerNum = ( activeLayerNum <= 0 ? 1 : activeLayerNum );
		switch (planeType) {
		case PlaneType.XY:
			calcPlaneChunkId_ = calcPlaneXYChunkId;
			break;
		case PlaneType.XZ:
			calcPlaneChunkId_ = calcPlaneXZChunkId;
			break;
		case PlaneType.YZ:
			calcPlaneChunkId_ = calcPlaneYZChunkId;
			break;
		}
		initChunkCallback( initTargetPos );
		bInitialized_ = true;
	}

	void initChunkCallback( Vector3 pos ) {
		if (changeChunkCallback_ != null) {
			Vector2Int id = calcChunkId( pos );
			var activeIds = new List<Vector2Int>();
			for (int y = id.y - layerNum_; y <= id.y + layerNum_; ++y) {
				for (int x = id.x - layerNum_; x <= id.x + layerNum_; ++x) {
					activeIds.Add( new Vector2Int( x, y ) );
				}
			}
			preChunkId_ = id;
			preActiveIds_ = activeIds;

			// 初期コールバック
			changeChunkCallback_( activeIds, new List<Vector2Int>() );
			bCalledInitCallback_ = true;
		}
	}


	// チャンクの状態を更新
	//  pos: ターゲットの位置
	public override void updateChunk( Vector3 pos ) {
		if (bInitialized_== false) {
			return;
		}
		if ( bCalledInitCallback_ == false ) {
			initChunkCallback( pos );
		} else if ( changeChunkCallback_ != null ) {
			Vector2Int id = calcChunkId( pos );
			if ( preChunkId_ != id ) {
				var activeIds = new List<Vector2Int>();
				for (int y = id.y - layerNum_; y <= id.y + layerNum_; ++y) {
					for (int x = id.x - layerNum_; x <= id.x + layerNum_; ++x) {
						activeIds.Add( new Vector2Int( x, y ) );
					}
				}
				// preActiveに無くてactiveにあるのが新規アクティブIds
				// preActiveにあってactiveに無いのが非アクティブIds
				var newActiveIds = new List<Vector2Int>();
				var noneActiveIds = new List<Vector2Int>();
				foreach ( var activeId in activeIds ) {
					bool isNewActive = true;
					foreach ( var preActiveId in preActiveIds_ ) {
						if ( preActiveId == activeId ) {
							// 引き続きアクティブ
							isNewActive = false;
							break;
						}
					}
					if ( isNewActive == true ) {
						// 新規アクティブに
						newActiveIds.Add( activeId );
					}
				}
				foreach ( var preActiveId in preActiveIds_ ) {
					bool isnoneActive = true;
					foreach ( var activeId in activeIds ) {
						if (preActiveId == activeId) {
							// 引き続きアクティブ
							isnoneActive = false;
							break;
						}
					}
					if (isnoneActive == true) {
						// 非アクティブに
						noneActiveIds.Add( preActiveId );
					}
				}

				// 通知
				changeChunkCallback_( newActiveIds, noneActiveIds );

                preChunkId_ = id;
                preActiveIds_ = activeIds;
			}
		}
	}

	Vector2Int calcChunkId( Vector3 pos ) {
		return calcPlaneChunkId_( pos );
	}

	Vector2Int calcPlaneXYChunkId( Vector3 pos ) {
		return new Vector2Int(
			Mathf.FloorToInt( ( pos - offset_ ).x / size_ ),
			Mathf.FloorToInt( ( pos - offset_ ).y / size_ )
		);
	}
	Vector2Int calcPlaneXZChunkId(Vector3 pos) {
		return new Vector2Int(
			Mathf.FloorToInt( ( pos - offset_ ).x / size_ ),
			Mathf.FloorToInt( ( pos - offset_ ).z / size_ )
		);
	}
	Vector2Int calcPlaneYZChunkId(Vector3 pos) {
		return new Vector2Int(
			Mathf.FloorToInt( ( pos - offset_ ).y / size_ ),
			Mathf.FloorToInt( ( pos - offset_ ).z / size_ )
		);
	}

	bool bInitialized_ = false;					// 初期化した？
	float size_ = 1.0f;							// チャンク図形の辺の長さ
	Vector3 offset_ = Vector2.zero;				// 原点位置のオフセット
	Vector2Int preChunkId_ = Vector2Int.zero;   // 初期チャンクID
	List<Vector2Int> preActiveIds_ = new List<Vector2Int>();	// 直前にアクティブなIDs
	bool bCalledInitCallback_ = false;          // 初期コールバック呼んだ？
	int layerNum_ = 1;                          // アクティブ化する層数
	System.Func<Vector3, Vector2Int> calcPlaneChunkId_;	// Idを算出
}
