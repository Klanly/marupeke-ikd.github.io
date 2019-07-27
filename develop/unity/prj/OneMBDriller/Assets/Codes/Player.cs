using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float radius_ = 1.0f;

    public void setup( BlockCollideManager fieldCollider ) {
        fieldCollider_ = fieldCollider;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
}
