using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorWindowFrame : MonoBehaviour
{
    [SerializeField]
    WindowFrame frame_;

    [SerializeField]
    MultiMeshText text_;

    public void setStr( string str ) {
        text_.clear();
        text_.setStr( 0, str );
        text_.updateAll();
        text_.gameObject.SetActive( false );
        var s = new Vector2( 1.0f, 2.0f );
        var e = new Vector2( 8.0f, 2.0f );
        GlobalState.time( 0.25f, (sec, t) => {
            frame_.setSize( Lerps.Vec2.easeOut( s, e, t ) );
            return true;
        } ).finish(()=> {
            text_.gameObject.SetActive( true );
        } );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
