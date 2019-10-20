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
		float interval = 20.0f;
		float p = 1.0f / interval;
		int tryCount = 1000;
		int unitCount = 2000;
		float exEncountNum = 2000 / interval;
		int[] resCount = new int[ tryCount ];

		for ( int i = 0; i < tryCount; ++i ) {
			for ( int j = 0; j < unitCount; ++j ) {
				if ( Random.value < p ) {
					resCount[ i ]++;
				}
			}
		}

		float ave = 0.0f;
		for ( int i = 0; i < tryCount; ++i ) {
			ave += resCount[ i ];
		}
		ave /= tryCount;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
