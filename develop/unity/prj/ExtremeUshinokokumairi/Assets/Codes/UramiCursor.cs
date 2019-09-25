using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UramiCursor : MonoBehaviour
{
    [SerializeField]
    GameObject cursor_;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // カーソル位置をXY平面投影点へ
        Vector3 pos = Vector3.zero;
        if ( CameraUtil.calcClickPosition( Camera.main, Input.mousePosition, out pos ) == true ) {
            cursor_.transform.position = pos;
        }
    }
}
