﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceType : int
{
    FaceType_Left = 0,
    FaceType_Right = 1,
    FaceType_Down = 2,
    FaceType_Up = 3,
    FaceType_Front = 4,
    FaceType_Back = 5,
    FaceType_Num = 6,
    FaceType_None = -1,  // 内側
}


namespace FaceTypeUtil
{
    public class Util
    {
        public static string getFaceShortName( FaceType face )
        {
            return shortNames_[ (int)face + 1 ];
        }
        static List<string> shortNames_ = new List<string> { "", "L", "R", "D", "U", "F", "B", "" };
    }
}