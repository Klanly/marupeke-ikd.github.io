using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaraDollSystem : MonoBehaviour
{
    [SerializeField]
    WaraDoll waraDollPrefab_;

    [SerializeField]
    Transform dollPos_;

    private void Awake() {
        waraDoll_ = PrefabUtil.createInstance( waraDollPrefab_, dollPos_, Vector3.zero, Quaternion.identity );
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
}
