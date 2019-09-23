using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaraDollSystem : MonoBehaviour
{
    [SerializeField]
    WaraDoll waraDollPrefab_;

    [SerializeField]
    Transform dollPos_;

    [SerializeField]
    KugiPoint[] kugiPoints_;


    public class Parameter {
        public int minKugiCount_ = 3;
        public int maxKugiCount_ = 5;
    }

    private void Awake() {
        waraDoll_ = PrefabUtil.createInstance( waraDollPrefab_, dollPos_, Vector3.zero, Quaternion.identity );
    }

    // セットアップ
    public void setup( Parameter param ) {
        parameter_ = param;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    WaraDoll waraDoll_;
    Parameter parameter_ = new Parameter();
}
