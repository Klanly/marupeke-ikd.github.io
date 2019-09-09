using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    int xNum_ = 8;

    [SerializeField]
    int yNum_ = 8;

    [SerializeField]
    float unitWidth_ = 2.0f;

    [SerializeField]
    float unitHeight_ = 3.7f;

    [SerializeField]
    Transform clientRoot_;

    public int XNum { get { return xNum_; } }
    public int YNum { get { return yNum_; } }
    public float UnitWidth { get { return unitWidth_; } }
    public float UnitHeight { get { return unitHeight_; } }
    public Transform ClientRoot { get { return clientRoot_; } }
    public Mahjang.Pai[,] Box { get { return box_; } }

    public Vector3 getPos( int idxX, int idxY ) {
        return new Vector3(
            unitWidth_ * ( idxX + 0.5f ),
            unitHeight_ * ( idxY + 0.5f ),
            0.0f
        );
    }

    private void Awake() {
        box_ = new Mahjang.Pai[ xNum_, yNum_ + 2 ];  // 上2個の所から発生
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    Mahjang.Pai[,] box_;
}
