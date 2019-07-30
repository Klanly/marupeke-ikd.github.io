using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの弾(Level1)
public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    float initSpeed_ = 1.0f;    // 初速(m/s)

    [SerializeField]
    float lifeTime_ = 3.0f;

    [SerializeField]
    float radius_ = 0.25f;

    public System.Action FinishCallback { set { finishCallback_ = value; } }
    System.Action finishCallback_;

    // セットアップ
    public void setup( Player player ) {
        curTime_ = 0.0f;
        blockCollideManager_ = player.getBlockCollideManager();

        // Playerの進行方向へ飛ばす
        transform.forward = player.transform.forward;
        transform.position = player.transform.position;
        gameObject.SetActive( true );
    }

    private void Awake() {
        gameObject.SetActive( false );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;
        curTime_ += t;

        if ( curTime_ >= lifeTime_ ) {
            finishCallback_();
            gameObject.SetActive( false );
        }

        var pos = transform.localPosition;
        pos += transform.forward * initSpeed_ * t;
        transform.localPosition = pos;

        // コリジョン
        Vector2 pene = Vector2.zero;
        var block = blockCollideManager_.toCircleCollide( new Vector2( pos.x, pos.z ), radius_, ref pene );
        if ( block != null ) {
			// ブロックのHPを自分の攻撃力分下げる
			if ( block.damage( attackPower_, false ) == true ) {
                // 破壊したので中のイベントを発生
                GameManager.getInstance().emitBlockEvent( block );
                block.setDestroy();
            }

            // ブロック変更通知
            GameManager.getInstance().updateBlock( block );
            finishCallback_();
            gameObject.SetActive( false );
        }
    }

    float curTime_ = 0.0f;
    BlockCollideManager blockCollideManager_;
	short attackPower_ = 300;
}
