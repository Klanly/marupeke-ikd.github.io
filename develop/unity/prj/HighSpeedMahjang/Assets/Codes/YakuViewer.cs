using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YakuViewer : MonoBehaviour
{
    [SerializeField]
    MoveText moveTextPrefab_;

    [SerializeField]
    Transform YakuPos_;

    // ビュースタート
    public void start( TehaiSet tehaiSet, Mahjang.MahjangScoreCalculator.YakuData yakuData, int score, int han, System.Action finishCallback ) {
        if ( yakuData == null || yakuData.yakuList_.Count == 0 ) {
            // 役無し
            finishCallback();
            return;
        }
        bool isYakuman = false;
        if ( yakuData.yakuList_[ 0 ].bYakuman_ == true ) {
            isYakuman = true;
        }
        // 役名
        float yOffset = -4.0f;
        float delaySec = 0.5f;
        int yakuNum = yakuData.yakuList_.Count;
        for ( int i = 0; i < yakuNum; ++i ) {
            var yaku = PrefabUtil.createInstance( moveTextPrefab_, transform );
            yaku.transform.localPosition = YakuPos_.localPosition + new Vector3( 5.0f, yOffset * i, 0.0f );
            yaku.text( getYakuName( yakuData.yakuList_[ i ].yaku_ ) );
            yaku.setup( YakuPos_.localPosition + new Vector3( 0.0f, yOffset * i, 0.0f ), 0.75f, delaySec * i );
            moveTexts_.Add( yaku );
        }

        // ハン数
        var hanText = PrefabUtil.createInstance( moveTextPrefab_, transform );
        hanText.transform.localPosition = YakuPos_.localPosition + new Vector3( 5.0f, yOffset * yakuNum, 0.0f );
        if ( isYakuman == true ) {
            hanText.text( string.Format( "役満" ) );
        } else {
            hanText.text( string.Format( "{0} 飜", han ) );
        }
        hanText.setup( YakuPos_.localPosition + new Vector3( 0.0f, yOffset * yakuNum, 0.0f ), 0.75f, delaySec * yakuNum );
        moveTexts_.Add( hanText );

        // スコア
        var scoreText = PrefabUtil.createInstance( moveTextPrefab_, transform );
        scoreText.transform.localPosition = YakuPos_.localPosition + new Vector3( 5.0f, yOffset * ( yakuNum + 1.0f ), 0.0f );
        scoreText.text( string.Format( "{0}", score ) );
        scoreText.setup( YakuPos_.localPosition + new Vector3( 0.0f, yOffset * ( yakuNum + 1.0f ), 0.0f ), 0.75f, delaySec * ( yakuNum + 1.0f ) );
        moveTexts_.Add( scoreText );

        float waitSec = delaySec * ( yakuNum + 2.0f ) + 4.0f; 
        GlobalState.wait( waitSec, () => {
            foreach ( var t in moveTexts_ ) {
                Destroy( t.gameObject );
            }
            moveTexts_.Clear();
            finishCallback();
            return false;
        } );
    }

    // 役名を取得
    string getYakuName( Mahjang.MahjangScoreCalculator.Yaku yaku ) {
        var yakuNames = new Dictionary<Mahjang.MahjangScoreCalculator.Yaku, string > {
            { Mahjang.MahjangScoreCalculator.Yaku.None, "役無し" },
            { Mahjang.MahjangScoreCalculator.Yaku.Menzentsumo, "門前自摸" },
            { Mahjang.MahjangScoreCalculator.Yaku.Riti, "立直" },
            { Mahjang.MahjangScoreCalculator.Yaku.Ippatsu, "一発" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tanyao, "タンヤオ" },
            { Mahjang.MahjangScoreCalculator.Yaku.Pinhu, "平和" },
            { Mahjang.MahjangScoreCalculator.Yaku.Ipeko, "一盃口" },
            { Mahjang.MahjangScoreCalculator.Yaku.Haku, "白" },
            { Mahjang.MahjangScoreCalculator.Yaku.Hatsu, "發" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tyun, "中" },
            { Mahjang.MahjangScoreCalculator.Yaku.Ton, "東" },
            { Mahjang.MahjangScoreCalculator.Yaku.Nan, "南" },
            { Mahjang.MahjangScoreCalculator.Yaku.Sha, "西" },
            { Mahjang.MahjangScoreCalculator.Yaku.Pei, "北" },
            { Mahjang.MahjangScoreCalculator.Yaku.Chankan, "槍槓" },
            { Mahjang.MahjangScoreCalculator.Yaku.Rinshankaiho, "嶺上開花" },
            { Mahjang.MahjangScoreCalculator.Yaku.Haiteiraoyue, "海底撈月" },
            { Mahjang.MahjangScoreCalculator.Yaku.Houteiraoyui, "河底撈魚" },
            { Mahjang.MahjangScoreCalculator.Yaku.Daburi, "ダブリー" },
            { Mahjang.MahjangScoreCalculator.Yaku.Titoitsu, "七対子" },
            { Mahjang.MahjangScoreCalculator.Yaku.Dabuton, "ダブ東" },
            { Mahjang.MahjangScoreCalculator.Yaku.Dabunan, "ダブ南" },
            { Mahjang.MahjangScoreCalculator.Yaku.Dabusha, "ダブ西" },
            { Mahjang.MahjangScoreCalculator.Yaku.Dabupei, "ダブ北" },
            { Mahjang.MahjangScoreCalculator.Yaku.ToiToi, "対々和" },
            { Mahjang.MahjangScoreCalculator.Yaku.Sananko, "三暗刻" },
            { Mahjang.MahjangScoreCalculator.Yaku.Sanshokudouko, "三色同刻" },
            { Mahjang.MahjangScoreCalculator.Yaku.Sansyokudouzyun, "三色同順" },
            { Mahjang.MahjangScoreCalculator.Yaku.Honroutou, "混老頭" },
            { Mahjang.MahjangScoreCalculator.Yaku.Ikkitukan, "一気通貫" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tyanta, "チャンタ" },
            { Mahjang.MahjangScoreCalculator.Yaku.Shousangen, "小三元" },
            { Mahjang.MahjangScoreCalculator.Yaku.Sankantsu, "三槓子" },
            { Mahjang.MahjangScoreCalculator.Yaku.Honiso, "混一色" },
            { Mahjang.MahjangScoreCalculator.Yaku.Zyuntyan, "チャン" },
            { Mahjang.MahjangScoreCalculator.Yaku.Ryanpeiko, "二盃口" },
            { Mahjang.MahjangScoreCalculator.Yaku.NagashiMangan, "満貫" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tiniso, "清一色" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tenho, "天和" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tiho, "地和" },
            { Mahjang.MahjangScoreCalculator.Yaku.Renho, "人和" },
            { Mahjang.MahjangScoreCalculator.Yaku.Ryuiso, "緑一色" },
            { Mahjang.MahjangScoreCalculator.Yaku.Daisangen, "大三元" },
            { Mahjang.MahjangScoreCalculator.Yaku.Shosushi, "小四喜" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tuiso, "字一色" },
            { Mahjang.MahjangScoreCalculator.Yaku.Kokushimusou, "国士無双" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tyurenpoto, "九蓮宝燈" },
            { Mahjang.MahjangScoreCalculator.Yaku.Suanko, "四暗刻" },
            { Mahjang.MahjangScoreCalculator.Yaku.Tinrouto, "清老頭" },
            { Mahjang.MahjangScoreCalculator.Yaku.Sukantsu, "四槓子" },
            { Mahjang.MahjangScoreCalculator.Yaku.Suankotanki, "四暗刻単騎" },
            { Mahjang.MahjangScoreCalculator.Yaku.Daisushi, "大四喜" },
            { Mahjang.MahjangScoreCalculator.Yaku.ZyunseiTyurenpoto, "純正九蓮宝燈" },
            { Mahjang.MahjangScoreCalculator.Yaku.KokushiMusou13, "国士無双十三面待" },
        };
        return yakuNames[ yaku ];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<MoveText> moveTexts_ = new List<MoveText>();
}
