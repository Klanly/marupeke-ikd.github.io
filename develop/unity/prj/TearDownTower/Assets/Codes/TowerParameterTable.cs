using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerParameterTable : Table {
	public TowerParameterTable() {
		create( "Table/towerparameter_data.tsv" );
	}

	// データ格納前コール
	//  派生クラスで必要に応じて使用
	protected override void preStore(int dataNum) {
		params_ = new Tower.Param[ dataNum ];
	}

	// 1レコードを格納
	//  派生クラスで具体的な格納を行う
	protected override void storeData( Dictionary<string, Val> values ) {
		var param = new Tower.Param();
		var id = values[ "id" ];
		param.level_ = values[ "level" ].iVal_;
		param.colNum_ = values[ "colNum" ].iVal_;
		param.minStackNum_ = values[ "minStackNum" ].iVal_;
		param.maxStackNum_ = values[ "maxStackNum" ].iVal_;
		param.groupNum_ = values[ "groupNum" ].iVal_;
		param.innerRadius_ = values[ "innerRadius" ].fVal_;
		param.brokenWaitSec_ = values[ "brokenWaitSec" ].fVal_;
		param.brokenIntervalSec_ = values[ "brokenIntervalSec" ].fVal_;
		param.blockFallSec_ = values[ "blockFallSec" ].fVal_;
		param.electricNeedleSpeed_ = values[ "electricNeedleSpeed" ].fVal_;
		params_[ dataIndex_ ] = param;
		dataIndex_++;
	}

	// Towerパラメータを取得
	public Tower.Param getParam( int towerLevel ) {
		if ( towerLevel >= params_.Length + 1 )
			return params_[ params_.Length - 1 ];
		return params_[ towerLevel - 1 ];
	}

	Tower.Param[] params_;
	int dataIndex_ = 0;
}
