using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 人排出ルール
//  朝8～9時に出勤ラッシュで多く人を排出
//  9時以降は落ち着くが12時に少しだけ多くなる。
//  12～17時までは少ない。
//  17～20時まで長く帰宅ラッシュ（最大排出）
//  20時以降はパラパラと。

//  朝方はランニングしている人がちらほら
//  お昼にはランニングする人はいない
//  18時頃からランニングする人が増える。
//  夜中にランニングする人はちらちらといる。

public class HumanRule : PassengerRule
{
}
