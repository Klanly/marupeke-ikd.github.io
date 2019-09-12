using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 門前セット
public class MenzenSet {

    public Mahjang.PaiGroup PaiGroup { get { return paiGroup_; } }
    public List<PaiObject> Pais { get { return pais_; } }

    // 成立している？
    public bool isValid() {
        return !( paiGroup_.getType() == Mahjang.PaiGroup.Type.None ); 
    }

    // 牌をセット
    public int set( List<PaiObject> list, int start ) {
        if ( list.Count - start <= 2 ) {
            return 1;
        }
        // 槓子、暗子をチェック
        var ps = new List<PaiObject>();
        var p0 = list[ start ];
        var pt0 = p0.getPai().PaiType;
        ps.Add( p0 );
        for ( int i = start + 1; i < list.Count; ++i ) {
            var pt = list[ i ].getPai().PaiType;
            if ( pt != pt0 ) {
                break;
            }
            if ( pt == pt0 ) {
                ps.Add( list[ i ] );
            }
        }
        var koutsuList = new List<Mahjang.Pai>();
        if ( ps.Count >= 4 ) {
            // 槓子確定
            for ( int i = 0; i < 4; ++i ) {
                pais_.Add( ps[ i ] );
                koutsuList.Add( ps[ i ].getPai() );
            }
            paiGroup_.set( koutsuList, false );
            return ps.Count;
        } else if ( ps.Count == 3 ) {
            // 暗子確定
            for ( int i = 0; i < 3; ++i ) {
                pais_.Add( ps[ i ] );
                koutsuList.Add( ps[ i ].getPai() );
            }
            paiGroup_.set( koutsuList, false );
            return ps.Count;
        } else if ( ps.Count == 2 ) {
            return ps.Count;    // 成立無し
        }

        // 順子チェック
        var shuntsuList = new List<Mahjang.Pai>();
        shuntsuList.Add( p0.getPai() );
        for ( int i = start + 1; i < start + 3; ++i ) {
            ps.Add( list[ i ] );
            shuntsuList.Add( list[ i ].getPai() );
        }
        paiGroup_.set( shuntsuList, false );
        if ( paiGroup_.getType() == Mahjang.PaiGroup.Type.Shuntsu ) {
            pais_ = ps;
        }

        // 対子チェック
        if ( ps[ 0 ].getPai().PaiType == ps[ 1 ].getPai().PaiType ) {
            pais_.Add( ps[ 0 ] );
            pais_.Add( ps[ 1 ] );
            paiGroup_.set( new Mahjang.Pai[] { ps[ 0 ].getPai(), ps[ 1 ].getPai() }, false );
            // 右端は次に回せるので
            return 2;
        } else if ( ps[ 0 ].getPai().PaiType == ps[ 2 ].getPai().PaiType ) {
            pais_.Add( ps[ 0 ] );
            pais_.Add( ps[ 2 ] );
            paiGroup_.set( new Mahjang.Pai[] { ps[ 0 ].getPai(), ps[ 2 ].getPai() }, false );
            return 3;
        } else if ( ps[ 1 ].getPai().PaiType == ps[ 2 ].getPai().PaiType ) {
            pais_.Add( ps[ 1 ] );
            pais_.Add( ps[ 2 ] );
            paiGroup_.set( new Mahjang.Pai[] { ps[ 1 ].getPai(), ps[ 2 ].getPai() }, false );
            return 3;
        }

        // 成立無し
        return 1;
    }

    Mahjang.PaiGroup paiGroup_ = new Mahjang.PaiGroup();
    List<PaiObject> pais_ = new List<PaiObject>();
}
