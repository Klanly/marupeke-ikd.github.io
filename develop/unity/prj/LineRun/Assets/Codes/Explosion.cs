using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 柵爆破
//  このオブジェクトの周りにある柵を吹き飛ばします

public class Explosion : MonoBehaviour
{
	[SerializeField]
	Collider colloder_;

	[SerializeField]
	float corePower_ = 10.0f;   // 爆発中心点のフォース（kgm/s^2）

	[SerializeField]
	float sonicVector_ = 1.0f;  // 衝撃波の伝わる速さ（m/sec）

	[SerializeField, Range( 0.01f, 0.99f ) ]		// 爆破力の減衰率
	float decRate_ = 0.90f;

	[SerializeField]
	float gravityPower_ = 9.81f;			// 重力加速度

	[ SerializeField]
	string targetTag_ = "";

	[SerializeField]
	bool bDebug_ = false;


	public float CorePower { set { corePower_ = value; } get { return corePower_; } }
	public float SonicVector { set { sonicVector_ = value; } get { return sonicVector_; } }
	public float DecRate { set { decRate_ = value; } get { return decRate_; } }
	public float GravityPower { set { gravityPower_ = value; } get { return gravityPower_; } }

	private void Awake()
	{
		colloder_.enabled = false;
	}

	void Start()
    {
        
    }

    void Update()
    {
		g_.z = gravityPower_;

		if ( bDebug_ == true ) {
			bDebug_ = false;
			explosion();
		}

		validateCount_--;
		if (validateCount_ <= 0 ) {
			colloder_.enabled = false;
			validateCount_ = 0;
		}
	}

	// 今の位置から周囲を爆破する
	public void explosion()
	{
		// コリジョンを2フレーム程有効にしてOnTriggerEnterに柵を
		// 飛び込ませる
		colloder_.enabled = true;
		validateCount_ = 2;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ( other.tag == targetTag_) {
			other.enabled = false;
			// AutoExplotionをくっつけて勝手に飛んでもらう
			var e = other.gameObject.AddComponent< AutoExplosion >();
			var corePos = transform.position;
			corePos.z += 2.0f;  // 地面下に
			e.setup( g_, corePos, corePower_, sonicVector_, decRate_, ( t ) => {
				if ( t >= 0.5f && e.transform.position.z >= 0.0f )
					return AutoExplosion.DestroyFlag.GameObject;
				return AutoExplosion.DestroyFlag.Continue;
			} );
		}
	}

	private void OnDrawGizmos()
	{
		// 衝撃波の伝わる速さ
		Gizmos.DrawWireSphere( transform.position, sonicVector_ );

		// コアの強さが半減する半径
		float x = -Mathf.Log( 0.5f ) / Mathf.Log( decRate_ );
		Gizmos.DrawWireSphere( transform.position, x );
	}

	int validateCount_ = 0;
	Vector3 g_ = new Vector3( 0.0f, 0.0f, 9.81f );
}
