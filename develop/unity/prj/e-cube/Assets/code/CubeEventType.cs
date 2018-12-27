using System.Collections.Generic;

// 発行可能イベント一覧

public enum CubeEventType : int
{
    None,   // 何もしない

    // 簡便指定
    Rot_R = Rot_Right | RotCol_1 | RotDeg_90,
    Rot_L = Rot_Left  | RotCol_1 | RotDeg_90,
    Rot_D = Rot_Down  | RotCol_1 | RotDeg_90,
    Rot_U = Rot_Up    | RotCol_1 | RotDeg_90,
    Rot_F = Rot_Front | RotCol_1 | RotDeg_90,
    Rot_B = Rot_Back  | RotCol_1 | RotDeg_90,
    Rot_IR = Rot_Right | RotCol_1 | RotDeg_90 | Rot_Inv,
    Rot_IL = Rot_Left  | RotCol_1 | RotDeg_90 | Rot_Inv,
    Rot_ID = Rot_Down  | RotCol_1 | RotDeg_90 | Rot_Inv,
    Rot_IU = Rot_Up    | RotCol_1 | RotDeg_90 | Rot_Inv,
    Rot_IF = Rot_Front | RotCol_1 | RotDeg_90 | Rot_Inv,
    Rot_IB = Rot_Back  | RotCol_1 | RotDeg_90 | Rot_Inv,

    // 正回転 （正面に見て時計回り）
    Rot_Mask = 7,   // 正回転マスク
    Rot_Right = 1,
    Rot_Left  = 2,
    Rot_Down  = 3,
    Rot_Up    = 4,
    Rot_Front = 5,
    Rot_Back  = 6,

    // 逆回転 (正面に見て反時計回り)
    Rot_Inv_Mask = 15,  // 逆回転マスク
    Rot_Inv = 1 << 3,   // 逆回転フラグ
    Rot_Inv_Right = Rot_Right | Rot_Inv,
    Rot_Inv_Left  = Rot_Left  | Rot_Inv,
    Rot_Inv_Down  = Rot_Down  | Rot_Inv,
    Rot_Inv_Up    = Rot_Up    | Rot_Inv,
    Rot_Inv_Front = Rot_Front | Rot_Inv,
    Rot_Inv_Back  = Rot_Back  | Rot_Inv,

    // 回転列（同時指定があるのでビット）
    RotCol_All = 1 << 4,  // 全部の列を一度に
    RotCol_1  = 1 <<  5,  // 1列目
    RotCol_2  = 1 <<  6,  // 2列目
    RotCol_3  = 1 <<  7,  // 3列目
    RotCol_4  = 1 <<  8,  // 4列目
    RotCol_5  = 1 <<  9,  // 5列目
    RotCol_6  = 1 << 10,  // 6列目
    RotCol_7  = 1 << 11,  // 7列目
    RotCol_8  = 1 << 12,  // 8列目
    RotCol_9  = 1 << 13,  // 9列目
    RotCol_10 = 1 << 14,  // 10列目
    RotCol_11 = 1 << 15,  // 11列目
    RotCol_12 = 1 << 16,  // 12列目
    RotCol_13 = 1 << 17,  // 13列目
    RotCol_14 = 1 << 18,  // 14列目
    RotCol_15 = 1 << 19,  // 15列目
    RotCol_16 = 1 << 20,  // 16列目

    // 回転角度（回転で正・逆を指定しているが90、180度、270度を用意）
    // 回転は同時指定しないので数値指定
    RotDeg_Mask = 7 << 21,   // 回転角度マスク
    RotDeg_0   = 1 << 21,    //   0度
    RotDeg_90  = 2 << 21,    //  90度
    RotDeg_180 = 3 << 21,    // 180度
    RotDeg_270 = 4 << 21,    // 270度
}

public class EventUtil
{
    // 回転イベントを回転軸、角度、回転列に変換
    public static bool convEventToRotInfo( int n, CubeEventType eventType, out AxisType axis, out CubeRotationType rotType, out int[] colIndices )
    {
        axis = AxisType.AxisType_X;
        rotType = CubeRotationType.CRT_0;

        // 回転
        CubeEventType rot = eventType & CubeEventType.Rot_Mask;
        if ( ( int )rot == 0 ) {
            colIndices = new int[ 0 ] { };
            return false;   // 回転指定が無い
        }

        // 回転角度
        CubeEventType deg = eventType & CubeEventType.RotDeg_Mask;
        if ( deg == CubeEventType.RotDeg_0 ) {
            colIndices = new int[ 0 ] { };
            return false;   // 無回転
        }

        // 軸確定
        switch ( rot ) {
            case CubeEventType.Rot_Left:
            case CubeEventType.Rot_Right:
                axis = AxisType.AxisType_X;
                break;
            case CubeEventType.Rot_Down:
            case CubeEventType.Rot_Up:
                axis = AxisType.AxisType_Y;
                break;
            case CubeEventType.Rot_Front:
            case CubeEventType.Rot_Back:
                axis = AxisType.AxisType_Z;
                break;
        }

        // 回転列
        // L, D, F指定の場合はそのまま、R, U, Bの場合は反転が必要
        if ( eventType == CubeEventType.RotCol_All ) {
            colIndices = new int[ n ];
            for ( int i = 0; i < n; ++i )
                colIndices[ i ] = i;
        } else {
            CubeEventType[] cols = new CubeEventType[ 16 ] {
                CubeEventType.RotCol_1,
                CubeEventType.RotCol_2,
                CubeEventType.RotCol_3,
                CubeEventType.RotCol_4,
                CubeEventType.RotCol_5,
                CubeEventType.RotCol_6,
                CubeEventType.RotCol_7,
                CubeEventType.RotCol_8,
                CubeEventType.RotCol_9,
                CubeEventType.RotCol_10,
                CubeEventType.RotCol_11,
                CubeEventType.RotCol_12,
                CubeEventType.RotCol_13,
                CubeEventType.RotCol_14,
                CubeEventType.RotCol_15,
                CubeEventType.RotCol_16,
            };
            var indices = new List<int>();
            int idx = 0;
            foreach ( var c in cols ) {
                if ( ( int )( eventType & c ) != 0 ) {
                    indices.Add( idx );
                }
                idx++;
            }
            if ( indices.Count == 0 ) {
                // 回転列指定がされていない
                colIndices = new int[ 0 ] { };
                return false;   // 回転指定が無い
            }
            if ( rot == CubeEventType.Rot_Right || rot == CubeEventType.Rot_Up || rot == CubeEventType.Rot_Back ) {
                for ( int i = 0; i < indices.Count; ++i ) {
                    indices[ i ] = n - 1 - indices[ i ];
                }
            }
            colIndices = indices.ToArray();
        }

        // 回転方向
        CubeRotationType[] rotationTypes = new CubeRotationType[ 2 ];
        switch ( deg ) {
            case CubeEventType.RotDeg_90:
                rotationTypes[ 0 ] = CubeRotationType.CRT_Plus_90;
                rotationTypes[ 1 ] = CubeRotationType.CRT_Minus_90;
                break;
            case CubeEventType.RotDeg_180:
                rotationTypes[ 0 ] = CubeRotationType.CRT_Plus_180;
                rotationTypes[ 1 ] = CubeRotationType.CRT_Minus_180;
                break;
            case CubeEventType.RotDeg_270:
                rotationTypes[ 0 ] = CubeRotationType.CRT_Plus_270;
                rotationTypes[ 1 ] = CubeRotationType.CRT_Minus_270;
                break;
        }
        bool isInv = ( ( int )( eventType & CubeEventType.Rot_Inv ) != 0 );
        rotType = rotationTypes[ isInv ? 0 : 1 ];

        return true;
    }
}