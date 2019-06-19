using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<StageManager> stagePrefabs_;

    public System.Action FinishCallback { set; get; }

    public void initStage( int stage ) {
        curStageIdx_ = stage;
    }

    // ステージ生成
    void createStage() {
        if ( curStageIdx_ >= stagePrefabs_.Count ) {
            // 終わり
            FinishCallback();
            return;
        }

        if ( curStage_ != null ) {
            Destroy( curStage_.gameObject );
        }

        curStage_ = PrefabUtil.createInstance( stagePrefabs_[ curStageIdx_ ], transform );
        curStage_.ClearCallback = () => {
            // 次のステージへ
            curStageIdx_++;
            createStage();
        };
        curStage_.FinishCallback = (res) => {
            // 終わり
            FinishCallback();
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        createStage();
    }

    int curStageIdx_ = 0;
    StageManager curStage_;
}
