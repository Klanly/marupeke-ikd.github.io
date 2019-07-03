using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurceLoaderUpdater : MonoBehaviour {
    private void Awake()
    {
        DontDestroyOnLoad( this );
    }

    private void Update()
    {
        ResourceLoader.getInstance().update();
    }
}
