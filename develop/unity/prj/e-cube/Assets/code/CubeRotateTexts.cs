using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 回転テキスト送り

public class CubeRotateTexts {

    public void setup( CubePracticeData data, UnityEngine.UI.Text text, UnityEngine.UI.Image bg )
    {
        text_ = text;
        var list = data.getRotateList();
        foreach ( var r in list ) {
            codes_.Add( createCode( r ) );
        }
        setIdx( -1 );

        var delta = text_.rectTransform.sizeDelta;
        delta.y = ( list.Count / 8 + 1 ) * text_.fontSize;
        text_.rectTransform.sizeDelta = delta;
        bg.rectTransform.sizeDelta = delta;
    }

    public bool setIdx( int idx )
    {
        // 指定idx番目のcodeに色を付ける
        // 8コードで改行
        string codeText = "";
        for ( int i = 0; i < codes_.Count; ++i ) {
            if ( i == idx ) {
                codeText += string.Format( "<color=#ff0022>{0}</color>", codes_[ i ] );
            } else {
                codeText += codes_[ i ]; 
            }
            if ( i % 8 == 7 )
                codeText += "\n";
            else
                codeText += " ";
        }
        text_.text = codeText;

        return ( idx >= 0 && idx < codes_.Count );
    }

    protected virtual string createCode( CubePracticeData.RotateUnit r )
    {
        if ( r.colIndices_.Count >= 2 ||
            ( r.colIndices_.Count == 1 && r.colIndices_[ 0 ] != 0 )
        ) {
            return r.getRotateCode();
        }
        return FaceTypeUtil.Util.getFaceShortName( r.face_ ) + r.getRotDirSymbolMark();
    }

    List<string> codes_ = new List<string>();
    CubePracticeData data_;
    UnityEngine.UI.Text text_;
}
