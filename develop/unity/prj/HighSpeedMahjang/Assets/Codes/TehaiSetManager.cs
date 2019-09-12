using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 手配を並べる入れ物を作る人
public class TehaiSetManager : MonoBehaviour {

    [SerializeField]
    TehaiSet tehaiSetPrefab_;

    [SerializeField]
    Transform tehaiPos_;

    // 手配セットを作成
    public TehaiSet createNewTehaiSet() {
        var obj = PrefabUtil.createInstance<TehaiSet>( tehaiSetPrefab_, tehaiPos_ );
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
