using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer src_;

    public class Param {
        public string name = "";
        public string material = "";
        public float weight = 0.0f;
        public string weightUnit = "g";
        public float dimensionX = 1.0f;
        public float dimensionY = 1.0f;
        public float dimensionZ = 1.0f;
        public string dimensionUnit = "cm";
        public string answer = "";
        public string image = "";
        public string comment = "";
        public string point = "";
    }

    public void setParam( Param param ) {
        param_ = param;
        setImage( param_.image );
    }

    public Param getParam() {
        return param_;
    }

    // 指定名のスプライトを設定
    void setImage(string name) {
        // リソースからロード
        var sprite = ResourceLoader.getInstance().loadSync<Sprite>( "garbages/" + name );
        if ( sprite != null ) {
            src_.sprite = sprite;
        } else {
            Debug.Log( "Cardスプライト読み込み失敗" );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Param param_;
}
