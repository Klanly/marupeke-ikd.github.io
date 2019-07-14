using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_tokyo_shibuya_data : Table {
	public static Data_tokyo_shibuya_data getInstance() {
		return instance_;
	}
	Data_tokyo_shibuya_data() {
		create( "Table/data_tokyo_shibuya_data" );
	}

	// 1レコードを格納
	protected override void storeData( Dictionary<string, Val> values ) {
		var param = new Param();
		param.id_ = values[ "id" ].sVal_;
		param.name_ = values[ "name" ].sVal_;
		param.material_ = values[ "material" ].sVal_;
		param.weight_ = values[ "weight" ].fVal_;
		param.weightUnit_ = values[ "weightUnit" ].sVal_;
		param.dimensionX_ = values[ "dimensionX" ].fVal_;
		param.dimensionY_ = values[ "dimensionY" ].fVal_;
		param.dimensionZ_ = values[ "dimensionZ" ].fVal_;
		param.dimensionUnit_ = values[ "dimensionUnit" ].sVal_;
		param.answer_ = values[ "answer" ].sVal_;
		param.image_ = values[ "image" ].sVal_;
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
		public string name_;
		public string material_;
		public float weight_;
		public string weightUnit_;
		public float dimensionX_;
		public float dimensionY_;
		public float dimensionZ_;
		public string dimensionUnit_;
		public string answer_;
		public string image_;

	}
	static Data_tokyo_shibuya_data instance_ = new Data_tokyo_shibuya_data();
	Dictionary< string, Param > params_ = new Dictionary<string, Param>();
	List<Param> paramList_ = new List<Param>();
}