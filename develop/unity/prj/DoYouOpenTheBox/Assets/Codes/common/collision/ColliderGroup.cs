using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// コリジョングループ
//【宿題！！】
//  Colliderのグループを作ってお互いに衝突判定できるようにする！
public class ColliderGroup : MonoBehaviour
{
/*
    [SerializeField]
    List<Collider> colliders_;     // Colliderグループ

    [SerializeField]
    bool bAutoCollect_ = false;     // 子ノード内のColliderを自動的に集めてグループに所属させる？

    void Start()
    {
        // Colliderグループ自動生成
        if ( bAutoCollect_ == true ) {
            GameObjectUtil.getChildrenComponents( gameObject, ref colliders_, true );
        }

        // 一時近似球コライダー作成
        if ( colliders_.Count > 0 ) {
            firstCollider_ = new SphereCollider();
            var bounds = new Bounds();
            for ( int i = 1; i < colliders_.Count; ++i ) {
                bounds.Encapsulate( colliders_[ i ].bounds );
            }
            firstCollider_ = new SphereCollider();
            firstCollider_.center = bounds.center;
            firstCollider_.radius = bounds.size.magnitude / 2.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    SphereCollider firstCollider_;
*/
}
