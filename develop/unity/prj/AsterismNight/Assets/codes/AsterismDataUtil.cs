using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 星座データユーティリティ
public class AsterismDataUtil {

    public class Star
    {
        public int hipId_;  // ヒッパルコスId
        public string name_;    // 名前（英語）
        public Vector2 pos_;       // 位置（極座標：lat, long)
        public float magnitude_;   // 見かけ等級
    }

    public class Line
    {
        public Vector2 start_;     // 始点位置（極座標： lat. long）
        public Vector2 end_;       // 終点位置（極座標： lat. long）
        public int startHipId_;    // 始点ヒッパルコスId
        public int endHipId_;      // 終点ヒッパルコスId
    }

    public class Asterism
    {
        public int id_;            // 星座Id
        public string shortName_;  // 略名
        public string name_;       // 星座名（英語）
        public string jpName_;     // 星座名（日本）
        public int starNum_;       // 恒星数
        public List<Star> stars_ = new List<Star>();   // 恒星
        public List<Line> lines_ = new List<Line>();   // 恒星間ライン
    }

    // 星座Idからデータ取得
    static public Asterism getData( int astId )
    {
        if ( astId < 1 || astId > 89 )
            return null;

        var data = new Asterism();
        int astIndex = astId - 1;

        var ast = Table_asterism_ast.getInstance().getData( astIndex );
        data.id_ = astId;
        data.shortName_ = ast.shortName_;
        data.name_ = ast.name_;
        data.jpName_ = ast.jpName_;
        var posTable = Table_asterism_star_pos.getInstance();
        var lineHipTable = Table_asterism_line_hip.getInstance();
        var starHipTable = Table_asterism_star_hip.getInstance();

        // 恒星
        var starHipIndices = lineHipTable.getStarHipIndicesFromShortName( data.shortName_ );
        data.starNum_ = starHipIndices.Count;
        foreach( var i in starHipIndices ) {
            // ヒッパルコスIdに対応する恒星データを取得
            Star star = new Star();
            var posData = posTable.getDataFromHipId( i );
            star.hipId_ = i;
            star.magnitude_ = posData.magnitude_;
            star.name_ = starHipTable.getName( star.hipId_ );
            star.pos_ = new Vector2( posData.lat_, posData.long_ );
            data.stars_.Add( star );
        }

        // 恒星間ライン
        var linePair = lineHipTable.getLinesFromShortName( data.shortName_ );
        foreach( var pair in linePair ) {
            var line = new Line();
            var start = posTable.getDataFromHipId( pair.startHipId_ );
            var end = posTable.getDataFromHipId( pair.endHipId_ );
            line.start_ = new Vector2( start.lat_, start.long_ );
            line.end_ = new Vector2( end.lat_, end.long_ );
            line.startHipId_ = pair.startHipId_;
            line.endHipId_ = pair.endHipId_;
            data.lines_.Add( line );
        }

        return data;
    }
}
