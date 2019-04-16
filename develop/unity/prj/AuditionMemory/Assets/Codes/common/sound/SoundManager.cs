using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public void playBGM( string bgmName, bool forceRestart = false ) {
   		if ( forceRestart == false && curBGMName_ == bgmName )
			return;
		if ( bgmMap_.ContainsKey( bgmName ) == false )
			return;
        bgmAudio_.PlayOneShot( bgmMap_[ bgmName ] );
		curBGMName_ = bgmName;
	}

	public void stopBGM() {
		if ( curBGMName_ != "" ) {
            bgmAudio_.Stop();
			curBGMName_ = "";
		}
	}

	public float playSE( string seName, float delaySec ) {
		if ( seMap_.ContainsKey( seName ) == false )
			return 0.0f;
        var clip = seMap_[ seName ];
        if ( delaySec == 0.0f ) {
            seAudio_.PlayOneShot( seMap_[ seName ] );
        }  else {
            GlobalState.wait( delaySec, () => {
                seAudio_.PlayOneShot( seMap_[ seName ] );
                return false;
            } );
        }
        return clip.length + delaySec;
    }

    public void addSE( string name, AudioClip clip ) {
        seMap_[ name ] = clip;
    }

    public void removeSE( string name ) {
        if ( seMap_.ContainsKey( name ) == true ) {
            seMap_.Remove( name );
        }
    }

    private void Awake() {
        seAudio_ = gameObject.AddComponent<AudioSource>();
        bgmAudio_ = gameObject.AddComponent<AudioSource>();
	}

	Dictionary<string, AudioClip> bgmMap_ = new Dictionary<string, AudioClip>();
	Dictionary<string, AudioClip> seMap_ = new Dictionary<string, AudioClip>();
	AudioSource curBGM_;
    AudioSource seAudio_;
    AudioSource bgmAudio_;
    string curBGMName_;
}

class SoundAccessor {
	public static SoundAccessor getInstance() {
        if ( accessor_.manager_ == null ) {
            GameObject manager = new GameObject( "Sound Manager" );
            accessor_.registerManager( manager.AddComponent<SoundManager>() );
        }
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

	public float playSE( string name, float delaySec = 0.0f ) {
		return manager_.playSE( name, delaySec );
	}

    public void loadSE( string filePath, string seName ) {
        ResourceLoader.getInstance().loadAsync<AudioClip>( filePath, (res, obj) => {
            manager_.addSE( seName, obj );
        } );
    }

    public void removeSE( string seName ) {
        manager_.removeSE( seName );
    }

	static SoundAccessor accessor_ = new SoundAccessor();
	SoundManager manager_;
}