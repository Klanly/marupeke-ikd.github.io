using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDesc : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Button btn_;

    [SerializeField]
    RectTransform stockPos_;

    private void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        rectTransform_ = GetComponent<RectTransform>();
        initPos_ = rectTransform_.position;
        btn_.onClick.AddListener( () => {
            bShow_ = !bShow_;
            var curPos = rectTransform_.position;
            if ( bShow_ == true ) {
                GlobalState.time( 0.75f, (sec, t) => {
                    rectTransform_.position = Lerps.Vec3.easeOut( curPos, initPos_, t );
                    return true;
                } );
            } else {
                GlobalState.time( 0.75f, (sec, t) => {
                    rectTransform_.position = Lerps.Vec3.easeOut( curPos, stockPos_.position, t );
                    return true;
                } );
            }
        } );	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    RectTransform rectTransform_;
    Vector3 initPos_;
    bool bShow_ = true;
}
