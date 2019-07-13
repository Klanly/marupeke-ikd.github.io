using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlace : MonoBehaviour
{
    [SerializeField]
    Card card_;

    public void setParam( Card.Param param ) {
        card_.setParam( param );
    }

    public Card.Param getParam() {
        return card_.getParam();
    }

    public void setActive( bool isActive ) {
        card_.gameObject.SetActive( isActive );
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
