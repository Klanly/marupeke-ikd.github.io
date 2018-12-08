using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ComboText : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text combText_;

    [SerializeField]
    UnityEngine.UI.Text combScore_;

    public void setup( int comboCount, int addScore, Bonus bonus )
    {
        string str = "";
        if ( bonus == Bonus.Bonus_No )
            str = "{0} COMBO";
        else if ( bonus == Bonus.Bonus_Quick )
            str = "{0} COMBO QUICK!";
        else if ( bonus == Bonus.Bonus_Limit)
            str = "{0} COMBO LIMIT!!!";
        combText_.text = string.Format( str, comboCount );
        combScore_.text = string.Format( "+{0}", addScore );
    }

    private void Awake()
    {
        var color = combText_.color;
        color.a = 0;
        combText_.color = color;

        color = combScore_.color;
        color.a = 0;
        combScore_.color = color;
    }

    void seq(UnityEngine.UI.Text text, float delayTime )
    {
        DOVirtual.DelayedCall( delayTime, () => {
            text.rectTransform.localRotation = Quaternion.Euler( 0.0f, 0.0f, -20.0f );
            text.rectTransform.localScale = new Vector3( 2.0f, 2.0f, 2.0f );

            Sequence sq = DOTween.Sequence();
            sq.Append( DOTween.ToAlpha( () => text.color, c => text.color = c, 1.0f, 0.15f ) );
            sq.Append( DOTween.ToAlpha( () => text.color, c => text.color = c, 1.0f, 0.35f ) );
            sq.Append( DOTween.ToAlpha( () => text.color, c => text.color = c, 0.0f, 0.35f ) );

            Sequence sqTrans = DOTween.Sequence();
            sqTrans.Append( text.rectTransform.DORotate( new Vector3( 0.0f, 0.0f, 0.0f ), 0.15f ) );

            Sequence sqScale = DOTween.Sequence();
            sqScale.Append( text.rectTransform.DOScale( 1.0f, 0.35f ) );
        } );
    }

    // Use this for initialization
    void Start () {
        seq( combText_, 0.0f );
        seq( combScore_, 0.10f );
        GameObject.Destroy( gameObject, 2.0f );
    }

    // Update is called once per frame
    void Update () {
		
	}
}
