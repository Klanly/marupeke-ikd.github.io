using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 特定アクション補間

public class LerpAction {
    // 溜めて「ピョーン」とジャンプして衝撃吸収着地
    //  g             : 重力加速度。9.8で地球と同じ位
    //  buildUpTime   : 溜め時間。0以上
    //  sinkDist      : 溜める時に沈み込む距離。0以上
    //  getDownHeight : 着地地点の高さ。0以上
    //  overDist      : 着地地点からジャンプの最高到達高さまでの差分高。0以上。
    //  underDist     : 着地してから沈み込む距離。0以上
    //  t             : 経過時刻。0以上。アクション終了時刻は
    //  calcFinishTime: アクション終了時間を返す場合にtrue、実際の高さを計算する場合はfalse。
    //  戻り値：高さ
    public static float jump( float g = 9.8f, float buildUpTime = 0.3f, float sinkDist = 0.2f, float getDownHeight = 1.0f, float overDist = 0.3f, float underDist = 0.05f, float t = 0.0f, bool calcFinishTime = false ) {
        if ( g <= 0.0f )
            return 0.0f;

        float F = getDownHeight;
        float D = sinkDist;
        float Of = overDist;
        float Uf = underDist;
        float H = D + F + Of;
        float tD = buildUpTime;
        float tJ = Mathf.Sqrt( 2.0f * H / g ) + Mathf.Sqrt( 2.0f * Of / g );
        float G = -g * tJ + Mathf.Sqrt( 2.0f * g * H );
        bool noJump = H == 0.0f && tJ == 0.0f;
        float tB = 0.0f;
        if ( noJump == false ) {
            tB = -2.0f * Uf / G;
        }
        if ( calcFinishTime == true ) {
            return buildUpTime + tJ + tB * 2.0f;
        }

        // 0 - buidUpTime
        if ( tD > 0.0f && t >= 0.0f && t < buildUpTime ) {
            //  沈み込み（2次関数）
            return D / ( tD * tD ) * ( t - tD ) * ( t - tD ) - D;

        // buildUpTime - getDownTime (tD + tJ )
        } else if ( t >= buildUpTime && t < tD + tJ ) {
            // ジャンプ（2次関数）
            return -g / 2.0f * ( t - tD ) * ( t - tD ) + Mathf.Sqrt( 2.0f * g * H ) * ( t - tD ) - D;

        } else if ( Uf <= 0.0f ) {
            return F;   // 吸収無し着地

        } else if ( t >= tD + tJ && t < tD + tJ + tB ) {
            // 着地後吸収（2次関数）
            float x = t - tD - tJ;
            return F + G * G / ( 4.0f * Uf ) * x * x + G * x;

        } else if ( t >= tD + tJ + tB && t < tD + tJ + tB * 2.0f ) {
            // 吸収後立ち上がり（3次関数）
            float x = ( t - tD - tJ - tB ) / tB;
            return Uf * x * x * ( 3.0f - 2.0f * x ) + F - Uf;
        }

        // 着地済み
        return F;
    }

    // ちょっと飛び上がって下に飛び降りて衝撃吸収着地
    //  g             : 重力加速度。9.8で地球と同じ位
    //  buildUpTime   : 溜め時間。0以上
    //  sinkDist      : 溜める時に沈み込む距離。0以上
    //  getDownHeight : 着地地点の高さ。0以下
    //  overDist      : ゼロ地点からジャンプの最高到達高さまでの差分高。0以上。
    //  underDist     : 着地してから沈み込む距離。0以上
    //  t             : 経過時刻。0以上。アクション終了時刻は
    //  calcFinishTime: アクション終了時間を返す場合にtrue、実際の高さを計算する場合はfalse。
    //  戻り値：高さ
    public static float jumpDown(float g = 9.8f, float buildUpTime = 0.3f, float sinkDist = 0.2f, float getDownHeight = -1.0f, float overDist = 0.3f, float underDist = 0.05f, float t = 0.0f, bool calcFinishTime = false) {
        if ( g <= 0.0f )
            return 0.0f;

        float F = -getDownHeight;
        float D = sinkDist;
        float Of = overDist;
        float Uf = underDist;
        float H = D + Of;
        float tD = buildUpTime;
        float v0 = Mathf.Sqrt( 2.0f * g * H );
        float tJ = ( v0 + Mathf.Sqrt( v0 * v0 + 2.0f * ( F - D ) * g ) ) / g;
        float G = -g * tJ + v0;
        bool noJump = H == 0.0f && tJ == 0.0f;
        float tB = 0.0f;
        if ( noJump == false ) {
            tB = -2.0f * Uf / G;
        }
        if ( calcFinishTime == true ) {
            return buildUpTime + tJ + tB * 2.0f;
        }

        // 0 - buidUpTime
        if ( tD > 0.0f && t >= 0.0f && t < buildUpTime ) {
            //  沈み込み（2次関数）
            return D / ( tD * tD ) * ( t - tD ) * ( t - tD ) - D;

            // buildUpTime - getDownTime (tD + tJ )
        } else if ( t >= buildUpTime && t < tD + tJ ) {
            // ジャンプ下り（2次関数）
            return -g / 2.0f * ( t - tD ) * ( t - tD ) + Mathf.Sqrt( 2.0f * g * H ) * ( t - tD ) - D;

        } else if ( Uf <= 0.0f ) {
            return -F;   // 吸収無し着地

        } else if ( t >= tD + tJ && t < tD + tJ + tB ) {
            // 着地後吸収（2次関数）
            float x = t - tD - tJ;
            return -F + G * G / ( 4.0f * Uf ) * x * x + G * x;

        } else if ( t >= tD + tJ + tB && t < tD + tJ + tB * 2.0f ) {
            // 吸収後立ち上がり（3次関数）
            float x = ( t - tD - tJ - tB ) / tB;
            return Uf * x * x * ( 3.0f - 2.0f * x ) - F - Uf;
        }

        // 着地済み
        return -F;
    }
}
