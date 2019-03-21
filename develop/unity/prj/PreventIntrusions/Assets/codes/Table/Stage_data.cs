using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_data : Table {
	public static Stage_data getInstance() {
		return instance_;
	}
	Stage_data() {
		create( "Table/stage_data" );
	}

	// 1レコードを格納
	protected override void storeData( Dictionary<string, Val> values ) {
		var param = new Param();
		param.id_ = values[ "id" ].sVal_;
		param.width_ = values[ "width" ].iVal_;
		param.height_ = values[ "height" ].iVal_;
		param.enemyNum_ = values[ "enemyNum" ].iVal_;
		param.maxBarricadeNum_ = values[ "maxBarricadeNum" ].iVal_;
		param.time_ = values[ "time" ].iVal_;
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
		public int width_;
		public int height_;
		public int enemyNum_;
		public int maxBarricadeNum_;
		public int time_;

	}
	static Stage_data instance_ = new Stage_data();
	Dictionary< string, Param > params_ = new Dictionary<string, Param>();
	List<Param> paramList_ = new List<Param>();
}