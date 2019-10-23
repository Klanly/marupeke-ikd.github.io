using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : GameManagerBase
{
	[SerializeField]
	Player player_ = null;

	[SerializeField]
	UnityEngine.UI.Text distanceI_;
	[SerializeField]
	UnityEngine.UI.Text distanceF_;

	[SerializeField]
	UnityEngine.UI.Image gameOver_;

	[SerializeField]
	UnityEngine.UI.Button retryBtn_;


	public void toGameOver()
	{
		bGameOver_ = true;
		gameOver_.gameObject.SetActive( true );
		retryBtn_.gameObject.SetActive( true );
	}

	private void Awake()
	{
		gameOver_.gameObject.SetActive( false );
		retryBtn_.gameObject.SetActive( false );
		retryBtn_.onClick.AddListener( () => {
			retryBtn_.enabled = false;
			SceneManager.LoadScene( "game" );
		} );
	}

	void Start()
    {
        
    }

    void Update()
    {
		float m = player_.getCurDistance();
		int intM = ( int )m;
		distanceI_.text = string.Format( "{0}.", intM );
		distanceF_.text = string.Format( "{0}", ( int )( 100 * ( m - intM ) ) );

		stateUpdate();

		if ( bGameOver_ == true ) {
			if ( Input.GetKeyDown( KeyCode.Z ) ) {
				retryBtn_.enabled = false;
				SceneManager.LoadScene( "game" );
			}
		}
    }

	bool bGameOver_ = false;
}
