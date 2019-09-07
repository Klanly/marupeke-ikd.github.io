using System.Collections;
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

        public Pai(int paiType) {
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
        public void setAsMark(string mark) {
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
                ( PaiType >= M1 + 1 && PaiType <= M9 - 1 );
        }

        // ヤオ九牌？(1,9,字牌）
        public bool isYaotyu() {
            if ( isValid() == false ) {
                return false;
            }
            return !isTyunTyan();
        }

        // 1,9牌？
        public bool is1_9() {
            if ( isValid() == false ) {
                return false;
            }
            return !isTyunTyan() && !isJi();
        }

        // 何れかと一致？
        public bool isThere( int[] paiTypes ) {
            foreach ( int t in paiTypes ) {
                if ( paiType_ == t )
                    return true;
            }
            return false;
        }

        // 数字を取得
        public int toNumber() {
            if ( isValid() == false || isJi() == true ) {
                return -1;
            }
            return ( PaiType % 9 ) + 1;
        }

        // タイプ取得
        public Type getType() {
            if ( isValid() == false )
                return Type.None;
            if ( PaiType >= S1 && PaiType <= S9 )
                return Type.Sozu;
            else if ( PaiType >= P1 && PaiType <= P9 )
                return Type.Pinzu;
            else if ( PaiType >= M1 && PaiType <= M9 )
                return Type.Manzu;
            return Type.Ji;
        }

        // 順子として次の牌が成立している？
        public bool isNext(Pai next) {
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
        static public bool isToitsu(Pai[] pais) {
            if ( pais.Length != 2 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false ) {
                return false;
            }
            return ( pais[ 0 ].PaiType == pais[ 1 ].PaiType );
        }

        // 順子？
        static public bool isShuntsu(Pai[] pais, bool doSort = true) {
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
        static public bool isKoutsu(Pai[] pais) {
            if ( pais.Length != 3 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false || pais[ 2 ].isValid() == false ) {
                return false;
            }
            return ( pais[ 0 ].PaiType == pais[ 1 ].PaiType && pais[ 1 ].PaiType == pais[ 2 ].PaiType );
        }

        // 槓子？
        static public bool isKantss(Pai[] pais) {
            if ( pais.Length != 4 ) {
                return false;
            }
            if ( pais[ 0 ].isValid() == false || pais[ 1 ].isValid() == false || pais[ 2 ].isValid() == false || pais[ 3 ].isValid() == false ) {
                return false;
            }
            return ( pais[ 0 ].PaiType == pais[ 1 ].PaiType && pais[ 1 ].PaiType == pais[ 2 ].PaiType && pais[ 2 ].PaiType == pais[ 3 ].PaiType );
        }

        // 国士無双？
        static public bool isKokushi(Pai[] pais, bool doSort = true) {
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

        // 同じ牌のみの構成？（対子、刻子、槓子）
        public bool isSamePai() {
            return ( type_ == Type.Kantsu || type_ == Type.Koutsu || type_ == Type.Toitsu );
        }

        // 同じ牌のみで3牌以上の構成？（刻子、槓子）
        public bool isKoutsuPai() {
            return ( type_ == Type.Kantsu || type_ == Type.Koutsu );
        }

        // 面子をセット
        // isMinko : 明刻？
        public Type set(Pai[] pais, bool isMinko) {
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
            calcHash(); // ハッシュ値算出
            return type_;
        }

        public Type set(List<Pai> pais, bool isMinko) {
            var list = pais.ToArray();
            return set( list, isMinko );
        }

        public Type set(List<Pai> pais, int s, int num, bool isMinko) {
            var list = new List<Pai>();
            for ( int i = 0; i < num; ++i )
                list.Add( pais[ s + i ] );
            return set( list, isMinko );
        }

        // 牌を昇順に並べ替える
        //  無効牌は最後尾に並ぶようにする
        public static void sort(ref Pai[] pais) {
            pais = pais.OrderBy( (pai) => {
                if ( pai.PaiType == -1 ) {
                    return 100;
                }
                return pai.PaiType;
            } ).ToArray();
        }

        // 牌を昇順に並べ替える
        //  無効牌は最後尾に並ぶようにする
        public static void sort(ref List<Pai> pais) {
            pais.Sort( (l, r) => {
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

        // ハッシュ値を取得
        public long getHash() {
            return hash_;
        }

        // 符数を取得
        //  待ちは考慮しない
        //  bakaze: 場風（対子の符に関係する）
        //  zikaze: 自風（対子の符に関係する）
        public int getHu(Kaze bakaze, Kaze zikaze) {
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

        // ハッシュ値算出
        void calcHash() {
            // Noneは考えない事にする
            if ( type_ == Type.None ) {
                hash_ = -1;
                return;
            }
            // 1-6: 牌の種類(0～33)。順子は最初の牌
            // 7-8: 門面種（00:対子、01:順子、10:刻子、11:槓子）
            // 9  : 明刻（0:明刻、1:暗刻）
            // 10 : 国士無双（他のビットは全部0）
            int tn = 0;
            switch ( type_ ) {
                case Type.Shuntsu:
                    tn = 1;
                    break;
                case Type.Koutsu:
                    tn = 2;
                    break;
                case Type.Kantsu:
                    tn = 3;
                    break;
                case Type.Kokushi:
                    hash_ = 1 << 9;
                    return;
            }
            int mn = ( bMinko_ ? 0 : 1 );
            int p = pais_[ 0 ].PaiType;

            hash_ = p | ( tn << 6 ) | ( mn << 8 );
        }

        Pai[] pais_;
        Type type_ = Type.None;
        bool bMinko_ = false;
        long hash_ = -1;
    }

    // 手配セット
    //  暗刻セット（七対子があるので最大7セット）と明刻セット（最大4セット）からなる
    public class PaiSet {
        public List<PaiGroup> ankouGroup_ = new List<PaiGroup>();   // 暗刻グループ
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

            if ( minkous.Count >= 5 ) {
                return false;   // 5個以上の明刻は上がり形として不成立
            }
            foreach ( var m in minkous ) {
                if ( m.isValid() == false )
                    return false;   // 上がりとして不成立
                if ( m.isMinko() == false && m.getType() != PaiGroup.Type.Kantsu ) {
                    // 槓子以外は明刻を認めない
                    return false;
                }
            }

            // 牌の数が正しいか（少牌もしくは多牌していないか）チェック
            //  14 - 明刻群数 × 3 = 暗刻牌数
            if ( 14 - minkous.Count * 3 != ankous.Count ) {
                return false;
            }

            PaiGroup.sort( ref ankous );

            // 暗刻牌が七対子になってる？
            {
                PaiSet paiSet = new PaiSet();
                if ( minkous.Count == 0 && checkTitoitu( ankous, ref paiSet ) ) {
                    paiSetList_.Add( paiSet );
                }
            }
            // 国士無双になってる？
            {
                PaiSet paiSet = new PaiSet();
                PaiGroup g = new PaiGroup();
                g.set( ankous.ToArray(), false );
                if ( minkous.Count == 0 && g.getType() == PaiGroup.Type.Kokushi ) {
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
            if ( ankous.Count != 14 ) {
                return false;
            }
            for ( int i = 0; i < 7; ++i ) {
                if ( ankous[ 2 * i ].PaiType != ankous[ 2 * i + 1 ].PaiType ) {
                    return false;   // 対子の成立無し
                }
                PaiGroup g = new PaiGroup();
                g.set( new Pai[] { ankous[ 2 * i ], ankous[ 2 * i + 1 ] }, false );
                outPaiSet.ankouGroup_.Add( g );
            }
            return true;
        }

        class GroupTree {
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
            for ( int i = 0; i < 4; ++i ) {
                paiTypeList.Add( new List<Pai>() );
            }
            // 振り分け
            //  ankousがソート済みなので各リスト内もソートされている
            //   0: 索子
            //   1: 筒子
            //   2: 萬子
            //   3: 字牌
            foreach ( var p in ankous ) {
                if ( p.isSouzu() )
                    paiTypeList[ 0 ].Add( p );
                else if ( p.isPinzu() )
                    paiTypeList[ 1 ].Add( p );
                else if ( p.isManzu() )
                    paiTypeList[ 2 ].Add( p );
                else if ( p.isJi() )
                    paiTypeList[ 3 ].Add( p );
                else
                    return false;   // 無効牌が含まれていた
            }
            foreach ( var tl in paiTypeList ) {
                if ( tl.Count % 3 == 1 )
                    return false;   // 不成立
            }

            int toitsuCount = 0;
            var jiGroupList = new List<PaiGroup>();

            // 字牌が暗刻になっていない、対子が2つ以上ある場合は不成立
            if ( paiTypeList[ 3 ].Count > 0 ) {
                int count = 0;
                var p = paiTypeList[ 3 ][ 0 ];
                for ( int i = 0; i < paiTypeList[ 3 ].Count; ++i ) {
                    count++;    // 自分はカウント
                                // 最後以外の牌で次の牌と同じだったら次へ
                    if ( i + 1 < paiTypeList[ 3 ].Count && paiTypeList[ 3 ][ i ].PaiType == paiTypeList[ 3 ][ i + 1 ].PaiType ) {
                        continue;
                    } else {
                        // 次が別の牌（＝自分が塊の最後）
                        if ( count == 1 )
                            return false;   // 不成立
                        else if ( count == 2 ) {
                            // 対子
                            toitsuCount++;
                            if ( toitsuCount >= 2 )
                                return false;   // 対子が複数ある
                            PaiGroup g = new PaiGroup();
                            g.set( new Pai[] { paiTypeList[ 3 ][ i ], paiTypeList[ 3 ][ i - 1 ] }, false );
                            jiGroupList.Add( g );
                            count = 0;
                        } else if ( count == 3 ) {
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
            var mentsuGroups = new List<List<List<PaiGroup>>>();
            for ( int i = 0; i < 3; ++i ) {
                var groups = new List<List<PaiGroup>>();
                if ( mentsuAnalyzeEntry( paiTypeList[ i ], ref groups ) == false ) {
                    return false;   // 不成立
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
                    foreach ( var pg in mentsuGroups[ 1 ] ) {
                        foreach ( var mg in mentsuGroups[ 2 ] ) {
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
                            foreach ( var g in jiGroupList ) {
                                paiSet.ankouGroup_.Add( g );
                            }
                            foreach ( var g in minkous ) {
                                paiSet.minkoGroup_.Add( g );
                            }
                            paiSetList_.Add( paiSet );
                        }
                    }
                }
            }

            return true;
        }

        void mentsuAnalyze(List<Pai> pais, ref GroupTree parentGroupTree) {
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
            foreach ( var p in pais ) {
                if ( nums.Contains( p.PaiType ) == false ) {
                    nums.Add( p.PaiType );
                }
            }
            foreach ( var n in nums ) {
                // nの刻子が出来るかチェック
                List<Pai> subPais = new List<Pai>();
                List<Pai> koutsu = new List<Pai>();
                for ( int i = 0; i < pais.Count; ++i ) {
                    if ( pais[ i ].PaiType == n ) {
                        koutsu.Add( pais[ i ] );
                        if ( koutsu.Count == 3 ) {
                            // 刻子成立
                            // 残りをサブセットへ
                            for ( i = i + 1; i < pais.Count; ++i ) {
                                subPais.Add( pais[ i ] );
                            }
                            break;
                        }
                    } else {
                        subPais.Add( pais[ i ] );
                    }
                }
                if ( koutsu.Count == 3 ) {
                    // 刻子成立
                    PaiGroup g = new PaiGroup();
                    g.set( koutsu, false );
                    var tree = new GroupTree();
                    tree.group_ = g;
                    parentGroupTree.nextGroupTrees_.Add( tree );
                    // 残りのサブセットで再度解析
                    mentsuAnalyze( subPais, ref tree );
                }

                if ( n % 9 >= 7 ) {
                    // 順子不成立
                    continue;
                }

                // nを先頭にした順子が出来るかチェック
                subPais.Clear();
                List<Pai> shuntsu = new List<Pai>();
                int searchNum = n;
                for ( int i = 0; i < pais.Count; ++i ) {
                    if ( pais[ i ].PaiType == searchNum ) {
                        shuntsu.Add( pais[ i ] );
                        if ( shuntsu.Count == 3 ) {
                            // 順子成立
                            // 残りをサブセットへ
                            for ( i = i + 1; i < pais.Count; ++i ) {
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
                }
            }
        }

        // 面子解析開始
        bool mentsuAnalyzeEntry(List<Pai> pais, ref List<List<PaiGroup>> mentsuGroups) {
            if ( pais.Count == 0 ) {
                return true;
            }
            if ( pais.Count % 3 == 1 ) {
                return false;   // 不成立
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
                collectMentsuGroups( g, groupStack, ref tmpGroups );
            }
            var sameMenzenChecker = new HashSet<long>();
            foreach ( var g in tmpGroups ) {
                if ( g.Count == mentsuNum ) {
                    g.Sort( (a, b) => {
                        if ( a.getHash() == b.getHash() )
                            return 0;
                        return ( a.getHash() < b.getHash() ? -1 : 1 );
                    } );
                    long hash = 0;
                    for ( int i = 0; i < g.Count; ++i ) {
                        hash |= ( g[ i ].getHash() << ( i * 9 ) );
                    }
                    if ( sameMenzenChecker.Contains( hash ) == false ) {
                        mentsuGroups.Add( g );
                        sameMenzenChecker.Add( hash );
                    }
                }
            }

            return mentsuGroups.Count > 0;
        }

        void collectMentsuGroups(GroupTree parent, Stack<PaiGroup> groupStack, ref List<List<PaiGroup>> groups) {
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
            Titoitsu,       // 七対子                   ///
            Dabuton,        // ダブ東                   ///
            Dabunan,        // ダブ南                   ///
            Dabusha,        // ダブ西                   ///
            Dabupei,        // ダブ北                   ///
            ToiToi,         // 対々和                   ///
            Sananko,        // 三暗刻                   ///
            Sanshokudouko,  // 三色同刻                 ///
            Sansyokudouzyun,// 三色同順                 ///
            Honroutou,      // 混老頭（ホンロウトウ）   ///
            Ikkitukan,      // 一気通貫                 ///
            Tyanta,         // チャンタ                 ///
            Shousangen,     // 小三元                   /// 
            Sankantsu,      // 三槓子（サンカンツ）     ///

            // 3飜役
            Honiso,         // 混一色（ホンイーソー）   ///
            Zyuntyan,       // 純チャン（ジュンチャン） ///
            Ryanpeiko,      // 二盃口                   ///

            // 5飜役
            NagashiMangan,  // 流し満貫

            // 6飜役
            Tiniso,         // 清一色（チンイーソー）   ///

            // 役満
            Tenho,          // 天和
            Tiho,           // 地和
            Renho,          // 人和
            Ryuiso,         // 緑一色        ///
            Daisangen,      // 大三元        ///
            Shosushi,       // 小四喜        ///
            Tuiso,          // 字一色                              ///
            Kokushimusou,   // 国士無双                            ///
            Tyurenpoto,     // 九蓮宝燈（チューレンポートウ）      ///
            Suanko,         // 四暗刻                              ///
            Tinrouto,       // 清老頭（チンロウトウ）              ///
            Sukantsu,       // 四槓子                              ///

            // ダブル役満
            Suankotanki,    // 四暗刻単騎          ///
            Daisushi,       // 大四喜              ///
            ZyunseiTyurenpoto,  // 純正九蓮宝燈（ジュンセイチューレンポートウ）       ///
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

        // 場の状態
        public class BaState {
            public Kaze bakaze_ = Kaze.Ton;    // 場風
            public Kaze zikaze_ = Kaze.Ton;    // 上がり手の風
            public int ponba_ = 1;             // 本場数
            public bool bOya_ = false; // 上り手は親？

            // 点数レート
            public float rate() {
                return bOya_ ? 1.5f : 1.0f;
            }
        }

        // 役と点数を解析
        //  ankous : 伏せ牌
        //  minkous: 明刻及び槓子（暗槓、明槓）
        //  isOya  : 親？
        public bool analyze(List<Pai> ankous, List<PaiGroup> minkous, BaState baState) {
            Pais tehai = new Pais();
            if ( tehai.setPais( ankous, minkous ) == false ) {
                // 成立していない
                return false;
            }

            var paiSetList = tehai.getPaiSetList();
            List<YakuData> yakuDataList = new List<YakuData>();
            foreach ( var ps in paiSetList ) {
                YakuData yakuData = null;
                if ( analyzeYaku( ps, baState, out yakuData ) == true ) {
                    yakuDataList.Add( yakuData );
                }
            }

            return true;
        }

        // 役を解析
        bool analyzeYaku(PaiSet paiSet, BaState baState, out YakuData data) {
            data = new YakuData();

            // 国士無双？
            if ( judge_Kokushimusou( paiSet, baState, data ) == true ) {
                return true;
            }

            judge_Tinrouto( paiSet, baState, data );    // 清老頭
            judge_Tuiso( paiSet, baState, data );       // 字一色
            judge_Ryuiso( paiSet, baState, data );      // 緑一色

            // 七対子？
            if ( judge_Titoitsu( paiSet, baState, data ) == true ) {
            } else {
                // 九蓮宝燈？
                if ( judge_Tyurenpoto( paiSet, baState, data ) == true ) {
                    return true;
                }
                judge_Suanko( paiSet, baState, data );      // 四暗刻
                judge_Daisushi( paiSet, baState, data );    // 大四喜
                judge_Sukantsu( paiSet, baState, data );    // 四槓子
                judge_Shosushi( paiSet, baState, data );    // 小四喜
                judge_Daisangen( paiSet, baState, data );   // 大三元

                judge_Ryanpeiko( paiSet, baState, data );   // 二盃口
            }

            judge_Tiniso( paiSet, baState, data );       // 清一色
            judge_Zyuntyan( paiSet, baState, data );     // 純チャン
            judge_Honiso( paiSet, baState, data );       // 混一色（ホンイーソー）
            judge_Sankantsu( paiSet, baState, data );    // 三槓子
            judge_Shousangen( paiSet, baState, data );   // 小三元
            judge_Tyanta( paiSet, baState, data );       // チャンタ
            judge_Ikkitukan( paiSet, baState, data );    // 一気通貫
            judge_Honroutou( paiSet, baState, data );    // 混老頭
            judge_Sansyokudouzyun( paiSet, baState, data );  // 三色同順
            judge_Sanshokudouko( paiSet, baState, data );    // 三色同刻
            judge_Sananko( paiSet, baState, data );          // 三暗刻
            judge_ToiToi( paiSet, baState, data );           // 対々和
            judge_DabuKaze( paiSet, baState, data );         // ダブ風

            return true;
        }

        // 国士無双？
        bool judge_Kokushimusou(PaiSet paiSet, BaState baState, YakuData yakuData) {
            if ( paiSet.ankouGroup_[ 0 ].getType() == PaiGroup.Type.Kokushi ) {
                // 13面待ち？
                var pais = paiSet.ankouGroup_[ 0 ].getPais();
                Pai agari = paiSet.ankouGroup_[ 0 ].getAgarihai();
                if ( agari == null ) {
                    yakuData.bValid_ = false;
                    return false;   // 上がり牌が無い
                }
                foreach ( var p in pais ) {
                    if ( p != agari && p.PaiType == agari.PaiType ) {
                        // 13面待ちなのでダブル役満
                        yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.KokushiMusou13, score_ = ( int )( 8000 * 8 * baState.rate() ), bYakuman_ = true } );
                        return true;
                    }
                }
                // 単騎待ち通常国士無双
                yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Kokushimusou, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
                return true;
            }
            return false;
        }

        // 九蓮宝燈？
        bool judge_Tyurenpoto(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 全て伏せ牌
            // 全て同じ種類の数牌
            // 11123456789999 + 任意の数牌
            if ( paiSet.minkoGroup_.Count > 0 ) {
                return false;
            }
            Pai agari = null;
            var nums = new Dictionary<int, int>();
            Pai def = paiSet.ankouGroup_[ 0 ].getPais()[ 0 ];
            var type = def.getType();
            if ( type == Pai.Type.None || type == Pai.Type.Ji ) {
                return false;
            }
            foreach ( var g in paiSet.ankouGroup_ ) {
                if ( agari == null ) {
                    agari = g.getAgarihai();
                }
                foreach ( var p in g.getPais() ) {
                    if ( p.isNumber() == false || p.getType() != type ) {
                        return false;
                    }
                    int n = p.toNumber();
                    if ( nums.ContainsKey( n ) == false ) {
                        nums[ n ] = 0;
                    }
                    nums[ n ]++;
                }
            }
            if ( nums.Count != 9 ) {
                return false;   // 種類が足りない
            }
            int[] nset = new int[] { 3, 1, 1, 1, 1, 1, 1, 1, 3 };
            int toitsuNum = 0;
            for ( int i = 1; i <= 9; ++i ) {
                nums[ i ] -= nset[ i - 1 ];
                if ( nums[ i ] < 0 ) {
                    return false;
                }
                if ( nums[ i ] == 1 ) {
                    toitsuNum = i;  // 対子の数
                }
            }
            // 九連確定
            if ( toitsuNum == agari.toNumber() ) {
                // 純正九連
                yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.ZyunseiTyurenpoto, score_ = ( int )( 8000 * 8 * baState.rate() ), bYakuman_ = true } );
                return true;
            }
            // 九連
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Tyurenpoto, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 四暗刻？
        bool judge_Suanko(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // すべて暗刻
            bool isTanki = false;
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isMinko() == true || pg.getType() != PaiGroup.Type.Kantsu )
                    return false;
            }
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isMinko() == true || pg.isSamePai() == false )
                    return false;
                if ( pg.getAgarihai() != null && pg.getType() == PaiGroup.Type.Toitsu ) {
                    isTanki = true;
                }
            }
            // 確定
            // 単騎？
            if ( isTanki == true ) {
                yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Suankotanki, score_ = ( int )( 8000 * 8 * baState.rate() ), bYakuman_ = true } );
                return true;
            }

            // 通常
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Suanko, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 大四喜？
        bool judge_Daisushi(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 暗刻、明刻に風牌4種類
            int kazeNum = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( ( pg.getType() == PaiGroup.Type.Koutsu || pg.getType() == PaiGroup.Type.Kantsu ) && pg.getPais()[ 0 ].isKaze() == true ) {
                    kazeNum++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( ( pg.getType() == PaiGroup.Type.Koutsu || pg.getType() == PaiGroup.Type.Kantsu ) && pg.getPais()[ 0 ].isKaze() == true ) {
                    kazeNum++;
                }
            }
            if ( kazeNum != 4 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Daisushi, score_ = ( int )( 8000 * 8 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 四槓子？
        bool judge_Sukantsu(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 槓子が4つ
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.getType() == PaiGroup.Type.Kantsu ) {
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.getType() == PaiGroup.Type.Kantsu ) {
                    num++;
                }
            }
            if ( num != 4 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Sukantsu, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 清老頭
        bool judge_Tinrouto(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // １,9牌のみの手、全て刻子
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isSamePai() == false || pg.getPais()[ 0 ].is1_9() == false ) {
                    return false;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isSamePai() == false || pg.getPais()[ 0 ].is1_9() == false ) {
                    return false;
                }
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Tinrouto, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 字一色
        bool judge_Tuiso(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 字牌のみ、すべて刻子
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isSamePai() == false || pg.getPais()[ 0 ].isJi() == false ) {
                    return false;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isSamePai() == false || pg.getPais()[ 0 ].isJi() == false ) {
                    return false;
                }
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Tuiso, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 小四喜
        bool judge_Shosushi(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 風牌の頭一つ、他の風牌が刻子
            int num = 0;
            bool isToitsu = false;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isSamePai() == true && pg.getPais()[ 0 ].isKaze() == true ) {
                    num++;
                    if ( pg.getType() == PaiGroup.Type.Toitsu ) {
                        isToitsu = true;
                    }
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isSamePai() == true && pg.getPais()[ 0 ].isKaze() == true ) {
                    num++;
                }
            }
            if ( num != 4 || isToitsu == false ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Shosushi, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 大三元
        bool judge_Daisangen(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 役牌が3つ刻子
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isKoutsuPai() == true && pg.getPais()[ 0 ].isYaku() == true ) {
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isKoutsuPai() == true && pg.getPais()[ 0 ].isYaku() == true ) {
                    num++;
                }
            }
            if ( num != 3 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Daisangen, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 緑一色
        bool judge_Ryuiso(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 発,2,3,4,6,8のみで構成
            int[] ps = new int[] { Pai.S1 + 1, Pai.S1 + 2, Pai.S1 + 3, Pai.S1 + 5, Pai.S1 + 7, Pai.Ht };
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var pais = pg.getPais();
                foreach ( var p in pais ) {
                    if ( p.isThere( ps ) == false )
                        return false;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var pais = pg.getPais();
                foreach ( var p in pais ) {
                    if ( p.isThere( ps ) == false )
                        return false;
                }
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Ryuiso, score_ = ( int )( 8000 * 4 * baState.rate() ), bYakuman_ = true } );
            return true;
        }

        // 清一色
        bool judge_Tiniso(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 索子、筒子、萬子いずれかのみで構成
            //  門前6ハン、鳴き5ハン
            // 判定除外：各種役満
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            var unit = paiSet.ankouGroup_[ 0 ].getPais()[ 0 ];
            if ( unit.isNumber() == false )
                return false;
            var type = unit.getType();
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var pais = pg.getPais();
                foreach ( var p in pais ) {
                    if ( p.getType() != type )
                        return false;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var pais = pg.getPais();
                foreach ( var p in pais ) {
                    if ( p.getType() != type )
                        return false;
                }
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Tiniso, han_ = 6 + ( paiSet.minkoGroup_.Count > 0 ? -1 : 0 ), bYakuman_ = false } );
            return true;
        }

        // 七対子
        bool judge_Titoitsu(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 対子が7個（種類は問わない、同じ対子が複数も許可）
            if ( paiSet.ankouGroup_.Count != 7 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Titoitsu, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // 二盃口
        bool judge_Ryanpeiko(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 同型の順子が2組（暗刻のみ）
            // 判定除外：各種役満
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            if ( paiSet.ankouGroup_ .Count < 5 ) {
                return false;
            }
            var shuntus = new List<PaiGroup>();
            for ( int i = 0; i < paiSet.ankouGroup_.Count; ++i ) {
                var pg = paiSet.ankouGroup_[ i ];
                if ( pg.getType() == PaiGroup.Type.Toitsu )
                    continue;
                if ( pg.getType() != PaiGroup.Type.Shuntsu )
                    return false;
                shuntus.Add( pg );
            }
            // 1個目
            var idx = new List<int> { 1, 2, 3 };
            var pg0 = shuntus[ 0 ].getPais()[ 0 ].PaiType;
            for ( int j = 1; j < shuntus.Count; ++j ) {
                var pg1 = shuntus[ j ].getPais()[ 0 ].PaiType;
                if ( pg0 == pg1 ) {
                    // 同型順子あり
                    idx.Remove( j );
                    break;
                }
            }
            if ( idx.Count != 2 ) {
                return false;
            }

            // 2個目
            var pg1_0 = shuntus[ idx[ 0 ] ].getPais()[ 0 ].PaiType;
            var pg1_1 = shuntus[ idx[ 1 ] ].getPais()[ 0 ].PaiType;
            if ( pg1_0 != pg1_1 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Ryanpeiko, han_ = 3, bYakuman_ = false } );
            return true;
        }

        // 純チャン
        bool judge_Zyuntyan( PaiSet paiSet, BaState baState, YakuData yakuData ) {
            // 1,9の刻子、及び123,789の順子で構成
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var pais = pg.getPais();
                foreach ( var pi in pais ) {
                    if ( pi.isNumber() == false )
                        return false;
                }
                var p = pg.getPais()[ 0 ];
                if ( pg.isSamePai() == true ) {
                    if ( p.isTyunTyan() == true ) {
                        return false;
                    }
                    num++;
                    continue;
                } else if ( pg.getType() == PaiGroup.Type.Shuntsu && ( p.toNumber() == 1 || p.toNumber() == 7 ) ) {
                    num++;
                    continue;
                }
                return false;
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var pais = pg.getPais();
                foreach ( var pi in pais ) {
                    if ( pi.isNumber() == false )
                        return false;
                }
                var p = pg.getPais()[ 0 ];
                if ( pg.isSamePai() == true ) {
                    if ( p.isTyunTyan() == true ) {
                        return false;
                    }
                    num++;
                    continue;
                } else if ( pg.getType() == PaiGroup.Type.Shuntsu && ( p.toNumber() == 1 || p.toNumber() == 7 ) ) {
                    num++;
                    continue;
                }
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Zyuntyan, han_ = 3 + ( paiSet.minkoGroup_.Count > 0 ? -1 : 0 ), bYakuman_ = false } );
            return true;
        }

        // 混一色（ホンイーソー）
        bool judge_Honiso(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 索子、筒子、萬子のどれか1種類＋字牌（刻子）
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            // 色判定
            Pai numPai = null;
            bool detect = false;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var ps = pg.getPais();
                foreach ( var p in ps ) {
                    if ( p.isNumber() == true ) {
                        numPai = p;
                        detect = true;
                        break;
                    }
                }
                if ( detect == true ) {
                    break;
                }
            }
            if ( detect == false ) {
                foreach ( var pg in paiSet.minkoGroup_ ) {
                    var ps = pg.getPais();
                    foreach ( var p in ps ) {
                        if ( p.isNumber() == true ) {
                            numPai = p;
                            detect = true;
                            break;
                        }
                    }
                    if ( detect == true ) {
                        break;
                    }
                }
            }
            if ( detect == false ) {
                return false;
            }

            foreach ( var pg in paiSet.ankouGroup_ ) {
                var pais = pg.getPais();
                var p = pg.getPais()[ 0 ];
                if ( p.isNumber() == true && p.getType() != numPai.getType() ) {
                    return false;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var pais = pg.getPais();
                var p = pg.getPais()[ 0 ];
                if ( p.isNumber() == true && p.getType() != numPai.getType() ) {
                    return false;
                }
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Honiso, han_ = 3 + ( paiSet.minkoGroup_.Count > 0 ? -1 : 0 ), bYakuman_ = false } );
            return true;
        }

        // 三槓子
        bool judge_Sankantsu(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 槓子が3つ
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.getType() == PaiGroup.Type.Kantsu ) {
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.getType() == PaiGroup.Type.Kantsu ) {
                    num++;
                }
            }
            if ( num != 3 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Sankantsu, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // 小三元
        bool judge_Shousangen( PaiSet paiSet, BaState baState, YakuData yakuData ) {
            // 役牌2つ刻子＋頭
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isSamePai() && pg.getPais()[ 0 ].isYaku() == true ) {
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isSamePai() && pg.getPais()[ 0 ].isYaku() == true ) {
                    num++;
                }
            }
            if ( num != 3 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Shousangen, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // チャンタ
        bool judge_Tyanta(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 刻子は1,9,字牌で、順子は123,789で構成
            //  1,9のみだと純チャンになるので必ず1つ以上の字牌が含まれる -> 純チャンがある場合は無視
            //  順子が無いと混老頭になるので1つ以上の順子必須
            if ( containsYakuman( yakuData.yakuList_ ) == true || containsYaku( yakuData.yakuList_, Yaku.Zyuntyan, Yaku.Honroutou ) == true ){
                return false;
            }
            int num = 0;
            int shuntusNum = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isSamePai() && p.isYaotyu() == true ) {
                    num++;
                    continue;
                } else if ( pg.getType() == PaiGroup.Type.Shuntsu && ( p.toNumber() == 1 || p.toNumber() == 7 ) ) {
                    num++;
                    shuntusNum++;
                    continue;
                }
                return false;
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isSamePai() && p.isYaotyu() == true ) {
                    num++;
                    continue;
                } else if ( pg.getType() == PaiGroup.Type.Shuntsu && ( p.toNumber() == 1 || p.toNumber() == 7 ) ) {
                    num++;
                    shuntusNum++;
                    continue;
                }
                return false;
            }
            if ( num != 5 || shuntusNum == 0 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Tyanta, han_ = 2 + ( paiSet.minkoGroup_.Count > 0 ? -1 : 0 ), bYakuman_ = false } );
            return true;
        }

        // 一気通貫
        bool judge_Ikkitukan( PaiSet paiSet, BaState baState, YakuData yakuData ) {
            // 同じ色で123 456 789（明刻でもOK）
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            // 順子をビットでチェック
            //  0: 123
            //  1: 456
            //  2: 789
            // 値が7だったら役成立
            var check = new Dictionary<Pai.Type, int> {
                { Pai.Type.Sozu, 0 },
                { Pai.Type.Pinzu, 0 },
                { Pai.Type.Manzu, 0 },
            };
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.getType() == PaiGroup.Type.Shuntsu && ( p.toNumber() % 3 == 1 ) ) {
                    check[ p.getType() ] |= ( 1 << ( p.toNumber() / 3 ) );
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.getType() == PaiGroup.Type.Shuntsu && ( p.toNumber() % 3 == 1 ) ) {
                    check[ p.getType() ] |= ( 1 << ( p.toNumber() / 3 ) );
                }
            }
            bool isValid = false;
            foreach ( var n in check ) {
                if ( n.Value == 7 ) {
                    isValid = true;
                    break;
                }
            }
            if ( isValid == false ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Ikkitukan, han_ = 2 + ( paiSet.minkoGroup_.Count > 0 ? -1 : 0 ), bYakuman_ = false } );
            return true;
        }

        // 混老頭
        bool judge_Honroutou( PaiSet paiSet, BaState baState, YakuData yakuData ) {
            // 1,9,字牌のみで構成（刻子のみ）
            // 鳴かないと四暗刻になるので明刻が1つ以上必須
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            if ( paiSet.minkoGroup_.Count == 0 ) {
                return false;
            }

            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isSamePai() == true && p.isYaotyu() == true ) {
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isSamePai() == true && p.isYaotyu() == true ) {
                    num++;
                }
            }
            if ( num != 5 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Honroutou, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // 三色同順
        bool judge_Sansyokudouzyun(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 3色同じ並びの順子
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            int[] shuntsuIdx = new int[ 27 ];
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.getType() == PaiGroup.Type.Shuntsu && p.isNumber() == true ) {
                    shuntsuIdx[ p.PaiType ] = 1;
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.getType() == PaiGroup.Type.Shuntsu && p.isNumber() == true ) {
                    shuntsuIdx[ p.PaiType ] = 1;
                    num++;
                }
            }
            if ( num <= 2 ) {
                return false;
            }
            bool detect = false;
            for ( int i = 0; i < 9; ++i ) {
                if ( shuntsuIdx[ Pai.S1 + i ] == 1 && shuntsuIdx[ Pai.P1 + i ] == 1 && shuntsuIdx[ Pai.M1 + i ] == 1 ) {
                    detect = true;
                    break;
                }
            }
            if ( detect == false ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Sansyokudouzyun, han_ = 2 + (paiSet.minkoGroup_.Count > 0 ? -1 : 0), bYakuman_ = false } );
            return true;
        }

        // 三色同刻
        bool judge_Sanshokudouko(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 3色の同刻が揃っている
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            int[] koutsuIdx = new int[ 27 ];
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isKoutsuPai() && p.isNumber() == true ) {
                    koutsuIdx[ p.PaiType ] = 1;
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.getType() == PaiGroup.Type.Shuntsu && p.isNumber() == true ) {
                    koutsuIdx[ p.PaiType ] = 1;
                    num++;
                }
            }
            if ( num <= 2 ) {
                return false;
            }
            bool detect = false;
            for ( int i = 0; i < 9; ++i ) {
                if ( koutsuIdx[ Pai.S1 + i ] == 1 && koutsuIdx[ Pai.P1 + i ] == 1 && koutsuIdx[ Pai.M1 + i ] == 1 ) {
                    detect = true;
                    break;
                }
            }
            if ( detect == false ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Sanshokudouko, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // 三暗刻
        bool judge_Sananko(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 暗刻が3つ
            // 明刻はカウントしない
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isKoutsuPai() == true ) {
                    num++;
                }
            }
            if ( num != 3 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.Sananko, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // 対々和
        bool judge_ToiToi(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 刻子のみで構成
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            int num = 0;
            foreach ( var pg in paiSet.ankouGroup_ ) {
                if ( pg.isKoutsuPai() == true ) {
                    num++;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                if ( pg.isKoutsuPai() == true ) {
                    num++;
                }
            }
            if ( num != 4 ) {
                return false;
            }
            // 確定
            yakuData.yakuList_.Add( new YakuUnit { yaku_ = Yaku.ToiToi, han_ = 2, bYakuman_ = false } );
            return true;
        }

        // ダブ風
        bool judge_DabuKaze(PaiSet paiSet, BaState baState, YakuData yakuData) {
            // 場風と自風の刻子がある
            if ( containsYakuman( yakuData.yakuList_ ) == true ) {
                return false;
            }
            // 風が違う場合は対象外
            if ( baState.bakaze_ != baState.zikaze_ ) {
                return false;
            }
            var kazes = new Dictionary< int, Kaze>{
                { Pai.To, Kaze.Ton },
                { Pai.Na, Kaze.Nan },
                { Pai.Sh, Kaze.Sha },
                { Pai.Pe, Kaze.Pei },
            };
            var dabuKazes = new Dictionary<int, Yaku>{
                { Pai.To, Yaku.Dabuton },
                { Pai.Na, Yaku.Dabunan },
                { Pai.Sh, Yaku.Dabusha },
                { Pai.Pe, Yaku.Dabupei },
            };
            foreach ( var pg in paiSet.ankouGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isKoutsuPai() == true && p.isKaze() == true && kazes[ p.PaiType ] == baState.bakaze_ ) {
                    // 確定
                    yakuData.yakuList_.Add( new YakuUnit { yaku_ = dabuKazes[ p.PaiType ], han_ = 2, bYakuman_ = false } );
                    return true;
                }
            }
            foreach ( var pg in paiSet.minkoGroup_ ) {
                var p = pg.getPais()[ 0 ];
                if ( pg.isKoutsuPai() == true && p.isKaze() == true && kazes[ p.PaiType ] == baState.bakaze_ ) {
                    // 確定
                    yakuData.yakuList_.Add( new YakuUnit { yaku_ = dabuKazes[ p.PaiType ], han_ = 2, bYakuman_ = false } );
                    return true;
                }
            }
            return false;
        }

        // 指定の役が含まれている？
        bool containsYaku(List<YakuUnit> yakuList, params Yaku[] yakus ) {
            if ( yakus.Length == 0 )
                return false;
            foreach( var y in yakuList ) {
                foreach ( var targetYaku in yakus ) {
                    if ( y.yaku_ == targetYaku ) {
                        return true;
                    }
                }
            }
            return false;
        }

        // 役満を含んでいる？
        bool containsYakuman( List<YakuUnit> yakuList ) {
            foreach ( var y in yakuList ) {
                if ( y.bYakuman_ == true )
                    return true;
            }
            return false;
        }

    }
}