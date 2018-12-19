using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text scoreText_;

    [SerializeField]
    ComboText comboText_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( tasks_.Count > 0 ) {
            List<ComboTask> tasks = new List<ComboTask>();
            for ( int i = 0; i < tasks_.Count; ++i ) {
                if ( tasks_[ i ].update() == false ) {
                    tasks.Add( tasks_[ i ] );
                }
            }
            tasks_ = tasks;
        }
    }

    public void setup( float fieldRadius )
    {
        fieldRadius_ = fieldRadius;
    }

    public void destroyEnemy( LandoltEnemy enemy, bool isCombo, bool isMultiHit )
    {
        // コンボ数計算
        int preCombo = combo_;
        if ( isCombo == true ) {
            combo_++;
            if ( isMultiHit == true )
                combo_ += 2;
        } else {
            combo_ = 0;
        }

        // そもそも外れてる？
        if ( enemy == null )
            return;

        // 敵移動距離でボーナス
        Bonus bonus = Bonus.Bonus_No;
        var enemyLen = enemy.getMoveLen();
        float rate = enemyLen / fieldRadius_;
        if ( rate <= GameDefines.quickRate_g ) {
            bonus = Bonus.Bonus_Quick;
        } else if ( rate >= GameDefines.limitRate_g ) {
            bonus = Bonus.Bonus_Limit;
        }

        if ( isCombo == false ) {
            // コンボ失敗
            score_ += enemy.getBaseScore();
            scoreText_.text = string.Format( "{0:0000000000}", score_ );
        } else if ( combo_ == 1 ) {
            score_ += enemy.getBaseScore();
            scoreText_.text = string.Format( "{0:0000000000}", score_ );
        } else {
            for ( int i = preCombo + 1; i <= combo_; ++i ) {
                if ( tasks_.Count > 0 )
                    tasks_.Add( new ComboTask( this, i, enemy.getBaseScore(), tasks_[ tasks_.Count - 1 ].getDelay() + 0.35f, isMultiHit, bonus ) );
                else
                    tasks_.Add( new ComboTask( this, i, enemy.getBaseScore(), 0.0f, isMultiHit, bonus ) );
            }
        }
    }

    class ComboTask
    {
        public ComboTask( ScoreManager manager, int combo, int baseScore, float delay, bool multi, Bonus bonus )
        {
            manager_ = manager;
            combo_ = combo;
            baseScore_ = baseScore;
            delay_ = delay;
            multi_ = multi;
            bonus_ = bonus;
        }

        public float getDelay()
        {
            return delay_;
        }

        public bool update()
        {
            delay_ -= Time.deltaTime;
            if ( delay_ <= 0.0f )
                delay_ = 0.0f;
            if ( delay_ <= 0.0f ) {
                float bonusRate = 1.0f;
                if ( bonus_ == Bonus.Bonus_Quick )
                    bonusRate = 2.0f;
                else if ( bonus_ == Bonus.Bonus_Limit )
                    bonusRate = 3.0f;
                int addScore = (int)( ( baseScore_ * bonusRate ) * Mathf.Pow( (float)combo_, GameDefines.comboPowRate_g ) );
                manager_.score_ += addScore;
                manager_.scoreText_.text = string.Format( "{0:0000000000}", manager_.score_ );

                var co = Instantiate<ComboText>( manager_.comboText_, Vector3.zero, Quaternion.identity );
                co.setup( combo_, addScore, bonus_ );
                co.gameObject.SetActive( true );
            }
            return ( delay_ <= 0.0f );
        }

        ScoreManager manager_;
        int combo_;
        int baseScore_;
        float delay_;
        bool multi_;
        int preScore_;
        Bonus bonus_;
    }

    float fieldRadius_ = 100.0f;
    int score_ = 0;
    int combo_ = 0;
    List<ComboTask> tasks_ = new List<ComboTask>();
}
