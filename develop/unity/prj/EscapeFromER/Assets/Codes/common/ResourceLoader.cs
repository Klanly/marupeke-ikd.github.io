using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// リソースローダー
//
//  Unityのリソースは以下の3つの場所にあります：
//  ・Resourcesフォルダ
//  ・StreamingAssetsフォルダ下
//  ・独自ダウンロードサーバ上（AssetBundleサーバ）
//
//  このクラスはそれぞれの場所にあるファイルを統一したインターフェイスで
//  読み込みます。
//  ex) 何れかのフォルダ下にhoge/foo/sample.bytesを置いている場合
//      Resources.getInstance().loadAsync< TextAsset >( "hoge/foo/sample", ( res, text ) => {
//          // textはテンプレート引数の型にキャスト済み
//          myText = text.text;
//      });
//
//  Asyncを使った場合非同期で読み込まれるためリクエスト元で読み込み待ちが
//  必要です。
//

public class ResourceLoader {
    private static ResourceLoader instance_ = new ResourceLoader();
    private ResourceLoader() {
        GameObject updater = new GameObject( "ResourceLoaderUpdater" );
        updater.AddComponent<ResurceLoaderUpdater>();
    }
    public static ResourceLoader getInstance()
    {
        return instance_;
    }

    // 非同期ロード
    //  resourceFilePath: リソースのファイルパス（拡張子は除く）
    //  callback        : 読み込まれたアセットを返す。失敗の場合は第1引数にfalseが返る。
    public ResultBase loadAsync<T>( string resourceFilePath, System.Action< bool, T > callback ) where T: UnityEngine.Object
    {
        Result<T> res = new Result<T>();
        res.loadAsync( resourceFilePath, callback );
        results_.Add( res );
        return res;
    }

    // 同期ロード
    //  Resources以下のリソースを読み込みます。
    //  resourceFilePath: リソースのファイルパス（拡張子は除く）
    public T loadSync<T>(string resourceFilePath) where T : UnityEngine.Object
    {
        return Resources.Load<T>( resourceFilePath );
    }

    // 読み込みチェック（毎フレーム更新）
    public void update()
    {
        if ( results_.Count > 0 ) {
            List<ResultBase> results = new List<ResultBase>();
            foreach ( var r in results_ ) {
                if ( r.update() == false ) {
                    // 読み込み継続
                    results.Add( r );
                }
            }
            results_ = results;
        }
    }

    public class ResultBase
    {
        // 読み込みチェック
        virtual public bool update() { return true; }
    }

    // ロード中の状態
    public class Result<T> : ResultBase where T : UnityEngine.Object
    {
        // 非同期ロード
        public void loadAsync(string resourceFilePath, System.Action<bool, T> callback) {
            // Resourceフォルダ下読み込み
            resourceRequest_ = Resources.LoadAsync<T>( resourceFilePath );
            callback_ = callback;

            System.Func<bool> assetBundleUpdate = () => {
                callback_( false, null );
                state_ = null;
                return true;
            };
            System.Func< bool > resourcesUpdate = () => {
                if ( resourceRequest_.isDone == true ) {
                    if ( resourceRequest_.asset != null ) {
                        callback_( true, ( T )resourceRequest_.asset );
                        return true;
                    }
                    state_ = assetBundleUpdate;
                }
                return false;
            };
            state_ = resourcesUpdate;
        }

        // 読み込みチェック
        override public bool update()
        {
            if ( state_ == null )
                return true;
            return state_();
        }

        System.Func< bool > state_;
        System.Action<bool, T> callback_;
        ResourceRequest resourceRequest_;   // Resources.loadの経過・結果
    }

    List<ResultBase> results_ = new List<ResultBase>();
}
