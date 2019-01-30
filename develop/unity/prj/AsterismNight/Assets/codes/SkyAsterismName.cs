using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyAsterismName : MonoBehaviour {

    [SerializeField]
    TextMesh nameJp_;

    [SerializeField]
    TextMesh nameEn_;

    public void setup( int astId )
    {
        var data = Table_asterism_ast.getInstance().getData( astId - 1 );
        nameJp_.text = data.jpName_;
        nameEn_.text = data.name_;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
