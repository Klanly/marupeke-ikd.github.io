// Cube回転タイプ
public enum CubeRotationType : int {
    CRT_0 = 0,               // 0度
    CRT_Plus_90 = 90,        // +90度
    CRT_Plus_180 = 180,      // +180度
    CRT_Plus_270 = 270,      // +270度
    CRT_Minus_90 = -90,      // -90度
    CRT_Minus_180 = -180,    // -180度
    CRT_Minus_270 = -270,    // -270度
    CRT_NUM = 7
}

namespace CubeRotateUtil
{
    public class Util
    {
        static public int getRotateIndex( CubeRotationType rotType )
        {
            switch ( rotType ) {
                case CubeRotationType.CRT_0: return 0;
                case CubeRotationType.CRT_Plus_90: return 1;
                case CubeRotationType.CRT_Plus_180: return 2;
                case CubeRotationType.CRT_Plus_270: return 3;
                case CubeRotationType.CRT_Minus_90: return 4;
                case CubeRotationType.CRT_Minus_180: return 5;
                case CubeRotationType.CRT_Minus_270: return 6;
            }
            return -1;
        }

        static public CubeRotationType getInvRotType( CubeRotationType rotType )
        {
            switch ( rotType ) {
                case CubeRotationType.CRT_0: return CubeRotationType.CRT_0;
                case CubeRotationType.CRT_Plus_90: return CubeRotationType.CRT_Minus_90;
                case CubeRotationType.CRT_Plus_180: return CubeRotationType.CRT_Minus_180;
                case CubeRotationType.CRT_Plus_270: return CubeRotationType.CRT_Minus_270;
                case CubeRotationType.CRT_Minus_90: return CubeRotationType.CRT_Plus_90;
                case CubeRotationType.CRT_Minus_180: return CubeRotationType.CRT_Plus_180;
                case CubeRotationType.CRT_Minus_270: return CubeRotationType.CRT_Plus_270;
            }
            return CubeRotationType.CRT_0;
        }
    }
}