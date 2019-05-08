using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteAccPanel : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Image check_;

    [SerializeField]
    UnityEngine.UI.Button siteAccBtn_;

    [SerializeField]
    UnityEngine.UI.Text btnText_;

    public UnityEngine.UI.Button SiteAccBtn { get { return siteAccBtn_; } }

    public void setup( SiteManager siteManager ) {
        siteManager_ = siteManager;
        siteManager_.setActive( false );    // 初期状態はサイトは不活性で
    }

    public void setCheck( bool isCheck ) {
        check_.gameObject.SetActive( isCheck );
    }

    public void setBtnText( string text ) {
        btnText_.text = text;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    SiteManager siteManager_;
}
