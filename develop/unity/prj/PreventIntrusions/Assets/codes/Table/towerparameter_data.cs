using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C:\IKDDocs2\Programs\git_marupeke_io\develop\unity\prj\PreventIntrusions\RowData\Table\towerparameter_data : Table {
	public static C:\IKDDocs2\Programs\git_marupeke_io\develop\unity\prj\PreventIntrusions\RowData\Table\towerparameter_data getInstance() {
		return instance_;
	}
	C:\IKDDocs2\Programs\git_marupeke_io\develop\unity\prj\PreventIntrusions\RowData\Table\towerparameter_data() {
		create( "Table/C:\IKDDocs2\Programs\git_marupeke_io\develop\unity\prj\PreventIntrusions\RowData\Table\towerparameter_data" );
	}

	// 1レコードを格納
	protected override void storeData( Dictionary<string, Val> values ) {
		var param = new Param();
		param.id_ = values[ "id" ].sVal_;
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
		param.bgm_ = values[ "bgm" ].sVal_;
		params_[ values[ "id" ].sVal_ ] = param;

		paramList_.Add( param );
	}

	// データ数を取得
	public int getRowNum() {
		return params_.Count;
	}

	// パラメータを取得
	public Param getParam( string id ) {
		return params_[ id ];
	}

	// パラメータをインデックスで取得
	public Param getParamFromIndex( int idx ) {
		if ( idx >= paramList_.Count )
			return null;
		return paramList_[ idx ];
	}
	
	public class Param {
		public string id_;
		public int level_;
		public int colNum_;
		public int minStackNum_;
		public int maxStackNum_;
		public int groupNum_;
		public float innerRadius_;
		public float brokenWaitSec_;
		public float brokenIntervalSec_;
		public float blockFallSec_;
		public float electricNeedleSpeed_;
		public string bgm_;

	}
	static C:\IKDDocs2\Programs\git_marupeke_io\develop\unity\prj\PreventIntrusions\RowData\Table\towerparameter_data instance_ = new C:\IKDDocs2\Programs\git_marupeke_io\develop\unity\prj\PreventIntrusions\RowData\Table\towerparameter_data();
	Dictionary< string, Param > params_ = new Dictionary<string, Param>();
	List<Param> paramList_ = new List<Param>();
}