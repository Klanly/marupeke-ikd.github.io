﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 積まれた牌のグループ成立チェック
// ルール：
// ・縦横3個以上並んだ同種牌（索子、筒子、萬子、同柄風、同柄役牌）をチェック
//
//     [5][4][3]
//  [1][1][1][1][2]
//     [3][1][3]
//        [1]
//
// ・縦横判定
//  ・挿し込んだ後にフィールの無いの全部の牌について3つ以上並ぶ同種の塊を縦横それぞれで判定
//  ・横検索が先。見た目の左上から、下段へと進行。縦検索は左上から下方向へ、右段と進行。
//  ・役作りの為の面子及び対子は検索順に判断される。
//
// ・削除後詰め
//  ・3個以上列は削除されるが、これは別種含めフィールド全体で同時に消える
//  ・同種で削除された牌の上に乗っていた牌は下に詰められる
//  ・詰めた後に再び縦横判定をして、成立している物があれば採用（連鎖）
//  ・これを繰り返して消す牌の組み合わせが無くなるまで続ける
//
// ・対子先行
//  ・対子枠が空いている時に3個以上並んだ同種牌が成立した時、対子判定が先行される
//  ・[1][2][1]などが見つかった場合は[1][1]の対子成立
//  ・刻子もしくは槓子が出来た場合、それらの枠がある場合はそちらが優先、枠が埋まっていて
//  　対子待ち状態の時は対子として扱われる（その刻子、槓子は破棄）
//
// ・順子、刻子、槓子なら成立
// ・面子が成立していない場合は破棄される
//
// ・特殊ケース
// ・連続した並びで成立している面子は全部採用
//  ① 4つ並び
//   [A][B][C][D] -> [A,B,C]、[B,C,D],[A,B,C,D]をチェック
//  ⑤ 5つ並び
//   [A][B][C][D][E] -> [A,B,C]、[B,C,D]、[C,D,E]、[A,B,C,D]、[B,C,D,E]をチェック
//   ただし[A,B,C,D]もしくは[B,C,D,E]が槓子だった場合は3個並びはチェックしない（暗刻にしない）
//  ⑥ 6つ並び（駒を横にして挿し込み）
//  [A][B][C][D][E][F] ->[A,B,C]、[B,C,D]、[C,D,E]、[D,E,F]、[A,B,C,D]、[B,C,D,E]、[C,D,E,F]をチェック
//   ただし槓子が一つでもあった場合は3個並びの暗刻判定を除く
//
//  
//
public class PaiGroupChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
