using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField]
	List< AudioSource > BGMs_;

	[SerializeField]
	List<string> BGMNames_;

	[SerializeField]
	List<AudioSource> SEs_;

	[SerializeField]
	List<string> SENames_;

	public void playBGM( string bgmName, bool forceRestart = false ) {
		if ( forceRestart == false && curBGMName_ == bgmName )
			return;
		if ( bgmMap_.ContainsKey( bgmName ) == false )
			return;
		bgmMap_[ bgmName ].Play();
		stopBGM();
		curBGM_ = bgmMap_[ bgmName ];
		curBGMName_ = bgmName;
	}

	public void stopBGM() {
		if ( curBGM_ != null ) {
			curBGM_.Stop();
			curBGM_ = null;
			curBGMName_ = "";
		}
	}

	public void playSE( string seName, float delaySec ) {
		if ( seMap_.ContainsKey( seName ) == false )
			return;
		if ( delaySec == 0.0f )
			seMap_[ seName ].Play();
		else {
			GlobalState.wait( delaySec, () => {
				seMap_[ seName ].Play();
				return false;
			} );
		}
	}

	private void Awake() {
		for ( int i = 0; i < BGMs_.Count; ++i ) {
			bgmMap_[ BGMNames_[ i ] ] = BGMs_[ i ];
		}
		for ( int i = 0; i < SEs_.Count; ++i ) {
			seMap_[ SENames_[ i ] ] = SEs_[ i ];
		}
		SoundAccessor.getInstance().registerManager( this );
	}

	Dictionary<string, AudioSource> bgmMap_ = new Dictionary<string, AudioSource>();
	Dictionary<string, AudioSource> seMap_ = new Dictionary<string, AudioSource>();
	AudioSource curBGM_;
	string curBGMName_;
}

class SoundAccessor {
	public static SoundAccessor getInstance() {
		return accessor_;
	}

	public void registerManager( SoundManager manager ) {
		manager_ = manager;
	}

	public void playBGM( string name ) {
		manager_.playBGM( name );
	}

	public void stopBGM() {
		manager_.stopBGM();
	}

	public void playSE( string name, float delaySec = 0.0f ) {
		manager_.playSE( name, delaySec );
	}

	static SoundAccessor accessor_ = new SoundAccessor();
	SoundManager manager_;
}