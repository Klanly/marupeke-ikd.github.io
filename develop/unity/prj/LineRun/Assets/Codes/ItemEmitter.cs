using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEmitter : MonoBehaviour
{
	[SerializeField]
	Item itemPrefab_;

	[SerializeField]
	Player player_;

	[SerializeField]
	Field field_;

	[SerializeField]
	float averageInterval_ = 10.0f;

	// ブロック出現平均間隔(sec)
	public float AverageInterval {
		set {
			averageInterval_ = value;
			N_ = averageInterval_ / Time.deltaTime;
			p_ = 1.0f / N_;
		}
		get {
			return averageInterval_;
		}
	}

	private void Awake()
	{
		AverageInterval = averageInterval_;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (averageInterval_ <= 0.0f) {
			return;
		}

		if (Random.value < p_) {
			// Playerの後方にエミット
			var pp = player_.transform.position;
			var block = PrefabUtil.createInstance( itemPrefab_, transform );
			block.transform.position = new Vector3( Randoms.Float.valueCenter() * field_.Width, pp.y - 1.0f, 0.0f );
		}
	}

	float N_ = 0.0f;
	float p_ = 0.0f;
}
