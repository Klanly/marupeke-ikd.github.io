using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float radius_ = 1.0f;

    [SerializeField]
    PlayerBullet bulletPrefab_;

    [SerializeField]
    Transform bulletStockRoot_;


    public void setup( BlockCollideManager fieldCollider ) {
        fieldCollider_ = fieldCollider;
    }

    // ブロック衝突コライダー取得
    public BlockCollideManager getBlockCollideManager() {
        return fieldCollider_;
    }

    // 敵弾を登録
    //  Playerは自分で自分の衝突を検知します
    public void addEnemyBullet( EnemyBulletBase bullet ) {
        if ( enemyBullets_.Count > curEnemyBulletNum_ ) {
            enemyBullets_[ curEnemyBulletNum_ ] = bullet;
        } else {
            enemyBullets_.Add( bullet );
        }
        curEnemyBulletNum_++;
    }

    void shootBullet() {
        if ( bullets_.Count == 0 ) {
            return;
        }
        var bullet = bullets_.Pop();
        bullet.setup( this );
        bullet.transform.SetParent( null );
        bullet.FinishCallback = () => {
            if ( this == null )
                return;
            bullet.transform.SetParent( bulletStockRoot_ );
            bullets_.Push( bullet );
        };
    }

    private void Awake() {
        for ( int i = 0; i < 150; ++i ) {
            bullets_.Push( PrefabUtil.createInstance( bulletPrefab_, bulletStockRoot_ ) );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // コントロール
        if ( Input.GetMouseButton( 0 ) == true ) {
            shootBullet();
        }

        // コリジョンチェック
        var penetration = new Vector2();
        var pos = transform.position;
        var pos2D = new Vector2( pos.x, pos.z );
        var block = fieldCollider_.toCircleCollide( pos2D, radius_, ref penetration );
        if ( block != null ) {
            pos.x -= penetration.x;
            pos.z -= penetration.y;
            transform.position = pos;
        }

        // 敵弾との衝突判定
        int n = curEnemyBulletNum_;
        int idx = 0;
        var pp = transform.position;
        for ( int i = 0; i < n; ++i ) {
            var e = enemyBullets_[ i ];
            if ( e == null ) {
                continue;   // 弾自体が消えたらしい
            }
            // 弾当たってる？
            float er = e.getRadius();
            var ep = e.transform.position;
            float len = ( er + radius_ );
            if ( ( ep - pp ).sqrMagnitude <= len * len ) {
                // 衝突により消滅
                Destroy( e.gameObject );
            } else {
                // まだ生きてる弾なので位置を詰めて再登録
                enemyBullets_[ idx ] = e;
                idx++;
            }
        }
        curEnemyBulletNum_ = idx;
    }

    BlockCollideManager fieldCollider_;
    Stack<PlayerBullet> bullets_ = new Stack<PlayerBullet>();
    List<EnemyBulletBase> enemyBullets_ = new List<EnemyBulletBase>();
    int curEnemyBulletNum_ = 0;
}
