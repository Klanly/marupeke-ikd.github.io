using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaiObject : MonoBehaviour
{
    [SerializeField]
    MeshRenderer renderer_;

    // セットアップ
    public void setup( int idx, Texture tex ) {
        var mat = renderer_.material;
        mat.mainTexture = tex;
        renderer_.material = mat;

        pai_.PaiType = idx;
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
}
