using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 波の基底クラス

namespace WaveGenerator
{
	public class Wave
	{
		public enum EWaveType
		{
			None,				// 無し
			SinRipple,			// sinの環状波
			SinStraignt,		// sinの直進波
			TrochoidRipple,		// トロコイド環状波
			TrochoidStraight,	// トロコイド直進波
		}

		public EWaveType WaveType { get { return waveType_; } }
		public float Time { get { return sec_; } }
		public Wave() {

		}

		// 時刻を設定
		public void setTime(float sec) {
			sec_ = sec;
		}

		// ワールドに波を加算
		virtual public void addMe(World world) {
		}

		// クローン作成
		virtual public Wave clone() {
			return null;
		}

		// アクティブ？
		public bool Active { get { return bActive_; } set { bActive_ = value; } }

		protected float sec_ = 0.0f;
		protected bool bActive_ = true;
		protected EWaveType waveType_ = EWaveType.None;
	}
}
