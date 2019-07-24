using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロックフィールドのパラメータ
public class BlockFieldParameter
{
	public Vector2 regionMin_ = Vector2.zero;
	public Vector2 regionMax_ = Vector2.one;
	public int sepX_ = 32;
	public int sepY_ = 32;
    public int diamondNum_ = 5;
}
