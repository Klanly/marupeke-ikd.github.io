using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {

	[SerializeField]
	EnemyHiyorimy hiyorimy_;

	public enum EnemyType {
		Hiyorimy,
	}

	public Enemy create( EnemyType enemyType ) {
		switch ( enemyType ) {
			case EnemyType.Hiyorimy:
				return Instantiate<EnemyHiyorimy>( hiyorimy_ );
		}
		return null;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
