using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GameManagerBase
{
	[SerializeField]
	Player player_ = null;

	[SerializeField]
	UnityEngine.UI.Text distanceI_;
	[SerializeField]
	UnityEngine.UI.Text distanceF_;


	public void toGameOver()
	{
		bGameOver_ = true;
	}

	private void Awake()
	{
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
    }

	bool bGameOver_ = false;
}
