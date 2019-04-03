using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveGenerator
{
	public class Vector2
	{
		public Vector2() {

		}

		public Vector2( float x, float y ) {
			x_ = x;
			y_ = y;
		}

		public float x_ = 0.0f;
		public float y_ = 0.0f;

		// 加算
		public Vector2 add( ref Vector2 outVal, Vector2 r ) {
			outVal.x_ = x_ + r.x_;
			outVal.y_ = y_ + r.y_;
			return outVal;
		}

		// 減算
		public Vector2 sub(ref Vector2 outVal, Vector2 r) {
			outVal.x_ = x_ - r.x_;
			outVal.y_ = y_ - r.y_;
			return outVal;
		}

		// 乗算
		public Vector2 mul(ref Vector2 outVal, Vector2 r) {
			outVal.x_ = x_ * r.x_;
			outVal.y_ = y_ * r.y_;
			return outVal;
		}
		public Vector2 mul(ref Vector2 outVal, float r) {
			outVal.x_ = x_ * r;
			outVal.y_ = y_ * r;
			return outVal;
		}

		// 除算
		public Vector2 div(ref Vector2 outVal, Vector2 r) {
			outVal.x_ = x_ / r.x_;
			outVal.y_ = y_ / r.y_;
			return outVal;
		}
		public Vector2 div(ref Vector2 outVal, float r) {
			outVal.x_ = x_ / r;
			outVal.y_ = y_ / r;
			return outVal;
		}

		// 正規化
		public Vector2 normalize() {
			float len = Len;
			if (len == 0.0f)
				return new Vector2();
			return new Vector2( x_ / len, y_ / len );
		}

		// 長さ
		public float Len { get { return (float)Math.Sqrt( x_ * x_ + y_ * y_ ); } }
	}
}
