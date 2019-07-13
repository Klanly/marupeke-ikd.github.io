using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public class Param {
        public string name = "";
        public string material = "";
        public float weight = 0.0f;
        public string weightUnit = "g";
        public float dimensionX = 1.0f;
        public float dimensionY = 1.0f;
        public float dimensionZ = 1.0f;
        public string dimensionUnit = "cm";
    }

    public void setParam( Param param ) {
        param_ = param;
    }

    public Param getParam() {
        return param_;
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
