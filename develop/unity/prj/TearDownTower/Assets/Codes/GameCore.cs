using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour {

	[SerializeField]
	Tower towerPrefab_;

	[SerializeField]
	GameObject root_;

	[SerializeField]
	Player player_;

	public class Param {
		public Tower.Param towerParam_ = new Tower.Param();
		public Player.Param playerParam_ = new Player.Param();
	}

	public void setup( Param param ) {
		param_ = param;
		tower_ = Instantiate<Tower>( towerPrefab_ );
		tower_.transform.parent = root_.transform;
		tower_.transform.localPosition = Vector3.zero;
		tower_.setup( param.towerParam_ );
		tower_.gameObject.SetActive( false );

		player_.setup( param.playerParam_, tower_ );

		state_ = new Intro( this );
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ( state_ != null )
			state_ = state_.update();
	}

	class Intro : State< GameCore > {
		public Intro( GameCore parent ) : base( parent ) {
		}
		protected override State innerInit() {
			parent_.tower_.gameObject.SetActive( true );
			parent_.tower_.start();

			parent_.tower_.AllBlockDeletedCallback = () => {
				setNextState( new TowerClear( parent_ ) );
			};
			return null;
		}
		protected override State innerUpdate() {
			return this;
		}
	}

	class TowerClear : State< GameCore > {
		public TowerClear( GameCore parent ) : base( parent ) {
		}
		protected override State innerUpdate() {
			return this;
		}
	}

	State state_;
	Param param_;
	Tower tower_;
}
