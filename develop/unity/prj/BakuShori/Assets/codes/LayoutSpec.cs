using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// レイアウトスペック
public class LayoutSpec {
    // BombBox
    public BombBoxFactory.BonbBoxType bombBoxType_ = BombBoxFactory.BonbBoxType.Metal;
    public int bombBoxEntityStockNum_ = 5;
    public int bombBoxScrewHoleNum = 4;
    public int bombBoxScrewNum = 2;

    // GimicBox
    public int gimicBoxNum_ = 4;
    public int gimcBoxEntityStockNum_ = 3;
    public bool gimicBoxRandomType_ = true;         // ランダムに生成
    public List<GimicBoxFactory.BoxType> gimicBoxTypes_;    // ギミックボックスの種類を指定

    // Gimic
    public int gimicNum_ = 4;
    public bool gimicRandomType_ = true;            // ランダムに生成
    public List<GimicFactory.GimicType> gimicTypes_;    // ギミックの種類を指定

    public int seed_ = -1;
}
