using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mahjang;

public class Test : MonoBehaviour
{
    [SerializeField]
    bool bCheck_ = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    void check() {
        Pai[] pais = new Pai[] {
            new Pai( 27 ),
            new Pai( 28 ),
            new Pai( 29 ),
            new Pai( 30 ),
            new Pai( 31 ),
            new Pai( 32 ),
            new Pai( 33 ),
            new Pai( 0 ),
            new Pai( 8 ),
            new Pai( 9 ),
            new Pai( 17 ),
            new Pai( 18 ),
            new Pai( 26 ),
            new Pai( 26 ),
        };
        PaiGroup group = new PaiGroup();
        var type = group.set( pais, false );

        // 国士無双
        if ( false ){
            var ankous = new List<Pai>() {
                new Pai( 27 ),
                new Pai( 28 ),
                new Pai( 29 ),
                new Pai( 30 ),
                new Pai( 31 ),
                new Pai( 32 ),
                new Pai( 33 ),
                new Pai( 0 ),
                new Pai( 8 ),
                new Pai( 9 ),
                new Pai( 17 ),
                new Pai( 18 ),
                new Pai( 26 ),
                new Pai( 26 ),
            };
            var minkous = new List<PaiGroup>();
            ankous[ 12 ].Agari = true;
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 九連（索子）
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 0 ),
                new Pai( 0 ),
                new Pai( 0 ),
                new Pai( 1 ),
                new Pai( 2 ),
                new Pai( 3 ),
                new Pai( 4 ),
                new Pai( 5 ),
                new Pai( 5 ),
                new Pai( 6 ),
                new Pai( 8 ),
                new Pai( 8 ),
                new Pai( 8 ),
                new Pai( 7 ) { Agari = true },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 四暗刻
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 0 ),
                new Pai( 0 ),
                new Pai( 0 ),
                new Pai( 1 ),
                new Pai( 1 ),
                new Pai( 1 ),
                new Pai( 7 ),
                new Pai( 7 ),
                new Pai( 7 ),
                new Pai( 16 ),
                new Pai( 16 ),
                new Pai( 16 ){ Agari = true },
                new Pai( 9 ),
                new Pai( 9 ) { Agari = false },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 大四喜
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 30 ),
                new Pai( 30 ),
                new Pai( 30 ){ Agari = true },
                new Pai( 9 ),
                new Pai( 9 ) { Agari = false },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 四槓子
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 9 ),
                new Pai( 9 ),
            };
            var mp = new List<Pai>() {
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 30 ),
                new Pai( 30 ),
                new Pai( 30 ),
                new Pai( 30 ){ Agari = true },
            };
            var minkous = new List<PaiGroup>();
            for ( int i = 0; i < 4; ++i ) {
                var pg = new PaiGroup();
                for ( int p = 0; p < 4; ++p ) {
                    pg.set( mp, i * 4, 4, true );
                }
                minkous.Add( pg );
            }
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 清老頭
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 0 ),
                new Pai( 0 ),
                new Pai( 0 ),
                new Pai( 8 ),
                new Pai( 8 ),
                new Pai( 8 ),
                new Pai( 9 ),
                new Pai( 9 ),
                new Pai( 9 ),
                new Pai( 17 ),
                new Pai( 17 ),
                new Pai( 17 ){ Agari = true },
                new Pai( 19 ),
                new Pai( 19 ) { Agari = false },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 字一色
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 30 ),
                new Pai( 30 ),
                new Pai( 30 ){ Agari = true },
                new Pai( 7 ),
                new Pai( 7 ) { Agari = false },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 小四喜
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 3 ),
                new Pai( 4 ),
                new Pai( 5 ){ Agari = true },
                new Pai( 30 ),
                new Pai( 30 ) { Agari = false },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 大三元
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 31 ),
                new Pai( 31 ),
                new Pai( 31 ),
                new Pai( 32 ),
                new Pai( 32 ),
                new Pai( 32 ),
                new Pai( 33 ),
                new Pai( 33 ),
                new Pai( 33 ),
                new Pai( 3 ),
                new Pai( 4 ),
                new Pai( 5 ){ Agari = true },
                new Pai( 30 ),
                new Pai( 30 ) { Agari = false },
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 緑一色
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 5 ) { Agari = true },
                new Pai( Pai.Ht ),
                new Pai( Pai.Ht ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 大四喜、四槓子、四暗刻単騎、字一色
        if ( true ) {
            var ankous = new List<Pai>() {
                new Pai( 33 ),
                new Pai( 33 ) { Agari = true },
            };
            var mp = new List<Pai>() {
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 27 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 28 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 29 ),
                new Pai( 30 ),
                new Pai( 30 ),
                new Pai( 30 ),
                new Pai( 30 ),
            };
            var minkous = new List<PaiGroup>();
            for ( int i = 0; i < 4; ++i ) {
                var pg = new PaiGroup();
                for ( int p = 0; p < 4; ++p ) {
                    pg.set( mp, i * 4, 4, false );
                }
                minkous.Add( pg );
            }
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( bCheck_ == true ) {
            bCheck_ = false;
            check();
        }
    }
}
