using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_data : Table {
	public static Sound_data getInstance() {
		return instance_;
	}
	Sound_data() {
		create( "Table/sound_data" );
	}

	// 1レコードを格納
	protected override void storeData( Dictionary<string, Val> values ) {
		var param = new Param();
		param.id_ = values[ "id" ].sVal_;
		param.filename_ = values[ "filename" ].sVal_;
		param.name_ = values[ "name" ].sVal_;
		param.group_ = values[ "group" ].iVal_;
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
		public string filename_;
		public string name_;
		public int group_;

	}
	static Sound_data instance_ = new Sound_data();
	Dictionary< string, Param > params_ = new Dictionary<string, Param>();
	List<Param> paramList_ = new List<Param>();
}