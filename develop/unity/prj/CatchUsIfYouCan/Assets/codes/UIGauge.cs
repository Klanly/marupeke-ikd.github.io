using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGauge : MonoBehaviour {

    [SerializeField]
    GameObject uiFrame_;

    [SerializeField]
    UnityEngine.UI.Image uiGauge_;

    [SerializeField]
    float gaugeMaxWidth_;

    public Gradient gaugeColor_;

    [Range( 0, 1 )]
    public float level_;

    private void OnValidate()
    {
        uiGauge_.color = gaugeColor_.Evaluate( level_ );
    }

    // ゲージレベルを設定
    public void setLevel( float level )
    {
        level_ = Mathf.Clamp01( level );
        uiGauge_.color = gaugeColor_.Evaluate( level_ );

        var sz = uiGauge_.rectTransform.sizeDelta;
        sz.x = gaugeMaxWidth_ * level_;
        uiGauge_.rectTransform.sizeDelta = sz;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
