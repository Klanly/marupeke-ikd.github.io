using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ブロック情報
public class Block
{
	// ブロックタイプ
	public enum Type : byte
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

	public Block( int x, int y ) {
		data0_ = ( x & 0x3FF ) | ( ( y & 0x3FF ) << 12 );
	}
	public Block( Vector2Int idx ) {
		data0_ = ( idx.x & 0x3FF ) | ( ( idx.y & 0x3FF  ) << 12 );
	}

	public Block( Block.Type type, Vector2Int idx ) {
		type_ = type;
		data0_ = ( idx.x & 0x3FF ) | ( ( idx.y & 0x3FF ) << 12 );
	}
	public Block(Block.Type type, int x, int y) {
		type_ = type;
		data0_ = ( x & 0x3FF ) | ( ( y & 0x3FF ) << 12 );
	}

	public Vector2Int getIdx() {
		return new Vector2Int( data0_ & 0x3FF, ( data0_ >> 12 ) & 0x3FF );
	}

	// HP減少
	//  戻り値: HPをゼロにしたらtrue
	public bool damage( short dmg, bool autoChangeEmpty = false ) {
		if (isDestroy() == true)
			return false;	// 既に破壊されているブロックは追加ダメージを与えられない
		hp_ -= dmg;
		if ( hp_ <= 0 ) {
			hp_ = 0;
			if ( autoChangeEmpty == true )
                setDestroy();
			return true;
		}
		return false;
	}

	public bool isDestroy() {
		return ( data0_ & ( 1 << 24 ) ) != 0;
	}

	public void setDestroy() {
		data0_ |= ( 1 << 24 );
        type_ = Type.Empty;
		// 通知
		GameManager.getInstance().setBrokenBlockNum( 1 );
	}

	public Type type_ = Type.Empty;
	public short hp_ = 1;   // ブロックの耐久力
	int data0_ = 0;			// データ
								//  0-11: x座標(0～1023)
								// 12-23: y座標(0～1023)
								// 24   : destroyed
								// 25-31: reserved
}
