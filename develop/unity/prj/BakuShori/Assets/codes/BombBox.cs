using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 爆弾箱
//
//  表面にタイマー、ギミックボックス、各Answer、ギミックネジを持つ
//  また内部に赤青線を保持している。

public class BombBox : Entity {

    [SerializeField]
    BombBoxModel bombBoxModel_;

    // 完全成功コールバック
    public System.Action AllDiactiveDoneCallback { set { allDiactiveDoneCallback_ = value; } }

    // 失敗コールバック
    public System.Action FailureCallback { set { failureCallback_ = value; } }
    void Awake()
    {
        ObjectType = EObjectType.BombBox;
    }

    // フロントパネル開いた？
    public bool isOpenFrontPanel()
    {
        return bombBoxModel_.isOpenFrontPanel();
    }

    // タイマーを急激に減少
    public void advanceTimer( System.Action notifyZero )
    {
        bombBoxModel_.advanceTimer( notifyZero );
    }

    // タイマーを停止
    public void stopTimer()
    {
        bombBoxModel_.stopTimer();
    }

    // ギミック解除成功
    void gimicSuccess()
    {
        successGimicCount_++;
        if ( successGimicCount_ == gimicCount_ ) {
            // フロントオープン
            bombBoxModel_.openFrontPanel();
            Debug.Log( "Open front !" );
        }
    }

    // 箱爆発
    void explosion()
    {
        Debug.Log( "Explosion!!!" );
        failureCallback_();
    }

    // Entityを登録
    override public bool setEntity( int index, Entity entity )
    {
        // BombBoxはAnswer以外は登録できない
        if ( entity.isAnswer() == false )
        {
            return false;
        }
        return base.setEntity( index, entity );
    }

    // ギミックネジを取得
    public List<GimicScrew> getGimicScrewes()
    {
        return gimicScrews_;
    }

    // 箱の連携関係をダンプ
    public void dumpBox()
    {
        string str = "";
        dumpBox( this, ref str, 0, 0 );
        Debug.Log( str );
    }

    // 箱の連携関係をダンプ
    void dumpBox( Entity e, ref string str, int index, int indent )
    {
        System.Func<string> indentStr = () => {
            string s = "";
            for ( int i = 0; i < indent; ++i ) {
                s += " ";
            }
            return s;
        };
        str += indentStr() + index + ": ";
        if ( e == null ) {
            str += "null\n";
            return;
        }
        str += e.ObjectType.ToString() + e.Index + "\n";
        for ( int i = 0; i < getChildrenListSize(); ++i ) {
            dumpBox( e.getEntity( i ), ref str, i, indent + 4 );
        }
    }

    // 箱を形成
    public void buildBox( int randomNumber )
    {
        int ansIdx = 0;
        foreach( var e in childrenEntities_ ) {
            if ( e == null )
                continue;

            // 自分の直下にあるアンサー群はANSノードへ
            if ( e.isAnswer() == true ) {
                var ansNode = bombBoxModel_.getBombBoxAnswerNode( ansIdx );
                e.transform.parent = ansNode.transform;
                e.transform.localPosition = Vector3.zero;
                e.transform.localRotation = Quaternion.identity;
                ansIdx++;

                // アンサーノードの下をトラバースしてギミックボックス及び
                // ギミックを設置
                build( e );
            }
        }

        // 赤青ランプ設定
        bombBoxModel_.getRBLamp().setup( randomNumber );

        // 赤青ラインを切った時の結果を受ける
        bombBoxModel_.CutRBCallback = (color, res) => {
            if ( res == 1 ) {
                // 成功
                if ( color == BombBoxModel.CutLine.Red )
                    bSuccessRedLineCut_ = true;
                else if ( color == BombBoxModel.CutLine.Blue )
                    bSuccessBlueLineCut_ = true;
                if ( bSuccessRedLineCut_ == true && bSuccessBlueLineCut_ == true ) {
                    // 爆弾箱解除成功！
                    allDiactiveDoneCallback_();
                }
            } else if ( res == 0 ) {
                // 失敗
                explosion();
            }
        };

        // タイムオーバー時に失敗を受ける
        bombBoxModel_.getTimer().setNotifyZero( () => {
            explosion();
        });
    }

    // Entity以下を形成
    public void build( Entity e )
    {
        if ( e == null )
            return;

        if ( e.isGimic() == true ) {
            // ギミックを設置
            var gbNode = bombBoxModel_.getGimicBoxTrans( e.Index );
            e.transform.parent = gbNode.transform;
            e.transform.localPosition = Vector3.zero;
            // gbNodeの位置に対応してギミックを回転（ダサい…orz）
            // y軸-90度
            var q = Quaternion.Euler( 0.0f, -90.0f, 0.0f ) * e.transform.localRotation;
            e.transform.localRotation = q;

            var gimic = e as Gimic;
            // ギミック解除に成功したら成功カウントアップ
            gimic.SuccessCallback = () => {
                gimicSuccess();
            };

            // ギミック解除に失敗したら爆発
            gimic.FailureCallback = () => {
                explosion();
            };

            gimicCount_++;
        }
        else if ( e.isGimicBox() == true ) {
            // ギミックボックスの子に所属しているアンサーを
            // ギミックボックス内に設定
            var gimicBox = e as GimicBox;
            if ( gimicBox != null ) {
                var answers = gimicBox.getAnswres();
                for ( int i = 0; i < answers.Count; ++i ) {
                    var ans = answers[ i ];
                    var node = bombBoxModel_.getGimicBoxAnswerTrans( gimicBox.Index, i );
                    if ( node == null ) {
                        Debug.Assert( false );
                        continue;
                    }

                    // 蓋にスケールが入っていて回転で歪んでしまうので
                    // 蓋との親子関係は作らず
                    // 蓋の姿勢を常に監視するようにする（ダサい… orz）
                    var ansTO = ans.gameObject.AddComponent<TransObserver>();
                    ansTO.setTarget( node.transform );
                }

                // 蓋の表面にトラップをセット
                // 蓋との親子関係は作らず
                // 蓋の姿勢を常に監視するようにする（ダサい… orz）
                var trapPos = bombBoxModel_.getTrapTrans( gimicBox.Index );
                if ( trapPos == null ) {
                    Debug.Assert( false );                    
                } else {
                    var trap = gimicBox.getTrap();
                    var to = trap.gameObject.AddComponent<TransObserver>();
                    to.setTarget( trapPos.transform );
                }

                // ギミックボックスの蓋開けに成功したら
                // 蓋開けモーション再生
                gimicBox.SuccessCallback = () => {
                    bombBoxModel_.openCover( gimicBox.Index );
                };

                // ギミックボックスの蓋開けに失敗したら
                // 爆発
                gimicBox.FailureCallback = () => {
                    explosion();
                };
            }
        }

        int childNum = e.getChildrenListSize();
        for ( int i = 0; i < childNum; ++i ) {
            var c = e.getEntity( i );
            if ( c == null )
                continue;
            build( c );
        }

    }

    private void Start()
    {
/*
        int num = bombBoxModel_.getGimicBoxPlaceNum();
        for ( int i = 0; i < num; ++i )
           bombBoxModel_.openCover( i );
*/    }

    List<GimicScrew> gimicScrews_ = new List<GimicScrew>();
    int gimicCount_ = 0;
    int successGimicCount_ = 0;
    bool bSuccessRedLineCut_ = false;
    bool bSuccessBlueLineCut_ = false;
    System.Action allDiactiveDoneCallback_;
    System.Action failureCallback_;
}
