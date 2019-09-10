using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaiObject : MonoBehaviour {
    [SerializeField]
    MeshRenderer renderer_;

    public Vector2Int Index { get { return idx_; } }

    // セットアップ
    public void setup( int idx, Texture tex ) {
        var mat = renderer_.material;
        mat.mainTexture = tex;
        renderer_.material = mat;

        pai_.PaiType = idx;
    }

    // 牌情報を取得
    public Mahjang.Pai getPai() {
        return pai_;
    }

    // 確定インデックスを登録
    public void setIdx( Vector2Int idx ) {
        idx_ = idx;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mahjang.Pai pai_ = new Mahjang.Pai();
    Vector2Int idx_ = Vector2Int.zero;
}
