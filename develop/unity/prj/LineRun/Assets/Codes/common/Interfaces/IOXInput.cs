using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 仮装入力デバイスインターフェース
//  キーパッドやカーソル位置等を抽象化します
//  デジタル入力ボタン：RR, RL, RT, RB, LR, LL, LT, LB, L1, L2, R1, R2
//  アナログ入力ボタン：AL, AR
//  カーソル情報      ：スクリーン座標（左下原点）

public class IOXInput {
	public enum  Digital {
		RR,
		RL,
		RU,
		RD,
		LR,
		LL,
		LU,
		LD,
		L1,
		L2,
		R1,
		R2
	}
	public enum Analog {
		AL,
		AR
	}

	// キー変更オブジェクト
	public class Wired {
		public Digital this[ Digital d ] {
			get { return wireKeys_[ d ]; }
		}

		public Wired()
		{
			setDefault();
		}

		// 元に戻す
		public void setDefault()
		{
			foreach (var e in System.Enum.GetValues( typeof( Digital ) )) {
				wireKeys_[ (Digital)e ] = (Digital)e;
			}
		}

		// キーアサインを変更する
		//  pressedBtn    : 押したキー
		//  targetBtn     : ターゲットキー
		public void wire(Digital pressedBtn, Digital targetBtn )
		{
			wireKeys_[ pressedBtn ] = targetBtn;
		}
		private Dictionary<Digital, Digital> wireKeys_ = new Dictionary<Digital, Digital>();

		// コピー
		public void copy( Wired wired )
		{
			foreach ( var e in wired.wireKeys_ ) {
				wireKeys_[ e.Key ] = e.Value;
			}
		}
	}

	// Wiredを登録
	public void registerWired( Wired wired )
	{
		wired_ = wired;
	}

	// Wiredを取得
	public Wired getWired()
	{
		return wired_;
	}

	// 丁度Downした？
	public bool justDown(Digital d) {
		return innerJustDown( wired_[ d ] );
	}

	// 丁度Upした？
	public bool justUp(Digital d)
	{
		return innerJustUp( wired_[ d ] );
	}

	public bool holdDown( Digital d )
	{
		return innerHoldDown( wired_[ d ] );
	}

	public bool holdUp(Digital d)
	{
		return innerHoldUp( wired_[ d ] );
	}

	// 決定ボタン押した？
	public virtual bool decide()
	{
		// デフォルトはRD及びマウスLクリックにします。変更があればWiredで変更するか派生クラスで。
		return justDown( Digital.RD ) || Input.GetMouseButtonDown( 0 );
	}

	// カーソル位置を取得
	public virtual Vector2 cursorPos()
	{
		return Input.mousePosition;
	}

	// 方向ベクトルを取得
	public Vector2 getDirection()
	{
		Vector2 dir = Vector2.zero;
		if ( holdDown( Digital.LL ) == true ) {
			dir.x = -1.0f;
		} else if (holdDown( Digital.LR ) == true ) {
			dir.x = 1.0f;
		}
		if (holdDown( Digital.LU ) == true) {
			dir.y = 1.0f;
		} else if (holdDown( Digital.LD ) == true) {
			dir.y = -1.0f;
		}
		return dir.normalized;
	}

	// 丁度Downしたか
	protected virtual bool innerJustDown( Digital d )
	{
		return false;
	}

	// 丁度Upしたか
	protected virtual bool innerJustUp(Digital d)
	{
		return false;
	}

	// Down中か
	protected virtual bool innerHoldDown(Digital d)
	{
		if ( d == Digital.LL && Input.GetKey(KeyCode.LeftArrow) ) {
			return true;
		} else if ( d == Digital.LR && Input.GetKey(KeyCode.RightArrow) ) {
			return true;
		} else if ( d == Digital.LU && Input.GetKey(KeyCode.UpArrow) ) {
			return true;
		} else if ( d == Digital.LD && Input.GetKey(KeyCode.DownArrow) ) {
			return true;
		}
		return false;
	}

	// Up中したか
	protected virtual bool innerHoldUp(Digital d)
	{
		if (d == Digital.LL && !Input.GetKey( KeyCode.LeftArrow )) {
			return true;
		} else if (d == Digital.LR && !Input.GetKey( KeyCode.RightArrow )) {
			return true;
		} else if (d == Digital.LU && !Input.GetKey( KeyCode.UpArrow )) {
			return true;
		} else if (d == Digital.LD && !Input.GetKey( KeyCode.DownArrow )) {
			return true;
		}
		return true;
	}

	Wired wired_ = new Wired();
}
