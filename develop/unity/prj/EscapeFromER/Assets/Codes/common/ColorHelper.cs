using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カラーヘルパー
public class ColorHelper {
    // カラー取得
    public static Color getColor( uint hex ) {
        return new Color(
            ( hex >> 24 ) / 255.0f,
            ( ( hex >> 16 ) & 0xff ) / 255.0f,
            ( ( hex >>  8 ) & 0xff ) / 255.0f,
            ( hex & 0xff ) / 255.0f
        );
    }

    // カラー取得
    public static Color getColor( int r, int g, int b, int a ) {
        return new Color( r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f );
    }
}
