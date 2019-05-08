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
            return s + ( e - s ) * t;
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

        // EaseOut(0-1)
        public static float easeOut01(float t)
        {
            return t * ( 2.0f - t );
        }

        // EaseOutStrong(0-1)
        public static float easeOut01Strong(float t)
        {
            return easeOut01( t * t );
        }

        // EaseIn(0-1)
        public static float easeIn01(float t)
        {
            return t * t;
        }

        // EaseInStrong(0-1)
        public static float easeIn01Strong(float t)
        {
            return easeIn01( t * t );
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

        // EaseOut
        public static Vector3 easeOut( Vector3 s, Vector3 e, float t)
        {
            return linear( s, e, Float.easeOut01( t ) );
        }

        // EaseOutStrong
        public static Vector3 easeOutStrong(Vector3 s, Vector3 e, float t)
        {
            return linear( s, e, Float.easeOut01Strong( t ) );
        }

        // EaseIn
        public static Vector3 easeIn(Vector3 s, Vector3 e, float t)
        {
            return linear( s, e, Float.easeIn01( t ) );
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

        // EaseOut
        public static UnityEngine.Quaternion easeOut(UnityEngine.Quaternion s, UnityEngine.Quaternion e, float t)
        {
            return UnityEngine.Quaternion.Lerp( s, e, Float.easeOut01( t ) );
        }

        // EaseIn
        public static UnityEngine.Quaternion easeIn(UnityEngine.Quaternion s, UnityEngine.Quaternion e, float t)
        {
            return UnityEngine.Quaternion.Lerp( s, e, Float.easeIn01( t ) );
        }
    }
}
