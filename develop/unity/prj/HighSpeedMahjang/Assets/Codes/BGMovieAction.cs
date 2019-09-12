using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 背景で動いている何か
public class BGMovieAction : MonoBehaviour
{
    [SerializeField]
    GameObject objPrefab_;

    class ObjInfo {
        public float radius_ = 20.0f;
        public float rot_ = 0.0f;
        public float xRot_ = 0.0f;
        public GameObject obj_;

        public void update() {
            obj_.transform.localPosition = new Vector3( radius_ * Mathf.Cos( rot_ * Mathf.Deg2Rad ), radius_ * Mathf.Sin( rot_ * Mathf.Deg2Rad ), 0.0f );
            obj_.transform.localRotation = Quaternion.Euler( xRot_, 0.0f, rot_ );
        }
    }
    private void Awake() {
        for ( int i = 0; i < 36; ++i ) {
            var info = new ObjInfo();
            // info.xRot_ = 360.0f / 36.0f * i;
            info.rot_ = 360.0f / 36.0f * i;
            info.obj_ = PrefabUtil.createInstance( objPrefab_, transform );
            objects_.Add( info );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t_ += Time.deltaTime;
        foreach ( var info in objects_ ) {
            info.rot_ += 0.1f;
            info.xRot_ += 0.0f;
            info.update();
        }        
    }

    float t_ = 0.0f;
    List<ObjInfo> objects_ = new List<ObjInfo>();
}
