using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialData : MonoBehaviour {

	public class Param {
		public int[,] hBarricades_;
		public int[,] vBarricades_;
		public StageManager.Param stageParam_;
		public List<Vector2Int> enemyPoses_ = new List<Vector2Int>();
	}

	public Param getParam( int index ) {
		if ( index >= params_.Count )
			return null;
		return params_[ index ];
	}

	private void Awake() {
		{
			var param1 = new Param();
			param1.hBarricades_ = new int[ 4, 6 ] {
				{ 0, 0, 0, 0, 0, 0 },
				{ 0, 1, 0, 0, 1, 0 },
				{ 0, 0, 1, 0, 1, 0 },
				{ 0, 0, 0, 0, 0, 0 },
			};
			param1.vBarricades_ = new int[ 5, 5 ] {
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 0 },
				{ 0, 0, 0, 0, 0 },
			};
			var param = new StageManager.Param();
			param.stageIndex_ = 0;
			param.fieldParam_.region_.x = 4;
			param.fieldParam_.region_.y = 5;
			param.fieldParam_.maxBarricadeNum_ = 0;
			param.enemyNum_ = 0;
			param1.stageParam_ = param;
			params_.Add( param1 );
		}
		{
			var param2 = new Param();
			param2.hBarricades_ = new int[ 4, 6 ] {
				{ 0, 0, 0, 1, 0, 0 },
				{ 0, 0, 1, 0, 0, 0 },
				{ 0, 0, 0, 1, 0, 1 },
				{ 0, 0, 0, 1, 0, 0 },
			};
			param2.vBarricades_ = new int[ 5, 5 ] {
				{ 0, 0, 0, 1, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1 },
			};
			var param = new StageManager.Param();
			param.stageIndex_ = 0;
			param.fieldParam_.region_.x = 4;
			param.fieldParam_.region_.y = 5;
			param.fieldParam_.maxBarricadeNum_ = 0;
			param.enemyNum_ = 0;
			param2.stageParam_ = param;
			params_.Add( param2 );
		}
		{
			var param3 = new Param();
			param3.hBarricades_ = new int[ 4, 6 ] {
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 0, 1, 0, 0 },
				{ 0, 0, 0, 1, 0, 1 },
				{ 0, 0, 0, 0, 1, 0 },
			};
			param3.vBarricades_ = new int[ 5, 5 ] {
				{ 0, 0, 0, 0, 1 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 1, 1 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1 },
			};
			var param = new StageManager.Param();
			param.stageIndex_ = 0;
			param.fieldParam_.region_.x = 4;
			param.fieldParam_.region_.y = 5;
			param.fieldParam_.maxBarricadeNum_ = 0;
			param.enemyNum_ = 0;
			param3.stageParam_ = param;
			params_.Add( param3 );
		}
		{
			var param4 = new Param();
			param4.hBarricades_ = new int[ 6, 6 ] {
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 0, 1, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 1, 0, 1, 0 },
				{ 0, 0, 1, 0, 0, 1 },
			};
			param4.vBarricades_ = new int[ 7, 5 ] {
				{ 0, 0, 1, 1, 1 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 1, 0, 1, 0 },
				{ 0, 0, 1, 0, 1 },
			};
			var param = new StageManager.Param();
			param.stageIndex_ = 0;
			param.fieldParam_.region_.x = 6;
			param.fieldParam_.region_.y = 5;
			param.fieldParam_.maxBarricadeNum_ = 0;
			param.enemyNum_ = 0;
			param4.stageParam_ = param;
			params_.Add( param4 );
		}
		{
			var param5 = new Param();
			param5.hBarricades_ = new int[ 6, 6 ] {
				{ 0, 0, 0, 1, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 1, 0, 0, 1 },
			};
			param5.vBarricades_ = new int[ 7, 5 ] {
				{ 0, 0, 1, 1, 1 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 0 },
				{ 0, 0, 1, 1, 1 },
			};
			var param = new StageManager.Param();
			param.stageIndex_ = 0;
			param.fieldParam_.region_.x = 6;
			param.fieldParam_.region_.y = 5;
			param.fieldParam_.maxBarricadeNum_ = 0;
			param.enemyNum_ = 0;
			param5.stageParam_ = param;
			param5.enemyPoses_.Add( new Vector2Int( 4, 2 ) );
			param5.enemyPoses_.Add( new Vector2Int( 5, 4 ) );
			params_.Add( param5 );
		}
	}

	void Start () {

	}

	void Update () {
		
	}

	List<Param> params_ = new List<Param>();
}
