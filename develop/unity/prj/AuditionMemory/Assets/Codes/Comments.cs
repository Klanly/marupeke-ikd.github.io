using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 選択後コメント集
public class Comments {

    public enum ComboState {
        Success_First_First,
        Success_First_Already,
        Success_Already_First,
        Success_Already_Already,

        Failed_First_First_PairNone,
        Failed_First_First_PairAlready,
        Failed_First_Already_PairNone,
        Failed_First_Already_PairAlready,
        Failed_Already_First_PairNone,
        Failed_Already_First_PairAlready,
        Failed_Already_Already_PairNone,
        Failed_Already_Already_PairAlready,
    }

    public static string getComment( ComboState state ) {
        if ( dict_.Count == 0 ) {
            var SFF = new string[] { "偶然の一致！結果オーライ！" };
            var SFA = new string[] { "一発発見！いい耳してる！" };
            var SAF = new string[] { "まぐれ当たりじゃんww" };
            var SAA = new string[] { "何とか見つけたか", "よ～し見っけた" };

            var FFFN = new string[] { "最初だからノーカウント" };
            var FFFA = new string[] { "うわー、もう出てたのに！", "それあったってば！" };
            var FFAN = new string[] { "後の音前も聞いてるんだけどー" };
            var FFAA = new string[] { "正解あったのに～覚えてない？" };
            var FAFN = new string[] { "最初の音のペアまだ聞いてないよ～" };
            var FAFA = new string[] { "正解あるのに同じミス！痛い！" };
            var FAAN = new string[] { "闇雲に探してない？" };
            var FAAA = new string[] { "完全に迷子状態..." };

            dict_[ ComboState.Success_First_First ] = SFF;
            dict_[ ComboState.Success_First_Already ] = SFA;
            dict_[ ComboState.Success_Already_First ] = SAF;
            dict_[ ComboState.Success_Already_Already ] = SAA;

            dict_[ ComboState.Failed_First_First_PairNone ] = FFFN;
            dict_[ ComboState.Failed_First_First_PairAlready ] = FFFA;
            dict_[ ComboState.Failed_First_Already_PairNone ] = FFAN;
            dict_[ ComboState.Failed_First_Already_PairAlready ] = FFAA;
            dict_[ ComboState.Failed_Already_First_PairNone ] = FAFN;
            dict_[ ComboState.Failed_Already_First_PairAlready ] = FAFA;
            dict_[ ComboState.Failed_Already_Already_PairNone ] = FAAN;
            dict_[ ComboState.Failed_Already_Already_PairAlready ] = FAAA;
        }

        var comments = dict_[ state ];
        return comments[ Random.Range( 0, comments.Length ) ];
    }

    static Dictionary<ComboState, string[]> dict_ = new Dictionary<ComboState, string[]>();
}
