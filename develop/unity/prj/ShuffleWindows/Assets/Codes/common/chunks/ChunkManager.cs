using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// チャンク管理者
//  指定のチャンクのアクティブ・非アクティブを自動化
//
// 【ルール】
//  ターゲット点が存在するチャンクの周囲nセルをアクティブにする(n≧1)
//  毎フレームチェックしターゲットが所属するチャンクから外に出たらアクティブを更新
//  切り替え情報はコールバックされる
//
//  【チャンク図形】
//  1つのチャンクを構成する図形は矩形でも円でも六角形でも多角形でも球でも構わない。
//  このクラスではそれは意識しない。
//  単純に全ての図形をリンク化するとコストが掛かるので、矩形や六角形など敷き詰め
//  可能な図形については派生クラスで具体的な実装をする（矩形など）
public class ChunkManager<T>
{
	// チャンク更新時のコールバック
	//  arg1: 新しくアクティブになったチャンクIDのリスト
	//  arg2: 非アクティブに変更されたチャンクIDのリスト
	public System.Action<List<T>, List<T>> ChangeChunkCallback { set{ changeChunkCallback_ = value; } }
	protected System.Action<List<T>, List<T>> changeChunkCallback_;

	// チャンクの状態を更新
	//  pos: ターゲットの位置
	public virtual void updateChunk(Vector3 pos) { }
}
