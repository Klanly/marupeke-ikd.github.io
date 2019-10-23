using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
	[Inject]
	IOXInput input_ = null;

	[Inject]
	GameManager gameManager_;

	[SerializeField]
	float speed_ = 1.0f;

	[SerializeField]
	float flyTime_ = 1.0f;

	[SerializeField]
	float flyHeight_ = 2.0f;

	[SerializeField]
	Field field_;

	[SerializeField]
	float scrollSpeed_ = 1.0f;

	[SerializeField]
	Railling raillingPrefab_ = null;

	[SerializeField]
	float raillingLen_ = 2.0f;

	[SerializeField]
	Animation animation_;

	[SerializeField]
	GameObject sheep_;

	[SerializeField, Range( 0.001f, 1.0f )]
	float adjRunSpeed_ = 1.0f;

	[SerializeField]
	Explosion[] explosions_ = new Explosion[ 9 ];

	[SerializeField]
	GameObject fukidashiPrefab_ = null;

	public float getCurDistance()
	{
		return curDistance_;
	}

	private void Awake()
	{
		animation_.Play( "run" );
		int c = 0;
		for ( int y = -1; y <= 1; ++y ) {
			for (int x = -1; x <= 1; ++x ) {
				explosions_[ c ].transform.localPosition = new Vector3( field_.Width * x, field_.Height * y, 0.0f );
				c++;
			}
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		preNodePos_ = transform.position;
		prePos_ = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		if (bGameOver_ == true)
			return;

		prePos_ = transform.localPosition;

		// 上下左右移動
		// +スクロール
		Vector3 dir = input_.getDirection();
		var p = transform.localPosition + dir * speed_ * Time.deltaTime;
		p.y += scrollSpeed_ * Time.deltaTime;
		transform.localPosition = p;

		// ジャンプ
		if (jumpState_ == null) {
			animation_.Play( "run" );
			if (input_.decide() == true) {
				jumpTime_ = 0.0f;
				jumpState_ = jump;
				animation_.Play( "jump" );
			}
		} else {
			jumpState_();
		}

		p = adjustPosition( transform.localPosition );

		Vector3 sheepDir = p - prePos_;
		if ( sheepDir.magnitude > 0.0f ) {
			var preRot = sheep_.transform.rotation;
			var newRot = Quaternion.LookRotation( sheepDir, Vector3.back );
			sheep_.transform.rotation = Quaternion.Lerp( preRot, newRot, 0.3f );
			animation_[ "run" ].speed = sheepDir.magnitude / adjRunSpeed_;
		}

		// 柵作成チェック
		checkRaillingCreation();

		//		var p = transform.localPosition;
		//		var tex = gameManager_.getLineRecordTexture();
		//		tex.setPixel( ( int )p.x, ( int )p.y, Color.black, true );
		//		tex.apply();
	}

	void jump()
	{
		jumpTime_ += Time.deltaTime;
		if ( jumpTime_ >= flyTime_ ) {
			jumpTime_ = flyTime_;
		}
		float t = jumpTime_;
		var p = transform.localPosition;
		p.z = -4.0f * flyHeight_ * t / flyTime_ * ( -t / flyTime_ + 1.0f );
		if ( jumpTime_ == flyTime_ ) {
			p.z = 0.0f;
			jumpState_ = null;
		}
		transform.localPosition = p;
	}

	// 位置関係を調整
	Vector3 adjustPosition( Vector3 p )
	{
		if (p.x < field_.Left) {
			p.x += field_.Width;
			prePos_.x += field_.Width;
		} else if (p.x > field_.Right) {
			p.x -= field_.Height;
			prePos_.x -= field_.Width;
		}
		if (p.y < field_.Bottom) {
			p.y += field_.Height;
			prePos_.y += field_.Height;
		} else if (p.y > field_.Top) {
			p.y -= field_.Height;
			prePos_.y -= field_.Height;
		}
		transform.localPosition = p;

		curDistance_ += p.y - prePos_.y;

		return p;
	}

	void checkRaillingCreation()
	{
		var p = transform.position;
		var p2 = new Vector2( p.x, p.y );
		float l = ( p2 - preNodePos_ ).magnitude;
		if ( l >= raillingLen_ ) {
			if ( l <= raillingLen_ * 1.9f ) {
				// フィールド内に収まっている柵
				var railling = PrefabUtil.createInstance( raillingPrefab_, field_.transform );
				field_.addRailling( railling );
				railling.create( preRail_, p2, -p.z + 1.0f );
				preNodePos_ = p;
				preRail_ = railling;
			} else {
				// ワープ先なので補正
				Vector2[] ofs = new Vector2[ 8 ] {
					new Vector2( -field_.Width, -field_.Height ),
					new Vector2(  field_.Width, -field_.Height ),
					new Vector2( -field_.Width,  field_.Height ),
					new Vector2(  field_.Width,  field_.Height ),
					new Vector2( -field_.Width, 0.0f ),
					new Vector2(  field_.Width, 0.0f ),
					new Vector2( 0.0f, -field_.Height ),
					new Vector2( 0.0f,  field_.Height )
				};
				int idx = 0;
				float f = 1000000.0f;
				Vector3 adjPos = Vector3.zero;
				for ( int i = 0; i < 8; ++i ) {
					float d = ( p2 + ofs[ i ] - preNodePos_ ).magnitude;
					if ( d < f ) {
						adjPos = p2 + ofs[ i ];
						f = d;
						idx = i;
					}
				}
				if (f >= raillingLen_ && f <= raillingLen_ * 1.9f ) {
					var railling = PrefabUtil.createInstance( raillingPrefab_, field_.transform );
					field_.addRailling( railling );
					railling.create( preRail_, adjPos, -p.z + 1.0f );
					preNodePos_ = p;
					var railling2 = PrefabUtil.createInstance( raillingPrefab_, field_.transform );
					field_.addRailling( railling2 );
					railling2.create( null, p, -p.z + 1.0f );
					preRail_ = railling2;
				}

				// TODO:
				// 境界を跨ぐ柵をワープ先側にも作成
			}
		} 
	}

	private void OnTriggerEnter(Collider other)
	{
		if (bGameOver_ == true)
			return;

		if ( other.gameObject.tag == "item" ) {
			// アイテムゲット
			var item = other.gameObject.GetComponentInParent<Item>();
			if ( item != null ) {
				item.getMotion();
			}
			// 周囲を爆破
			foreach (var e in explosions_) {
				e.explosion();
				e.CorePower = explosions_[ 0 ].CorePower;
				e.SonicVector = explosions_[ 0 ].SonicVector;
				e.DecRate = explosions_[ 0 ].DecRate;
				e.GravityPower = explosions_[ 0 ].GravityPower;
			}
		} else if ( other.gameObject.tag == "rail" ) {
			var rail = other.gameObject.GetComponentInParent<Railling>();
			if (rail != null ) {
				if (rail.noDamageCount() < 0) {
					// ゲームオーバー
					gameManager_.toGameOver();
					bGameOver_ = true;
				}
				return;
			}
			var block = other.gameObject.GetComponentInParent<Block>();
			if ( block != null ) {
				if ( block.noDamageCount() < 0 ) {
					// ゲームオーバー
					gameManager_.toGameOver();
					bGameOver_ = true;
				}
			}
			return;
		} else if ( other.gameObject.tag == "rail_upper" ) {
			// +100m追加
			var addScore = PrefabUtil.createInstance( fukidashiPrefab_, transform, Vector3.zero );
			Destroy( addScore, 1.0f );
			curDistance_ += 100.0f;
		}
	}

	System.Action jumpState_ = null;
	bool bJumping_ = false;
	float jumpTime_ = 0.0f;
	Vector2 preNodePos_ = Vector2.zero;
	Railling preRail_ = null;
	Vector3 prePos_ = Vector3.zero;
	float curDistance_ = 0.0f;
	bool bGameOver_ = false;
}
