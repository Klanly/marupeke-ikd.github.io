using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 様々なLerp

public class Lerps {

    // float
    public class Float
    {
        // 線形補間
        public static float linear( float s, float e, float t )
        {
            return s * ( e - s ) * t;
        }

        // EaseInOut
        public static float easeInOut( float s, float e, float t )
        {
            return s + ( e - s ) * t * t * ( 3.0f - 2.0f * t );
        }

        // EaseInOut(0-1)
        public static float easeInOut01(float t)
        {
            return t * t * ( 3.0f - 2.0f * t );
        }
    }

    // Vector3
    public class Vec3
    {
        // 線形補間
        public static Vector3 linear( Vector3 s, Vector3 e, float t )
        {
            return Vector3.Lerp( s, e, t );
        }

        // EaseInOut
        public static Vector3 easeInOut( Vector3 s, Vector3 e, float t )
        {
            return linear( s, e, Float.easeInOut01( t ) );
        }
    }

    // Quaternion
    public class Quaternion
    {
        // 線形補間
        public static UnityEngine.Quaternion linear(UnityEngine.Quaternion s, UnityEngine.Quaternion e, float t )
        {
            return UnityEngine.Quaternion.Lerp( s, e, t );
        }

        // EaseInOut
        public static UnityEngine.Quaternion easeInOut(UnityEngine.Quaternion s, UnityEngine.Quaternion e, float t)
        {
            return UnityEngine.Quaternion.Lerp( s, e, Float.easeInOut01( t ) );
        }
    }
}
