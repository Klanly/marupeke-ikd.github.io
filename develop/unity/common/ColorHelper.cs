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
}
