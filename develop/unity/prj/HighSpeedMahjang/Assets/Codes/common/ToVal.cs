using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 値変換

namespace ToVal
{
    public class Conv
    {
        // string -> int
        static public int toInt( string s, int init )
        {
            int r = init;
            int.TryParse( s, out r );
            return r;
        }

        // string -> float
        static public float toFloat(string s, float init )
        {
            float r = init;
            float.TryParse( s, out r );
            return r;
        }
    }
}