using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロック情報
public class Block
{
	// ブロックタイプ
	public enum Type : uint
	{
		Empty = 0,

		Lock_UnBreak = 1,
		Lock_Wall = 2,
		Lock0 = 3,
		Lock1 = 4,
		Lock2 = 5,
		Lock3 = 6,
		Lock4 = 7,
		Lock5 = 8,
		Lock6 = 9,
		Lock7 = 10,
		Lock8 = 11,
		Lock9 = 12,

		Juel0 = 17,
		Juel1 = 18,
		Juel2 = 19,
		Juel3 = 20,
		Juel4 = 21,

		Trap0 = 64,
		Trap1 = 65,
		Trap2 = 66,
		Trap3 = 67,
		Trap4 = 68,
		Trap5 = 69,
		Trap6 = 70,
		Trap7 = 71,
		Trap8 = 72,
		Trap9 = 73,
	}

	public Block() { }

	public Block( Block.Type type ) {
		type_ = type;
	}

	public Type type_ = Type.Empty;
	public short hp_;		// ブロックの耐久力
}
