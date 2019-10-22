using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEmitter : MonoBehaviour
{
	[SerializeField]
	Block blockPrefab_;

	[SerializeField]
	Block blockPrefab2_;

	[SerializeField]
	Player player_;

	[SerializeField]
	Field field_;

	[SerializeField]
	float averageInterval_ = 10.0f;

	[SerializeField]
	float minWidth_ = 1.0f;

	[SerializeField]
	float maxWidth_ = 1.5f;

	[SerializeField]
	float minHeight_ = 1.0f;

	[SerializeField]
	float maxHeight_ = 1.5f;


	// ブロック出現平均間隔(sec)
	public float AverageInterval {
		set {
			averageInterval_ = value;
			N_ = averageInterval_ / Time.deltaTime;
			p_ = 1.0f / N_;
		} get {
			return averageInterval_;
		}
	}

	private void Awake()
	{
	}

	void Update()
    {
		AverageInterval = averageInterval_;

		if (averageInterval_ <= 0.0f) {
			return;
		}

		if (Random.value < p_) {
			// Playerの後方にエミット
			var pp = player_.transform.position;
			var block = PrefabUtil.createInstance( Random.Range( 0, 2 ) == 0 ? blockPrefab_ : blockPrefab2_, transform );
			block.emit( Random.Range( minWidth_, maxWidth_ ), 1.0f, Random.Range( minHeight_, maxHeight_ ), 0.0f );
			block.transform.position = new Vector3( Randoms.Float.valueCenter() * field_.Width,	pp.y - 1.0f, 0.0f );
			block.transform.rotation = Quaternion.Euler( 0.0f, 0.0f, Randoms.Float.valueCenter() * 45.0f );
		}
	}

	float N_ = 0.0f;
	float p_ = 0.0f;
}
