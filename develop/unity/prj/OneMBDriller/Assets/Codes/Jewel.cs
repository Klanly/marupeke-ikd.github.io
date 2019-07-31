using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 宝石
public class Jewel : MonoBehaviour
{
	[SerializeField]
	float radius_ = 0.8f;

	[SerializeField]
	Type type_ = Type.Diamond;

	public enum Type
	{
		Diamond,
		Sapphire,
	}

	public Type getType() {
		return type_;
	}

	public float getRadius() {
		return radius_;
	}

    // Start is called before the first frame update
    void Start()
    {
		var target = GameManager.getInstance().getPlayer();
		target.addJewel( this );
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
