﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firecracker1 : MonoBehaviour {

    public Rigidbody rig;
    public ConstantForce cf;
    public Transform IsKinematic;

    IEnumerator Start()

    {
        //Wait for 3 secs.
        yield return new WaitForSeconds(7);

        //Game object will turn off
        GameObject.Find("MeshRenderer1").SetActive(false);

		if ( rig != null )
	        rig.isKinematic = true;
		if ( cf != null )
	        cf.enabled = false;


    }
}