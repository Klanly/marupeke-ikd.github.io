﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 麻雀得点計算アルゴリズム

namespace Mahjang {
    // 風
    public enum Kaze : int {
        Ton = 27,    // 東
        Nan = 28,    // 南
        Sha = 29,    // 西
        Pei = 30     // 北
    }

    // 牌
    public class Pai {
        public enum Type {
            None,   // 無効牌
            Sozu,   // 索子
            Pinzu,  // 筒子
            Manzu,  // 萬子
            Ji      // 字牌
        }

        public Pai() {
        }

        public Pai( int paiType ) {
            PaiType = paiType;
        }

        public int PaiType {
            set {
                if ( value < -2 || value > 34 ) {
                    paiType_ = -1;  // 無効牌
                } else {
                    paiType_ = value;
                }
            }
            get {
                return paiType_;
            }
        }

        public bool Agari { set { bAgari_ = value; } get { return bAgari_; } }

        public static int S1 = 0;   // 1索
        public static int S9 = 8;   // 9索
        public static int P1 = 9;   // 1筒
        public static int P9 = 17;   // 9筒
        public static int M1 = 18;   // 1萬
        public static int M9 = 26;   // 9萬
        public static int To = 27;   // 東
        public static int Na = 28;   // 南
        public static int Sh = 29;   // 西
        public static int Pe = 30;   // 北
        public static int Ha = 31;   // 白
        public static int Ht = 32;   // 発
        public static int Tu = 33;   // 中

        // 牌の種類
        // 0  -  8: 索子
        // 9  - 17: 筒子
        // 18 - 26: 萬子
        // 27,28,29,30: 東南西北
        // 31,32,33   : 白発中
        int paiType_ = -1;
        bool bAgari_ = false;   // 上がり牌？

        // 記号文字で牌を設定
        //  "S1" - "S9" : 索子
        //  "P1" - "P9" : 筒子
        //  "M1" - "M9" : 萬子
        //  "To,Na,Sh,Pe,Ha,Ht,Tu" : 東南西北白発中
        public void setAsMark( string mark ) {
            if ( mark.Length != 2 ) {
                PaiType = -1;
                return;
            }
            char m1 = mark[ 0 ];
            char m2 = mark[ 1 ];
            if ( m2 >= '1' && m2 <= '9' ) {
                int n = m2 - '1';
                switch ( m1 ) {
                    case 'S':
                        PaiType = n;
                        break;
                    case 'P':
                        PaiType = 9 + n;
                        break;
                    case 'M':
                        PaiType = 18 + n;
                        break;
                    default:
                        PaiType = -1;
                        break;
                }
                return;
            }
            switch ( mark ) {
                case "To":
                    PaiType = 27;
                    break;
                case "Na":
                    PaiType = 28;
                    break;
                case "Sh":
                    PaiType = 29;
                    break;
                case "Pe":
                    PaiType = 30;
                    break;
                case "Ha":
                    PaiType = 31;
                    break;
                case "Ht":
                    PaiType = 32;
                    break;
                case "Tu":
                    PaiType = 33;
                    break;
            }
            PaiType = -1;
        }

        // 有効牌？
        public bool isValid() {
            return ( PaiType != -1 );
        }

        // 索子？
        public bool isSouzu() {
            if ( isValid() == false ) {
                return false;
            }
            return ( PaiType >= S1 && PaiType <= S9 );
        }

        // 筒子？
        public bool isPinzu() {
            if ( isValid() == false ) {
                return false;
            }
            return ( PaiType >= P1 && PaiType <= P9 );
        }

        // 萬子？
        public bool isManzu() {
            if ( isValid() == false ) {
                return false;
            }
            return ( PaiType >= M1 && PaiType <= M9 );
        }

        // 字牌？
        public bool isJi() {
            if ( isValid() == false ) {
                return false;
            }
            return PaiType >= 27;
        }

        // 数牌？
        public bool isNumber() {
            if ( isValid() == false ) {
                return false;
            }
            return PaiType < 27;
        }

        // 風牌？
        public bool isKaze() {
            if ( isValid() == false ) {
                return false;
            }
            return ( PaiType >= 27 && PaiType <= 30 ); 
        }

        // 役牌？
        public bool isYaku() {
            if ( isValid() == false ) {
                return false;
            }
            return ( PaiType >= 31 && PaiType <= 33 );
        }

        // 中張牌？（2～8）
        public bool isTyunTyan() {
            if ( isValid() == false ) {
                return false;
            }
            return
                ( PaiType >= S1 + 1 && PaiType <= S9 - 1 ) ||
                ( PaiType >= P1 + 1 && PaiType <= P9 - 1 ) ||
                ( PaiType >= M1 + 1 && PaiType <= M1 - 1 );
        }

        // ヤオ九牌？(1,9,字牌）
        public bool isYaotyu() {
            if ( isValid() == false ) {
                return false;
            }
            return !isTyunTyan();
        }

        // 順子として次の牌が成立している？
        public bool isNext( Pai next ) {
            if ( isNumber() == false || next.isNumber() == false ) {
                return false;
            }
            if ( PaiType + 1 != next.PaiType ) {
                // 次の数値にはなっていない
                return false;
            }
            if ( PaiType == S9 || PaiType == P9 || PaiType == M9 ) {
                // 9の次は続かない
                return false;
            }
            return true;
        }
    }

    // 面子
    public class PaiGroup {
        public enum Type {
            None,       // 無効
            Toitsu,     // 対子（2枚）
            Shuntsu,    // 順子（3枚）
            Koutsu,     // 刻子（3枚）
            Kantsu,     // 槓子（3枚）
            Kokushi,    // 国士無双（14枚）
        }

        // 対子？
        static public bool isToitsu( Pai[] pais ) {
            if ( pais.Length != 2 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false ) {
                return false;
            }
            return ( pais[ 0 ].PaiType == pais[ 1 ].PaiType );
        }

        // 順子？
        static public bool isShuntsu( Pai[] pais, bool doSort = true ) {
            if ( pais.Length != 3 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false || pais[ 2 ].isValid() == false ) {
                return false;
            }
            if ( doSort == true ) {
                sort( ref pais );
            }
            return ( pais[ 0 ].isNext( pais[ 1 ] ) && pais[ 1 ].isNext( pais[ 2 ] ) );
        }

        // 刻子？
        static public bool isKoutsu( Pai[] pais ) {
            if ( pais.Length != 3 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false || pais[ 2 ].isValid() == false ) {
                return false;
            }
            return ( pais[ 0 ].PaiType == pais[ 1 ].PaiType && pais[ 1 ].PaiType == pais[ 2 ].PaiType );
        }

        // 槓子？
        static public bool isKantss( Pai[] pais ) {
            if ( pais.Length != 4 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false || pais[ 2 ].isValid() == false || pais[ 3 ].isValid() == false ) {
                return false;
            }
            return ( pais[ 0 ].PaiType == pais[ 1 ].PaiType && pais[ 1 ].PaiType == pais[ 2 ].PaiType && pais[ 2 ].PaiType == pais[ 3 ].PaiType );
        }

        // 国士無双？
        static public bool isKokushi( Pai[] pais, bool doSort = true ) {
            if ( pais.Length != 14 ) {
                return false;
            }
            if ( doSort == true ) {
                sort( ref pais );
            }
            int[] ps = new int[] { Pai.S1, Pai.S9, Pai.P1, Pai.P9, Pai.M1, Pai.M9, Pai.To, Pai.Na, Pai.Sh, Pai.Pe, Pai.Ha, Pai.Ht, Pai.Tu };
            sort( ref pais );
            int ofs = 0;
            for ( int i = 0; i < ps.Length; ++i ) {
                if ( ps[ i ] != pais[ i + ofs ].PaiType ) {
                    return false;
                }
                if ( i + 1 < ps.Length && pais[ i ].PaiType == pais[ i + 1 ].PaiType ) {
                    ofs++;
                }
            }
            return true;
        }

        // 面子をセット
        // isMinko : 明刻？
        public Type set( Pai[] pais, bool isMinko ) {
            sort( ref pais );
            // 対子？
            if ( isToitsu( pais ) == true ) {
                type_ = Type.Toitsu;
                isMinko = false;    // 対子の明刻は無い
            }
            // 順子？
            else if ( isShuntsu( pais, false ) == true ) {
                type_ = Type.Shuntsu;
            }
            // 刻子？
            else if ( isKoutsu( pais ) == true ) {
                type_ = Type.Koutsu;
            }
            // 槓子？
            else if ( isKantss( pais ) == true ) {
                type_ = Type.Kantsu;
            }
            // 国士無双？
            else if ( isKokushi( pais, false ) == true ) {
                type_ = Type.Kokushi;
                isMinko = false;    // 国士無双の明刻は無い
            }
            // 何物でも無い
            else {
                type_ = Type.None;
                isMinko = false;
            }
            pais_ = pais;
            bMinko_ = isMinko;
            return type_;
        }

		public Type set( List< Pai > pais, bool isMinko ) {
			var list = pais.ToArray();
			return set( list, isMinko );
		}

        // 牌を昇順に並べ替える
        //  無効牌は最後尾に並ぶようにする
        public static void sort( ref Pai[] pais ) {
            pais = pais.OrderBy( (pai) => {
                if ( pai.PaiType == -1 ) {
                    return 100;
                }
                return pai.PaiType;
            } ).ToArray();
        }

        // 牌を昇順に並べ替える
        //  無効牌は最後尾に並ぶようにする
        public static void sort(ref List< Pai > pais) {
            pais.Sort( ( l, r ) => {
                if ( l.PaiType == r.PaiType ) {
                    return 0;
                }
                if ( l.isValid() == false ) {
                    return 1;
                }
                if ( r.isValid() == false ) {
                    return -1;
                }
                return ( l.PaiType < r.PaiType ? -1 : 1 );
            } );
        }

        // 成立している？
        public bool isValid() {
            return ( type_ != Type.None );
        }

        // 明刻？
        public bool isMinko() {
            return bMinko_;
        }

        // 型を取得
        public Type getType() {
            return type_;
        }

        // 牌を取得
        public Pai[] getPais() {
            return pais_;
        }

        // 上がり牌があれば取得
        public Pai getAgarihai() {
            foreach ( var p in pais_ ) {
                if ( p.Agari == true )
                    return p;
            }
            return null;
        }

        // 符数を取得
        //  待ちは考慮しない
        //  bakaze: 場風（対子の符に関係する）
        //  zikaze: 自風（対子の符に関係する）
        public int getHu( Kaze bakaze, Kaze zikaze ) {
            switch ( type_ ) {
                case Type.None:
                    return 0;
                case Type.Toitsu:
                    int hu = 0;
                    // 場風で2、自風で2、役牌で2
                    if ( pais_[ 0 ].isKaze() == true ) {
                        if ( pais_[ 0 ].PaiType == ( int )bakaze ) {
                            hu += 2;
                        }
                        if ( pais_[ 0 ].PaiType == ( int )zikaze ) {
                            hu += 2;
                        }
                        if ( pais_[ 0 ].isYaku() == true ) {
                            hu = 2;
                        }
                    }
                    return hu;
                case Type.Shuntsu:
                    return 0;

                case Type.Koutsu: {
                    // 暗刻
                    //  張中牌　：4
                    //  ヤオ九牌：8
                    // 明刻
                    //  張中牌　：2
                    //  ヤオ九牌：4
                    int bai = bMinko_ ? 1 : 2;
                    if ( pais_[ 0 ].isTyunTyan() == true ) {
                        return 2 * bai;
                    }
                    return 4 * bai;
                }
                case Type.Kantsu: {
                    // 暗槓
                    //  張中牌　：8
                    //  ヤオ九牌：16
                    // 明槓
                    //  張中牌　：4
                    //  ヤオ九牌：8
                    int bai = bMinko_ ? 1 : 2;
                    if ( pais_[ 0 ].isTyunTyan() == true ) {
                        return 4 * bai;
                    }
                    return 8 * bai;
                }
            }
            return 0;
        }

        Pai[] pais_;
        Type type_ = Type.None;
        bool bMinko_ = false;
    }

    // 手配セット
    //  暗刻セット（七対子があるので最大7セット）と明刻セット（最大4セット）からなる
    public class PaiSet {
        public List< PaiGroup > ankouGroup_ = new List<PaiGroup>();   // 暗刻グループ
        public List<PaiGroup> minkoGroup_ = new List<PaiGroup>();     // 明刻グループ
    }

	// 手牌
	//  同じ手配でも複数の解釈がありえるので、登録された手配を解析して1つ以上のPaiSetを作る。
	public class Pais {
		// 手牌を追加
		//  暗刻は全牌を、暗槓及び明刻はグループ毎（最大4グループ）それぞれ追加
		//  詳細な役はここでは判断しない
		//  自摸牌もしくはロン牌が2つ以上設定されている場合は不正にせず最初に見つかった牌以外は自摸牌フラグを下す
		//  ankous：伏せ牌群。2～14牌まで（0,1,4,7,10牌は不正になる）
		//  minkous：暗槓及び明刻（鳴き）0～4つまで
		public bool setPais(List<Pai> ankous, List<PaiGroup> minkous) {
			// 上がり形として成立しているか解析
			//  手配成立（役無しも含む）は以下の通り：
			//   ① 暗刻群＋明刻群（4グループ）＋頭
			//   ② 七対子（対子7グループ）
			//   ③ 国士無双（国士無双1グループ）

			clear();

			if (minkous.Count >= 5) {
				return false;   // 5個以上の明刻は上がり形として不成立
			}
			foreach (var m in minkous) {
				if (m.isValid() == false)
					return false;   // 上がりとして不成立
				if (m.isMinko() == false && m.getType() != PaiGroup.Type.Kantsu) {
					// 槓子以外は明刻を認めない
					return false;
				}
			}

			// 牌の数が正しいか（少牌もしくは多牌していないか）チェック
			//  14 - 明刻群数 × 3 = 暗刻牌数
			if (14 - minkous.Count * 3 != ankous.Count) {
				return false;
			}

			PaiGroup.sort( ref ankous );

			// 暗刻牌が七対子になってる？
			{
				PaiSet paiSet = new PaiSet();
				if (minkous.Count == 0 && checkTitoitu( ankous, ref paiSet )) {
					paiSetList_.Add( paiSet );
				}
			}
			// 国士無双になってる？
			{
				PaiSet paiSet = new PaiSet();
				PaiGroup g = new PaiGroup();
				g.set( ankous.ToArray(), false );
				if (minkous.Count == 0 && g.getType() == PaiGroup.Type.Kokushi) {
					paiSet.ankouGroup_.Add( g );
					paiSetList_.Add( paiSet );
				}
			}
			// 面子群になってる？
			{
				checkMentsu( ankous, minkous, ref paiSetList_ );
			}

			// 牌セットが1セット以上出来ていれば上がり形成立
			return paiSetList_.Count > 0;
		}

		// 牌リセット
		public void clear() {
			paiSetList_ = new List<PaiSet>();
		}

		// 七対子になってる？
		bool checkTitoitu(List<Pai> ankous, ref PaiSet outPaiSet) {
			if (ankous.Count != 14) {
				return false;
			}
			for (int i = 0; i < 7; ++i) {
				if (ankous[ 2 * i ].PaiType != ankous[ 2 * i + 1 ].PaiType) {
					return false;   // 対子の成立無し
				}
				PaiGroup g = new PaiGroup();
				g.set( new Pai[] { ankous[ 2 * i ], ankous[ 2 * i + 1 ] }, false );
				outPaiSet.ankouGroup_.Add( g );
			}
			return true;
		}

		class GroupTree
		{
			public PaiGroup group_ = new PaiGroup();
			public List<GroupTree> nextGroupTrees_ = new List<GroupTree>();
		}

		// 面子群になってる？
		bool checkMentsu(List<Pai> ankous, List<PaiGroup> minkous, ref List<PaiSet> paiSetList) {
			// 索子、筒子、萬子、字牌グループに分ける。槓子は存在しない。
			// グループの何れかが3の倍数 + 1枚（1,4,7,11）の場合は面子は成立していない
			// 3の倍数枚の場合はその牌種に頭は無い
			// 3の倍数+2枚の牌種に頭が存在する
			var paiTypeList = new List<List<Pai>>();
			for (int i = 0; i < 4; ++i) {
				paiTypeList.Add( new List<Pai>() );
			}
			// 振り分け
			//  ankousがソート済みなので各リスト内もソートされている
			//   0: 索子
			//   1: 筒子
			//   2: 萬子
			//   3: 字牌
			foreach (var p in ankous) {
				if (p.isSouzu())
					paiTypeList[ 0 ].Add( p );
				else if (p.isPinzu())
					paiTypeList[ 1 ].Add( p );
				else if (p.isManzu())
					paiTypeList[ 2 ].Add( p );
				else if (p.isJi())
					paiTypeList[ 3 ].Add( p );
				else
					return false;	// 無効牌が含まれていた
			}
			foreach (var tl in paiTypeList) {
				if (tl.Count % 3 == 1)
					return false;   // 不成立
			}

			int toitsuCount = 0;
			var jiGroupList = new List<PaiGroup>();

			// 字牌が暗刻になっていない、対子が2つ以上ある場合は不成立
			if (paiTypeList[ 3 ].Count > 0) {
				int count = 0;
				var p = paiTypeList[ 3 ][ 0 ];
				for (int i = 0; i < paiTypeList[ 3 ].Count; ++i) {
					count++;    // 自分はカウント
								// 最後以外の牌で次の牌と同じだったら次へ
					if (i + 1 < paiTypeList[ 3 ].Count && paiTypeList[ 3 ][ i ].PaiType == paiTypeList[ 3 ][ i + 1 ].PaiType) {
						continue;
					} else {
						// 次が別の牌（＝自分が塊の最後）
						if (count == 1)
							return false;   // 不成立
						else if (count == 2) {
							// 対子
							toitsuCount++;
							if (toitsuCount >= 2)
								return false;   // 対子が複数ある
							PaiGroup g = new PaiGroup();
							g.set( new Pai[] { paiTypeList[ 3 ][ i ], paiTypeList[ 3 ][ i - 1 ] }, false );
							jiGroupList.Add( g );
							count = 0;
						} else if (count == 3) {
							// 暗刻
							PaiGroup g = new PaiGroup();
							g.set( new Pai[] { paiTypeList[ 3 ][ i ], paiTypeList[ 3 ][ i - 1 ], paiTypeList[ 3 ][ i - 2 ] }, false );
							jiGroupList.Add( g );
							count = 0;
						} else {
							// 4枚以上なので不成立
							return false;
						}
					}
				}
			}

			// 数牌の面子解析
			var mentsuGroups = new List< List< List< PaiGroup > > >();
			for (int i = 0; i < 3; ++i) {
				var groups = new List<List<PaiGroup>>();
				if (mentsuAnalyzeEntry( paiTypeList[ i ], ref groups ) == false) {
					return false;	// 不成立
				}
				// 後々の為ゼロの場合は空を追加しておく
				if ( groups.Count == 0 ) {
					groups.Add( new List<PaiGroup>() );
				}
				mentsuGroups.Add( groups );
			}

			// 字牌、面子が揃ったので組み合わせて手配作成
			int combNum = ( mentsuGroups[ 0 ].Count * mentsuGroups[ 1 ].Count * mentsuGroups[ 2 ].Count );
			for ( int i = 0; i < combNum; ++i ) {
				foreach ( var sg in mentsuGroups[ 0 ] ) {
					foreach (var pg in mentsuGroups[ 1 ] ) {
						foreach (var mg in mentsuGroups[ 2 ]) {
							var paiSet = new PaiSet();
							// 索子、筒子、萬子
							foreach ( var s in sg ) {
								paiSet.ankouGroup_.Add( s );
							}
							foreach ( var p in pg ) {
								paiSet.ankouGroup_.Add( p );
							}
							foreach ( var m in mg ) {
								paiSet.ankouGroup_.Add( m );
							}
							// 字牌と明刻
							foreach (var g in jiGroupList) {
								paiSet.ankouGroup_.Add( g );
							}
							foreach (var g in minkous) {
								paiSet.minkoGroup_.Add( g );
							}
							paiSetList_.Add( paiSet );
						}
					}
				}
			}

			return true;
		}

		void mentsuAnalyze( List<Pai> pais, ref GroupTree parentGroupTree ) {
			if ( pais.Count <= 1 )
				return;

			// 頭フェーズ
			if ( pais.Count == 2 ) {
				if ( pais[ 0 ].PaiType == pais[ 1 ].PaiType ) {
					// 対子成立
					PaiGroup g = new PaiGroup();
					g.set( pais, false );
					var tree = new GroupTree();
					tree.group_ = g;
					parentGroupTree.nextGroupTrees_.Add( tree );
				}
				return;
			}

			// 含まれている数値の種類別に刻子、順子を作成
			var nums = new HashSet<int>();
			foreach (var p in pais) {
				if (nums.Contains( p.PaiType ) == false) {
					nums.Add( p.PaiType );
				}
			}
			foreach (var n in nums) {
				// nの刻子が出来るかチェック
				List<Pai> subPais = new List<Pai>();
				List<Pai> koutsu = new List<Pai>();
				for (int i = 0; i < pais.Count; ++i) {
					if (pais[ i ].PaiType == n) {
						koutsu.Add( pais[ i ] );
						if (koutsu.Count == 3) {
							// 刻子成立
							// 残りをサブセットへ
							for (i = i + 1; i < pais.Count; ++i) {
								subPais.Add( pais[ i ] );
							}
							break;
						}
					} else {
						subPais.Add( pais[ i ] );
					}
				}
				if (koutsu.Count == 3) {
					// 刻子成立
					PaiGroup g = new PaiGroup();
					g.set( koutsu, false );
					var tree = new GroupTree();
					tree.group_ = g;
					parentGroupTree.nextGroupTrees_.Add( tree );
					// 残りのサブセットで再度解析
					mentsuAnalyze( subPais, ref tree );
					return;
				} else {
					// 刻子不成立
					// nを先頭にした順子が出来るかチェック
					if ( n  % 9 >= 7 ) {
						// 順子不成立
						continue;
					}
					subPais.Clear();
					List<Pai> shuntsu = new List<Pai>();
					int searchNum = n;
					for ( int i = 0; i < pais.Count; ++i ) {
						if (pais[ i ].PaiType == searchNum) {
							shuntsu.Add( pais[ i ] );
							if (shuntsu.Count == 3) {
								// 順子成立
								// 残りをサブセットへ
								for (i = i + 1; i < pais.Count; ++i) {
									subPais.Add( pais[ i ] );
								}
								break;
							}
							searchNum++;
						} else {
							subPais.Add( pais[ i ] );
						}
					}
					if ( shuntsu.Count == 3 ) {
						// 順子成立
						PaiGroup g = new PaiGroup();
						g.set( shuntsu, false );
						var tree = new GroupTree();
						tree.group_ = g;
						parentGroupTree.nextGroupTrees_.Add( tree );
						// 残りのサブセットで再度解析
						mentsuAnalyze( subPais, ref tree );
						return;
					}
				}
			}
		}

		// 面子解析開始
		bool mentsuAnalyzeEntry( List<Pai> pais, ref List< List< PaiGroup > > mentsuGroups ) {
			if ( pais.Count == 0 ) {
				return true;
			}
			if ( pais.Count % 3 == 1 ) {
				return false;	// 不成立
			}
			int mentsuNum = 0;
			if ( pais.Count % 3 == 0 )
				mentsuNum = pais.Count / 3;
			else
				mentsuNum = pais.Count / 3 + 1; // 頭分

			// 面子解析
			var root = new GroupTree();
			mentsuAnalyze( pais, ref root );

			// 確定した物のみ抽出
			var tmpGroups = new List<List<PaiGroup>>();
			foreach ( var g in root.nextGroupTrees_ ) {
				var groupStack = new Stack<PaiGroup>();
				collectMentsuGroups( g, groupStack , ref tmpGroups );
			}
			foreach ( var g in tmpGroups ) {
				if ( g.Count == mentsuNum )
					mentsuGroups.Add( g );
			}

			return mentsuGroups.Count > 0;
		}

		void collectMentsuGroups( GroupTree parent, Stack<PaiGroup> groupStack, ref List< List < PaiGroup > > groups ) {
			groupStack.Push( parent.group_ );
			if ( parent.nextGroupTrees_.Count == 0 ) {
				var list = new List<PaiGroup>();
				foreach ( var g in groupStack ) {
					list.Add( g );
				}
				groups.Add( list );
			} else {
				foreach ( var t in parent.nextGroupTrees_ ) {
					collectMentsuGroups( t, groupStack, ref groups );
				}
			}
			groupStack.Pop();
		}

        // 牌セットリストを取得
        public List<PaiSet> getPaiSetList() {
            return paiSetList_;
        }

		List<PaiSet> paiSetList_ = new List<PaiSet>();
    }

    // 役解析器
    public class MahjangScoreCalculator {
        public enum Yaku {
            // 役無し
            None,           // 役無し
            // 1飜役
            Menzentsumo,    // 門前自摸
            Rithi,          // 立直
            Ippatsu,        // 一発
            Tanyao,         // タンヤオ
            Pinhu,          // 平和
            Ipeko,          // 一盃口
            Haku,           // 白（役牌）
            Hatsu,          // 発（役牌）
            Tyun,           // 中（役牌）
            Ton,            // 東
            Nan,            // 南
            Sha,            // 西
            Pei,            // 北
            Chankan,        // 槍槓（チャンカン）
            Rinshankaiho,   // 嶺上開花（リンシャンカイホウ）
            Haiteiraoyue,   // 海底撈月（ハイテイラオユエ）
            Houteiraoyui,   // 河底撈魚（ホウテイラオユイ）

            // 2飜役
            Daburi,         // ダブリー
            Titoitsu,       // 七対子
            Dabuton,        // ダブ東
            Dabunan,        // ダブ南
            Dabusha,        // ダブ西
            Dabupei,        // ダブ北
            ToiToi,         // 対々和
            Sananko,        // 三暗刻
            Sanshokudouko,  // 三色同刻
            Sansyokudouzyun,// 三色同順
            Honroutou,      // 混老頭（ホンロウトウ）
            Ikkitukan,      // 一気通貫
            Tyanta,         // チャンタ
            Shousangen,     // 小三元
            Sankantsu,      // 三槓子（サンカンツ）

            // 3飜役
            Honiso,         // 混一色（ホンイーソー）
            Zyuntyan,       // 純チャン（ジュンチャン）
            Ryanpeiko,      // 二盃口

            // 5飜役
            NagashiMangan,  // 流し満貫

            // 6飜役
            Tiniso,         // 清一色（チンイーソー）

            // 役満
            Tenho,          // 天和
            Tiho,           // 地和
            Renho,          // 人和
            Ryuho,          // 緑一色
            Daisangen,      // 大三元
            Shosushi,       // 小四喜
            Tuiso,          // 字一色
            Kokushimusou,   // 国士無双                            ///
            Tyurenpoto,     // 九蓮宝燈（チューレンポートウ）
            Suanko,         // 四暗刻
            Tinrouto,       // 清老頭（チンロウトウ）
            Sukantsu,       // 四槓子

            // ダブル役満
            Suankotanki,    // 四暗刻単騎
            Daisushi,       // 大四喜
            ZyunseiTyurenpoto,  // 純正九蓮宝燈（ジュンセイチューレンポートウ）
            KokushiMusou13, // 国士無双十三面待ち（コクシムソウジュウサンメンマチ）   ///
        }

        public class YakuUnit {
            public Yaku yaku_ = Yaku.None;
            public int score_ = 0;
            public int han_ = 0;
            public bool bYakuman_ = false;
        }

        public class YakuData {
            public bool bValid_ = true;    // 役成立
            public List<YakuUnit> yakuList_ = new List<YakuUnit>();
        }

        // 役と点数を解析
        //  ankous : 伏せ牌
        //  minkous: 明刻及び槓子（暗槓、明槓）
        //  isOya  : 親？
        public bool analyze( List< Pai > ankous, List< PaiGroup > minkous, bool isOya ) {
            Pais tehai = new Pais();
            if ( tehai.setPais( ankous, minkous ) == false ) {
                // 成立していない
                return false;
            }

            var paiSetList = tehai.getPaiSetList();
            List<YakuData> yakuDataList = new List<YakuData>();
            foreach ( var ps in paiSetList ) {
                YakuData yakuData = null;
                if ( analyzeYaku( ps, isOya, out yakuData ) == true ) {
                    yakuDataList.Add( yakuData );
                }
            }

            return true;
        }

        // 役を解析
        bool analyzeYaku( PaiSet paiSet, bool isOya, out YakuData data ) {
            data = new YakuData();
            float oyaRate = isOya ? 1.5f : 1.0f;

            // 国士無双？
            if ( paiSet.ankouGroup_[ 0 ].getType() == PaiGroup.Type.Kokushi ) {
                // 13面待ち？
                var pais = paiSet.ankouGroup_[ 0 ].getPais();
                Pai agari = paiSet.ankouGroup_[ 0 ].getAgarihai();
                if ( agari == null ) {
                    data.bValid_ = false;
                    return false;   // 上がり牌が無い
                }
                foreach ( var p in pais ) {
                    if ( p != agari && p.PaiType == agari.PaiType ) {
                        // 13面待ちなのでダブル役満
                        data.yakuList_.Add( new YakuUnit { yaku_ = Yaku.KokushiMusou13, score_ = ( int )( 8000 * 8 * oyaRate ), bYakuman_ = true } );
                        return true;
                    }
                }
                // 単騎待ち通常国士無双
                data.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Kokushimusou, score_ = ( int )( 8000 * 4 * oyaRate ), bYakuman_ = true } );
                return true;
            }


            return true;
        }
    }
}