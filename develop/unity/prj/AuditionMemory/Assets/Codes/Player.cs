using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    SelectCursor firstCursorPrefab_;

    [SerializeField]
    SelectCursor alreadyCursorPrefab_;

    [SerializeField]
    UnityEngine.UI.Text comment_;

    [SerializeField]
    UnityEngine.UI.Text comboText_;

    public void setup( GameManager manager ) {
        manager_ = manager;
    }

    private void Awake() {
        var color = comment_.color;
        color.a = 0.0f;
        comment_.color = color;
        comboText_.text = "";
    }

    void Start () {
        state_ = new Gaming( this );
	}
	
	void Update () {
        if ( state_ != null )
            state_ = state_.update();

    }

    Speaker isSelectSpeaker() {
        // レイ飛ばす
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit = new RaycastHit();
        if ( Physics.Raycast( ray, out hit ) == true ) {
            return hit.transform.GetComponent<Speaker>();
        }
        return null;
    }

    class Gaming : State< Player > {
        public Gaming(Player parent) : base( parent ) { }
        protected override State innerInit() {
            return null;
        }
        protected override State innerUpdate() {
            // カメラズーム
            var scrDelta = Input.mouseScrollDelta;
            float zoomScale = 0.2f;
            if ( scrDelta.y != 0.0f ) {
                var cp = Camera.main.transform.position;
                var nextY = cp.y + ( Camera.main.transform.forward.y ) * scrDelta.y * zoomScale;
                if ( nextY >= 1.6f && nextY <= 7.0f ) {
                    cp += Camera.main.transform.forward * scrDelta.y * zoomScale;
                    Camera.main.transform.position = cp;
                }
            }

            if ( Input.GetMouseButtonDown( 0 ) == true ) {
                // フィールドドラッグ開始
                fieldDrugging_ = true;
                clickPos_ = Input.mousePosition;
                cameraPicker_.startPicking( Camera.main, Input.mousePosition, Vector3.up, Vector3.zero );
            }

            if ( Input.GetMouseButtonUp( 0 ) == true ) {
                // フィールドドラッグ終了
                fieldDrugging_ = false;
                if ( ( clickPos_ - Input.mousePosition ).magnitude <= 0.3f ) {
                    // スピーカ選択？
                    var speaker = parent_.isSelectSpeaker();
                    if ( speaker != null ) {
                        speaker.playSE();
                        // 1つ目ならスタック
                        if ( bSelectFirst_ == true ) {
                            bSelectFirst_ = false;
                            firstSelectSpeaker_ = speaker;
                            firstSelectSpeaker_.enableSelect( false );  // スピーカー一時的に選択不可に

                            // カーソル表示
                            showCursor( speaker, ref firstCursor_ );
                            speaker.addSelectCount();
                        } else {
                            // 2つ目選択
                            // カーソル表示
                            showCursor( speaker, ref secondCursor_ );
                            speaker.addSelectCount();

                            // 得点計算
                            // baseSoreに連続コンボ数を掛け算したのが得点
                            int baseScore = 0;
                            int comboCount = 0;
                            bool isIncrement = true;
                            var commentFlag = Comments.ComboState.Success_First_First;
                            if ( firstSelectSpeaker_.getSEName() == speaker.getSEName() ) {
                                // 一致
                                // 1つ目がFirst Soundで2つ目もFirstSound -> 偶然の一致！   120点
                                // 1つ目がFirst Soundで2つ目がAlready... -> 適格合致！     150点
                                // 1つ目がAlready...で2つ目がFirst Sound -> まぐれ当たり！  80点
                                // 1つ目がAlready...で2つ目もAlready...  -> やっと当てたか  50点
                                sameComboCount_++;
                                defComboCount_ = 0;
                                comboCount = sameComboCount_;
                                parent_.manager_.getSpeakers( parent_, new Speaker[] { firstSelectSpeaker_, speaker } );
                                // 合致得点計算
                                if ( firstSelectSpeaker_.getSelectCount() == 1 ) {
                                    if ( speaker.getSelectCount() == 1 ) {
                                        // 偶然の一致得点
                                        baseScore = 120;
                                        commentFlag = Comments.ComboState.Success_First_First;
                                    } else {
                                        // 適格合致得点
                                        baseScore = 150;
                                        commentFlag = Comments.ComboState.Success_First_Already;
                                    }
                                } else {
                                    if ( speaker.getSelectCount() == 1 ) {
                                        // まぐれ当たり得点
                                        baseScore = 80;
                                        commentFlag = Comments.ComboState.Success_Already_First;
                                    } else {
                                        // やっと当てたか得点
                                        baseScore = 50;
                                        commentFlag = Comments.ComboState.Success_Already_Already;
                                    }
                                }
                            } else {
                                // 不一致
                                // 1つ目がFirst Soundで2つ目もFirst Sound、ペアはまだ出てない -> 最初だからしゃーない 0点
                                // 1つ目がFirst Soundで2つ目もFirst Sound、ペアはAlready      -> 選べたはずじゃん！   -20点
                                // 1つ目がFirst Soundで2つ目はAlready...、ペアはまだ出てない  -> 一度聞いたの忘れた？ -30点
                                // 1つ目がFirst Soundで2つ目はAlready...、ペアはAlready       -> 覚え間違いしちゃってるよ… -80点
                                // 1つ目がAlready...で2つ目はFirst Sound、ペアはまだ出てない  -> 同じミスしちゃった   -30点
                                // 1つ目がAlready...で2つ目はFirst Sound、ペアはAlready       -> 選べたミスをもう一度 -50点
                                // 1つ目がAlready...で2つ目もAlready... 、ペアはまだ出てない  -> 闇雲に探してない？   -120点
                                // 1つ目がAlready...で2つ目もAlready... 、ペアはAlready       -> 完全に迷子～         -150点
                                //
                                comboCount = defComboCount_;
                                isIncrement = false;

                                // ペアスピーカーの状態で減点が変わる
                                var otherSpeaker = parent_.manager_.getPairSpeaker( firstSelectSpeaker_ );
                                if ( firstSelectSpeaker_.getSelectCount() == 1 ) {
                                    if ( speaker.getSelectCount() == 1 ) {
                                        if ( otherSpeaker.getSelectCount() == 0 ) {
                                            baseScore = 0;  // 最初だからしゃーない
                                            commentFlag = Comments.ComboState.Failed_First_First_PairNone;
                                        } else {
                                            baseScore = -20; // 選べたはずじゃん！
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_First_First_PairAlready;
                                        }
                                    } else {
                                        if ( otherSpeaker.getSelectCount() == 0 ) {
                                            baseScore = -30;  // 一度聞いたの忘れた？
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_First_Already_PairNone;
                                        } else {
                                            baseScore = -40; // 覚え間違いしちゃってるよ…
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_First_Already_PairAlready;
                                        }
                                    }
                                } else {
                                    if ( speaker.getSelectCount() == 1 ) {
                                        if ( otherSpeaker.getSelectCount() == 0 ) {
                                            baseScore = -30;  // 同じミス事しちゃった
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_Already_First_PairNone;
                                        } else {
                                            baseScore = -40; // 選べたミスをもう一度
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_Already_First_PairAlready;
                                        }
                                    } else {
                                        if ( otherSpeaker.getSelectCount() == 0 ) {
                                            baseScore = -50;  // 闇雲に探してない？
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_Already_Already_PairNone;
                                        } else {
                                            baseScore = -60; // 完全に迷子…
                                            defComboCount_++;
                                            sameComboCount_ = 0;
                                            commentFlag = Comments.ComboState.Failed_Already_Already_PairAlready;
                                        }
                                    }
                                }
                                // 最初に選択したスピーカー選択可に戻す
                                firstSelectSpeaker_.enableSelect( true );
                                speaker.enableSelect( true );
                            }
                            // スコア
                            int defScore = baseScore * comboCount;
                            curScore_ += defScore;
                            parent_.manager_.updateScore( baseScore, comboCount, curScore_, isIncrement );
                            // コメント
                            showComment( commentFlag );
                            // コンボテキスト
                            if ( comboCount > 0 ) {
                                if ( baseScore < 0 ) {
                                    parent_.comboText_.color = ColorHelper.getColor( 0x661CE6FF );
                                    parent_.comboText_.text = "ミス" + comboCount + "連続中...";
                                } else if ( baseScore > 0 ) {
                                    parent_.comboText_.color = ColorHelper.getColor( 0xE66C1CFF );
                                    parent_.comboText_.text = "正解" + comboCount + "連続中！！";
                                }
                            }

                            // カーソル消去
                            firstCursor_.remove( 2.0f );
                            secondCursor_.remove( 2.0f );
                            bSelectFirst_ = true;
                        }
                    }
                }
            }

            if ( fieldDrugging_ == true && Input.GetMouseButton( 0 ) == true ) {
                cameraPicker_.updateCameraPos( Input.mousePosition );
            }
            return this;
        }

        void showCursor( Speaker speaker, ref SelectCursor cursor ) {
            // カーソル表示
            if ( speaker.getSelectCount() == 0 ) {
                // 最初の選択
                cursor = Instantiate<SelectCursor>( parent_.firstCursorPrefab_ );
            } else {
                // 2回目以降の選択
                cursor = Instantiate<SelectCursor>( parent_.alreadyCursorPrefab_ );
            }
            cursor.transform.position = speaker.transform.position;
        }

        void showComment( Comments.ComboState commentState ) {
            parent_.comment_.text = Comments.getComment( commentState );
            var color = parent_.comment_.color;
            GlobalState.time( 0.2f, (sec, t) => {
                color.a = Lerps.Float.easeInOut( 0.0f, 1.0f, t );
                parent_.comment_.color = color;
                return true;
            } ).nextTime( 1.0f, (sec, t )=> {
                return true;
            } ).nextTime( 0.2f, (sec, t )=> {
                color.a = Lerps.Float.easeInOut( 1.0f, 0.0f, t );
                parent_.comment_.color = color;
                return true;
            } );
        }

        CameraPicker cameraPicker_ = new CameraPicker();
        bool fieldDrugging_ = false;
        Vector3 clickPos_ = Vector3.zero;
        bool bSelectFirst_ = true;
        Speaker firstSelectSpeaker_ = null;
        SelectCursor firstCursor_;
        SelectCursor secondCursor_;
        int sameComboCount_ = 0;
        int defComboCount_ = 0;
        int curScore_ = 0;
    }

    GameManager manager_;
    State state_;
}
