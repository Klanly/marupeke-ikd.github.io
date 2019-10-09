﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステージ作成管理者
public class StageManager : MonoBehaviour
{
	public enum FieldObjectType {
		Empty,
		Start_R,
		Start_L,
		Start_U,
		Start_D,
		Goal,
		Tento,
		Ant,
		Wood_V,
		Wood_H,
		Wood_R,
		Wood_L,
		Rock
	}

	public class Data {
		public FieldObjectType[,] fieldObjectTypes_;
		public FieldObjectType bugType_;
		public int emitBugNum_ = 0;
		public Vector3 center_ = Vector3.zero;
		public Vector2 region_ = Vector2.zero;
	}

	// ステージ作成
	public Data createStage( int stageId, ObjectManager objectManager ) {
		var data = getStage( stageId );
		if ( data.emitBugNum_ == 0 ) {
			return null;
		}
		var nd = new Dictionary<FieldObjectType, string> {
			{ FieldObjectType.Rock, "rock" },
			{ FieldObjectType.Start_R, "start" },
			{ FieldObjectType.Start_L, "start" },
			{ FieldObjectType.Start_D, "start" },
			{ FieldObjectType.Start_U, "start" },
			{ FieldObjectType.Goal, "goal" },
			{ FieldObjectType.Wood_V, "wood_v" },
			{ FieldObjectType.Wood_H, "wood_h" },
			{ FieldObjectType.Wood_R, "wood_r" },
			{ FieldObjectType.Wood_L, "wood_l" },
			{ FieldObjectType.Tento, "tento" },
			{ FieldObjectType.Ant, "ant" },
			{ FieldObjectType.Empty, "empty" },
		};

		// フィールド
		float s = 2.0f;
		data.region_ *= s;
		var starts_ = new List<ObjectManager.FieldObjectParam>();
		for ( int y = 0; y < data.fieldObjectTypes_.GetLength( 1 ); ++y ) {
			for (int x = 0; x < data.fieldObjectTypes_.GetLength( 0 ); ++x ) {
				var p = new ObjectManager.FieldObjectParam();
				p.forward_ = Vector3.forward;
				p.initPos_ = new Vector3( x, 0.0f, y ) * s;
				p.objName_ = nd[ data.fieldObjectTypes_[ x, y ] ];
				switch (data.fieldObjectTypes_[ x, y ]) {
				case FieldObjectType.Start_U:
					p.forward_ = Vector3.forward;
					starts_.Add( p );
					break;
				case FieldObjectType.Start_D:
					p.forward_ = Vector3.back;
					starts_.Add( p );
					break;
				case FieldObjectType.Start_R:
					p.forward_ = Vector3.right;
					starts_.Add( p );
					break;
				case FieldObjectType.Start_L:
					p.forward_ = Vector3.left;
					starts_.Add( p );
					break;
				}
				objectManager.addFieldObject( p );
			}
		}

		// 虫
		float bugEmitSec = 1.5f;
		foreach ( var st in starts_ ) {
			var p = new ObjectManager.FieldObjectParam();
			p.forward_ = st.forward_;
			p.initPos_ = st.initPos_;
			p.objName_ = "tento";
			for ( int i = 0; i < data.emitBugNum_; ++i ) {
				GlobalState.wait( bugEmitSec * i, () => {
					objectManager.addFieldObject( p );
					return false;
				} );
			}
		}

		data.center_ *= s;
		return data;
	}

	// ステージ情報
	Data getStage( int stageId )
	{
		var fd = new Dictionary<char, FieldObjectType>() {
			{ '-', FieldObjectType.Empty },
			{ 'w', FieldObjectType.Start_U },
			{ 'a', FieldObjectType.Start_L },
			{ 'd', FieldObjectType.Start_R },
			{ 'x', FieldObjectType.Start_D },
			{ 'g', FieldObjectType.Goal },
			{ 'v', FieldObjectType.Wood_V },
			{ 'h', FieldObjectType.Wood_H },
			{ 'r', FieldObjectType.Wood_R },
			{ 'l', FieldObjectType.Wood_L },
			{ 'o', FieldObjectType.Rock },
		};

		Data data = new Data();
		string[] stage = null;
		if ( stageId == 0 ) {
			stage = new string[] {
				"oooooooooo",
				"o--------o",
				"o-----g--o",
				"o--------o",
				"o-----l-lo",
				"o--------o",
				"o--------o",
				"o--d--l--o",
				"o--------o",
				"oooooooooo",
			};
			data.emitBugNum_ = 10;
			data.bugType_ = FieldObjectType.Tento;
		}

		if ( stage == null ) {
			return null;
		}

		int maxY = stage.Length;
		int maxX = 0;
		for ( int y = 0; y < stage.Length; ++y ) {
			if ( stage[ y ].Length > maxX ) {
				maxX = stage[ y ].Length;
			}
		}
		FieldObjectType[,] f = new FieldObjectType[ maxX, maxY ];
		for (int y = 0; y < stage.Length; ++y) {
			var s = stage[ y ];
			int ey = stage.Length - y - 1;
			for ( int x = 0; x < s.Length; ++x ) {
				f[ x, ey ] = ( x >= s.Length ? FieldObjectType.Empty : fd[ s[x] ] );
			}
		}
		data.fieldObjectTypes_ = f;
		data.center_ = new Vector3( 0.5f * ( maxX - 1 ), 0.0f, 0.5f * ( maxY - 1 ) );
		data.region_ = new Vector2( maxX - 2, maxY - 2 ); // 壁は除く

		return data;
	}
}
