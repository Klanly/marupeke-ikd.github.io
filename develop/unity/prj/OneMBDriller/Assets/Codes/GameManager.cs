using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pref_;

    // Start is called before the first frame update
    void Start()
    {
        var poses = RandomPlace.distanceBase( new Vector2( 0.0f, 0.0f ), new Vector2( 10.0f, 10.0f ), 0.2f, 1000 );
        foreach ( var p in poses ) {
            GameObject obj = Instantiate<GameObject>( pref_ );
            obj.transform.localPosition = p;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
