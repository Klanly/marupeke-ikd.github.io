using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロックフィールドのパラメータ
public class BlockFieldParameter
{
    public class Block {
        public int num_ = 5;
        public float interval_ = 0.2f;
        public float intervalForPlayer_ = 0.1f;
        public short HP_ = 100;
    }
    public Vector2 regionMin_ = Vector2.zero;
	public Vector2 regionMax_ = Vector2.one;
	public int sepX_ = 32;
	public int sepY_ = 32;
    public Vector2 playerPos_ = Vector2.zero;
    public Block diamond_ = new Block();
    public Block sapphire_ = new Block();
    public Block enemyBullet1_ = new Block();
    public Block enemyBullet2_ = new Block();
}
