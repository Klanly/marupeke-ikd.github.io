using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	public Vector3 Dir { set { dir_ = value; } }
	public float Speed { set { speed_ = value; } }

    void Start()
    {
        
    }

    void Update()
    {
		t_ += Time.deltaTime;
		transform.position = dir_ * speed_ * t_;        
    }

	Vector3 dir_ = Vector3.zero;
	float speed_ = 0.0f;
	float t_ = 0.0f;
}
