using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
	[Inject]
	IOXInput input_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( input_.decide() == true ) {
			Debug.Log( input_.cursorPos() );
		}
    }
}
