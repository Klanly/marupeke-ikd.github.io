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
        if ( false ) {
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
        // 清一色
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
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 二盃口
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 3 ) { Agari = true },
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 純チャン
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 0 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 0 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 7 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ) { Agari = true },
                new Pai( Pai.P1 ),
                new Pai( Pai.P1 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }

        // 混一色
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 0 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 0 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 7 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ) { Agari = true },
                new Pai( Pai.Ha ),
                new Pai( Pai.Ha ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 三槓子
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( 9 ),
                new Pai( 9 ),
                new Pai( 7 ),
                new Pai( 7 ),
                new Pai( 7 ) { Agari = true }
            };
            var mp = new List<Pai>() {
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
            for ( int i = 0; i < 3; ++i ) {
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

        // 小三元
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.Ha ),
                new Pai( Pai.Ha ),
                new Pai( Pai.Ha ),
                new Pai( Pai.Ht ),
                new Pai( Pai.Ht ),
                new Pai( Pai.Ht ),
                new Pai( Pai.M1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 8 ) { Agari = true },
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // チャンタ
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.M1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ) { Agari = true },
                new Pai( Pai.P9 ),
                new Pai( Pai.P9 ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.M1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 2 ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 一気通貫
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 7 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ) { Agari = true },
                new Pai( Pai.P9 ),
                new Pai( Pai.P9 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 混老頭
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 ),
                new Pai( Pai.S9 ),
                new Pai( Pai.S9 ),
                new Pai( Pai.S9 ),
                new Pai( Pai.M1 ),
                new Pai( Pai.M1 ),
                new Pai( Pai.M1 ),
                new Pai( Pai.Ha ) { Agari = true },
                new Pai( Pai.Ha ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.M9 ),
                new Pai( Pai.M9 ),
                new Pai( Pai.M9 ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 三色同順
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.P1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 2 ),
                new Pai( Pai.M1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ) { Agari = true },
                new Pai( Pai.P9 ),
                new Pai( Pai.P9 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 三色同刻
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.P1 + 2 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ) { Agari = true },
                new Pai( Pai.P9 ),
                new Pai( Pai.P9 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 三暗刻
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.P1 + 2 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ) { Agari = true },
                new Pai( Pai.P9 ),
                new Pai( Pai.P9 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 対々和
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.P9 ){ Agari = true },
                new Pai( Pai.P9 ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.M9 ),
                new Pai( Pai.M9 ),
                new Pai( Pai.M9 ),
                new Pai( Pai.M9 ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // ダブ東
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.M1 + 1 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.M1 + 3 ),
                new Pai( Pai.P9 ){ Agari = true },
                new Pai( Pai.P9 ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.To ),
                new Pai( Pai.To ),
                new Pai( Pai.To ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            ba.bakaze_ = Kaze.Ton;
            ba.zikaze_ = Kaze.Ton;
            calc.analyze( ankous, minkous, ba );
        }
        // ダブ南
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.Na ),
                new Pai( Pai.Na ),
                new Pai( Pai.Na ),
                new Pai( Pai.P9 ){ Agari = true },
                new Pai( Pai.P9 ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            ba.bakaze_ = Kaze.Nan;
            ba.zikaze_ = Kaze.Nan;
            calc.analyze( ankous, minkous, ba );
        }
        // 東南
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.To ),
                new Pai( Pai.To ),
                new Pai( Pai.To ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 2 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.Na ),
                new Pai( Pai.Na ),
                new Pai( Pai.Na ),
                new Pai( Pai.P9 ){ Agari = true },
                new Pai( Pai.P9 ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            ba.bakaze_ = Kaze.Nan;
            ba.zikaze_ = Kaze.Ton;
            calc.analyze( ankous, minkous, ba );
        }
        // 白、中
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.Ha ),
                new Pai( Pai.Ha ),
                new Pai( Pai.Ha ),
                new Pai( Pai.P1 + 1 ),
                new Pai( Pai.P1 + 2 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.Na ),
                new Pai( Pai.Na ),
                new Pai( Pai.Na ),
                new Pai( Pai.P9 ){ Agari = true },
                new Pai( Pai.P9 ),
            };
            var ms = new List<Pai>() {
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ),
                new Pai( Pai.Tu ),
            };
            var minkous = new List<PaiGroup>();
            var pg = new PaiGroup();
            pg.set( ms, true );
            minkous.Add( pg );
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 一盃口
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
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 4 ) { Agari = true },
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 平和
        if ( false ) {
            var ankous = new List<Pai>() {
/*              new Pai( Pai.S1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ),
                new Pai( Pai.P1 + 5 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.M1 + 3 ),
                new Pai( Pai.M1 + 4 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 6 ) { Agari = true },
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
*/
                new Pai( Pai.S1 ),
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ),
                new Pai( Pai.P1 + 5 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.M1 + 3 ),
                new Pai( Pai.M1 + 4 ),
                new Pai( Pai.S1 + 6 ) { Agari = true },
                new Pai( Pai.S1 + 7 ),
                new Pai( Pai.S1 + 8 ),
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),

            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // タンヤオ
        if ( false ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ),
                new Pai( Pai.P1 + 5 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.M1 + 3 ),
                new Pai( Pai.M1 + 4 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 6 ) { Agari = true },
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            calc.analyze( ankous, minkous, ba );
        }
        // 立直一発門前自摸
        if ( true ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ),
                new Pai( Pai.P1 + 5 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.M1 + 3 ),
                new Pai( Pai.M1 + 4 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 6 ) { Agari = true },
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            ba.ippatsh_ = true;
            ba.riti_ = true;
            ba.tsumo_ = true;
            ba.zyunme_ = 1;
            calc.analyze( ankous, minkous, ba );
        }
        // ダブル立直一発門前自摸
        if ( true ) {
            var ankous = new List<Pai>() {
                new Pai( Pai.S1 + 1 ),
                new Pai( Pai.S1 + 2 ),
                new Pai( Pai.S1 + 3 ),
                new Pai( Pai.P1 + 3 ),
                new Pai( Pai.P1 + 4 ),
                new Pai( Pai.P1 + 5 ),
                new Pai( Pai.M1 + 2 ),
                new Pai( Pai.M1 + 3 ),
                new Pai( Pai.M1 + 4 ),
                new Pai( Pai.S1 + 4 ),
                new Pai( Pai.S1 + 5 ),
                new Pai( Pai.S1 + 6 ) { Agari = true },
                new Pai( Pai.S1 + 6 ),
                new Pai( Pai.S1 + 6 ),
            };
            var minkous = new List<PaiGroup>();
            var calc = new MahjangScoreCalculator();
            var ba = new MahjangScoreCalculator.BaState();
            ba.bOya_ = true;
            ba.ippatsh_ = true;
            ba.riti_ = true;
            ba.tsumo_ = true;
            ba.zyunme_ = 0;
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
