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

    public BlockCollideManager getBlockCollideManager() {
        return fieldCollider_;
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
    }

    BlockCollideManager fieldCollider_;
    Stack<PlayerBullet> bullets_ = new Stack<PlayerBullet>();
}
