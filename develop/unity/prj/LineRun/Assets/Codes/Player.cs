using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
	[Inject]
	IOXInput input_ = null;

	[Inject]
	GameManager gameManager_;

	[SerializeField]
	float speed_ = 1.0f;

	[SerializeField]
	float flyTime_ = 1.0f;

	[SerializeField]
	float flyHeight_ = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// 上下左右移動
		Vector3 dir = input_.getDirection();
		transform.localPosition = transform.localPosition + dir * speed_ * Time.deltaTime;

		// ジャンプ
		if (jumpState_ == null ) {
			if (input_.decide() == true) {
				jumpTime_ = 0.0f;
				jumpState_ = jump;
			}
		} else {
			jumpState_();
		}

		var p = transform.localPosition;
		var tex = gameManager_.getLineRecordTexture();
		tex.setPixel( ( int )p.x, ( int )p.y, Color.black, true );
		tex.apply();
	}

	void jump()
	{
		jumpTime_ += Time.deltaTime;
		if ( jumpTime_ >= flyTime_ ) {
			jumpTime_ = flyTime_;
		}
		float t = jumpTime_;
		var p = transform.localPosition;
		p.z = -4.0f * flyHeight_ * t / flyTime_ * ( -t / flyTime_ + 1.0f );
		if ( jumpTime_ == flyTime_ ) {
			p.z = 0.0f;
			jumpState_ = null;
		}
		transform.localPosition = p;
	}

	System.Action jumpState_ = null;
	bool bJumping_ = false;
	float jumpTime_ = 0.0f;
}
