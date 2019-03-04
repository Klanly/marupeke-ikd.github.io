using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField]
    FrameBlock blockPrefab_;

	[SerializeField]
	GameObject root_;

    public class Param {
		public int level_ = 1;
        public int colNum_ = 12;
		public int minStackNum_ = 2;
		public int maxStackNum_ = 4;
		public int groupNum_ = 8;
		public float innerRadius_ = -1.0f;
		public float brokenWaitSec_ = 0.1f;
		public float brokenIntervalSec_ = 0.1f;
		public float blockFallSec_ = 0.2f;
		public float electricNeedleSpeed_ = 20.0f;      // 最遠位置から最近位置に辿り着くまでの秒数
	}

	// 全部消したコールバック
	public System.Action AllBlockDeletedCallback { set { allBlockDeletedCallback_ = value; } }

	// ブロックを崩したコールバック
	//  colNum, rowNum, chainCount
	public System.Action< int, int, int > BreakBlocksCallback { set { breakBlocksCallback_ = value; } }

	// 電気針の移動速度を取得
	public float getElectricNeedleSpeed() {
		return param_.electricNeedleSpeed_;
	}

	// タワーを設定
	public void setup( Param param )
    {
        param_ = param;
	}

	// パラメータを取得
	public Param getParam() {
		return param_;
	}

	// 現在の最大高さを取得
	public int getCurMaxHeight() {
		return curMaxHeight_;
	}

	// カラム数を取得
	public int getColNum() {
		return param_.colNum_;
	}

	// 指定位置のブロックをインサート
	public void insertBlock( int colIdx, int rowIdx ) {
		if ( bAllowInsert_ == false )
			return;

		if ( rowIdx >= blockCols_[ colIdx ].Count )
			return;
		var srcBlock = blockCols_[ colIdx ][ rowIdx ];

		float deg = 360.0f / param_.colNum_;
		float rad = deg * Mathf.Deg2Rad;

		var block = Instantiate<FrameBlock>( blockPrefab_ );
		block.transform.parent = root_.transform;
		float radius = param_.innerRadius_ > 0.0f ? param_.innerRadius_ : block.getWidth() / ( 2.0f * Mathf.Sin( rad * 0.5f ) );
		block.transform.localPosition = new Vector3( radius * Mathf.Sin( rad * colIdx ), block.getHeight() * rowIdx, -radius * Mathf.Cos( rad * colIdx ) );
		block.transform.localRotation = Quaternion.Euler( 0.0f, -deg * colIdx, 0.0f );
		Color color = srcBlock.getFrameColor();

		block.setGroupIdx( srcBlock.getGroupIdx() );
		block.setFrameColor( color );
		block.setBodyColor( color );
		blockCols_[ colIdx ].Insert( rowIdx, block );

		for ( int i = rowIdx + 1; i < blockCols_[ colIdx ].Count; ++i ) {
			var b = blockCols_[ colIdx ][ i ];
			b.moveUp();
		}

		// 揃っているかチェック
		checkGroupOrder( 0 );
	}

	// タワー開始
	public void start()
    {
        state_ = new Intro( this );
    }

	// 現在の最大高を更新
	void updateCurMaxHeight() {
		int height = 0;
		for ( int i = 0; i < blockCols_.Count; ++i ) {
			height = Mathf.Max( height, blockCols_[ i ].Count );
		}
		curMaxHeight_ = height;
	}

	// 揃っているかチェック
	bool checkGroupOrder( int chainCount ) {
		updateCurMaxHeight();   // 高さ更新

		int orderTopRowIndex = -1;	// 揃ったグループの高さ

		bool bNoneOrder = false;
		for ( int r = 0; r < curMaxHeight_; ++r ) {
			int groupIdx = -1;
			if ( r >= blockCols_[ 0 ].Count ) {
				// 揃う事がもう無いので終了
				break;
			}
			if ( blockCols_[ 0 ][ r ].isTop() == false ) {
				// トップのみ注目
				continue;
			}

			// 以下トップブロックのみに注目出来る

			groupIdx = blockCols_[ 0 ][ r ].getGroupIdx();
			for ( int c = 1; c < param_.colNum_; ++c ) {
				if ( r >= blockCols_[ c ].Count ) {
					// 隙間アリ、揃っていない
					bNoneOrder = true;
					break;
				}
				if ( groupIdx != blockCols_[ c ][ r ].getGroupIdx() || blockCols_[ c ][ r ].isTop() == false ) {
					// 揃っていない
					bNoneOrder = true;
					break;
				}
			}
			if ( bNoneOrder == true )
				break;

			// 揃っている！
			orderTopRowIndex = r;
			break;
		}

		if ( orderTopRowIndex == -1 ) {
			return false;     // 揃っていないので終了
		}

		// 揃っている！//

		chainCount++;

		// 削除演出中はインサート禁止
		bAllowInsert_ = false;

		// 揃ったラインのブロックを削除
		float destroyTime = param_.brokenWaitSec_;
		float destroyLastTime = destroyTime + param_.brokenWaitSec_;
		int orderLineNum = orderTopRowIndex + 1;
		int destroyBlockNum = orderLineNum * param_.colNum_;
		int destroyBlockIdx = 0;
		for ( int r = 0; r < orderLineNum; ++r ) {
			for ( int c = 0; c < param_.colNum_; ++c ) {
				var block = blockCols_[ c ][ r ];
				block.destroy( Lerps.Float.linear( destroyTime, destroyLastTime, ( float )destroyBlockIdx / destroyBlockNum ) );
				destroyBlockIdx++;
			}
		}

		GlobalState.wait( destroyLastTime, () => {
			// 詰める
			for ( int c = 0; c < param_.colNum_; ++c ) {
				for ( int r = orderLineNum; r < blockCols_[ c ].Count; ++r ) {
					blockCols_[ c ][ r ].moveDown( orderLineNum, param_.blockFallSec_ );
				}
				blockCols_[ c ].RemoveRange( 0, orderLineNum );
			}

			// 最大高を更新
			updateCurMaxHeight();

			// 全部消した？
			if ( curMaxHeight_ == 0 ) {
				if ( allBlockDeletedCallback_ != null )
					allBlockDeletedCallback_();
				allBlockDeletedCallback_ = null;
			}

			// 演出終了
			//  さらに上のグループが揃っていたら連鎖演出
			GlobalState.wait( param_.blockFallSec_, () => {
				if ( checkGroupOrder( chainCount ) == false )
					bAllowInsert_ = true;   // 自分の演出が最後なのでインサートを許可
				return false;
			} );
			return false;
		} );

		// ブロック破壊通知
		if ( breakBlocksCallback_ != null )
			breakBlocksCallback_( param_.colNum_, orderTopRowIndex + 1, chainCount );

		return true;
	}

	private void Awake() {
		colors_ = new Color[] {
			red_, blue_, green_, yellow_
		};
	}

	// Use this for initialization
	void Start () {
		if ( state_ != null )
			state_ = state_.update();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    class StateBase : State
    {
        public StateBase( Tower parent )
        {
            parent_ = parent;
        }
        protected Tower parent_;
    }

    class Intro : StateBase
    {
        public Intro( Tower parent ) : base( parent )
        {
        }

		protected override State innerInit() {
			// 各列のサイズを決定
			var groupHeights = new List<int>();
			for ( int i = 0; i < parent_.param_.groupNum_; ++i ) {
				groupHeights.Add( Random.Range( parent_.param_.minStackNum_, parent_.param_.maxStackNum_ + 1 ) );
			}

			// 列リスト初期化
			for ( int c = 0; c < parent_.param_.colNum_; ++c ) {
				parent_.blockCols_.Add( new List<FrameBlock>() );
			}

			// タワーを構成するブロックを作成
			var param = parent_.param_;
			for ( int g = 0; g < param.groupNum_; ++g ) {
				int num = groupHeights[ g ];
				float deg = 360.0f / param.colNum_;
				float rad = deg * Mathf.Deg2Rad;
				// 各列の積載ブロック数を決定
				// もし全列が同じ数だったら振り直し
				int[] cnums = new int[ param.colNum_ ];
				while ( true ) {
					cnums[ 0 ] = Random.Range( 1, num + 1 );
					bool bDefferent = false;
					for ( int c = 1; c < param.colNum_; ++c ) {
						cnums[ c ] = Random.Range( 1, num + 1 );
						if ( cnums[ 0 ] != cnums[ c ] )
							bDefferent = true;
					}
					if ( bDefferent == true )
						break;
				}
				for ( int c = 0; c < param.colNum_; ++c ) {
					int cnum = Random.Range( 1, num + 1 );
					for ( int e = 0; e < cnum; ++e ) {
						int hp = parent_.blockCols_[ c ].Count;
						var block = Instantiate<FrameBlock>( parent_.blockPrefab_ );
						block.transform.parent = parent_.root_.transform;
						float radius = param.innerRadius_ > 0.0f ? param.innerRadius_ : block.getWidth() / ( 2.0f * Mathf.Sin( rad * 0.5f ) );
						block.transform.localPosition = new Vector3( radius * Mathf.Sin( rad * c ), block.getHeight() * hp, -radius * Mathf.Cos( rad * c ) );
						block.transform.localRotation = Quaternion.Euler( 0.0f, -deg * c, 0.0f );
						Color color = parent_.colors_[ g % parent_.colors_.Length ];

						block.setGroupIdx( g );
						block.setFrameColor( color );
						block.setBodyColor( color );
						if ( e + 1 == cnum ) {
							// グループトップのブロックにトップマークを付記
							block.setTopMark();
						}
						parent_.blockCols_[ c ].Add( block );
					}
				}
			}

			// 最大高を更新
			parent_.updateCurMaxHeight();
			return null;
		}
	}

	State state_;
    Param param_;
	int curMaxHeight_ = 0;
	Color red_ = new Color( 233 / 255.0f, 46 / 255.0f, 46 / 255.0f );
	Color blue_ = new Color( 46 / 255.0f, 94 / 255.0f, 233 / 255.0f );
	Color green_ = new Color( 56 / 255.0f, 233 / 255.0f, 97 / 255.0f );
	Color yellow_ = new Color( 228 / 255.0f, 234 / 255.0f, 50 / 255.0f );
	Color[] colors_;
	List< List< FrameBlock > > blockCols_ = new List< List< FrameBlock > >();
	System.Action allBlockDeletedCallback_;
	System.Action<int, int, int> breakBlocksCallback_;
	bool bAllowInsert_ = true;
}
