using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// メッシュに毛を生やすコンポーネント
//
//  対象のメッシュの表側の面にシェル法を用いて毛を生やします。
//  メッシュは法線方向に積層されます。
//  furTexテクスチャ上で白色が濃い部分程長い毛になります。真っ黒部分は毛が生えません。
//  生える場所はメッシュのUVに従います。UVが無いモデルの場合はメッシュのローカル座標の極座標値を
//  UV値として扱います。
//  全体的な毛の密度はdensityの値で調整します。furTexのスケールが変わるため、密度が濃くなる程毛が
//  細くなるので注意です。

public class FurModel : MonoBehaviour {

    [SerializeField]
    MeshFilter meshFilter_;

    [SerializeField]
    MeshRenderer meshRenderer_;

    [SerializeField]
    Material furMaterial_;

    [SerializeField]
    int shellNum_ = 15;

    [SerializeField]
    float maxFurLen_ = 1.0f;

    [SerializeField]
    float furScale_ = 1.0f;

    [SerializeField]
    Vector3 blowDir_ = Vector3.zero;

    [SerializeField]
    float blowPower_ = 1.0f;

    [SerializeField]
    float blowPowerEffect_ = 1.0f;

    [SerializeField]
    float dropRegistConst_ = 0.0f;

    [SerializeField]
    float gravity_ = 0.3f;

    [SerializeField]
    float furDepthColor = 0.0f;

    [SerializeField]
    float furTopColor = 1.0f;

    [SerializeField]
    float randomBlowPower_ = 0.0f;

    [SerializeField]
    Transform mainCameraTrans_;

    [SerializeField]
    ColliderCallback colliderCallback_;

    public ColliderCallback ColliderCallback { get { return colliderCallback_; } }
    public System.Action gameOverFinishCallback_ = null;

    public void addBlow( Vector3 blow )
    {
        // 瞬間的な風で物理的力を加える
        if ( bGameOver_ == false )
            impactList_.Add( blow );
    }

    public bool isGameOver()
    {
        return bGameOver_;
    }

    // ゲームオーバーへ遷移
    public void toGameOver()
    {
        bGameOver_ = true;
    }

    // Use this for initialization
    void Start () {

        string baseName = gameObject.name;
        GameObject parent = meshFilter_.gameObject;

        // shellNumの分だけメッシュを生成
        var mesh = meshFilter_.mesh;
        var vertices = mesh.vertices;
        var indices = mesh.triangles;
        var normals = mesh.normals;
        var uvs = mesh.uv;
        float furShellUnitLen = maxFurLen_ / shellNum_;
        for ( int i = 0; i < shellNum_; ++i ) {
            Mesh shellMesh = new Mesh();
            shellMesh.vertices = vertices;
            shellMesh.triangles = indices;
            shellMesh.normals = normals;
            shellMesh.uv = uvs;

            GameObject obj = new GameObject( baseName + "_shell" + i );
            MeshFilter filter = obj.AddComponent<MeshFilter>();
            MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
            filter.mesh = shellMesh;
            Material m = Instantiate<Material>( furMaterial_ );
            if ( m.HasProperty( "_FurStep" ) == true ) {
                m.SetFloat( "_FurStep", (float)i );
            }
            if ( m.HasProperty( "_ShellNum" ) == true ) {
                m.SetFloat( "_ShellNum", ( float )shellNum_ );
            }
            if ( m.HasProperty( "_FurLength" ) == true ) {
                m.SetFloat( "_FurLength", furShellUnitLen * ( i + 1 ) );
            }
            if ( m.HasProperty( "_FurScale" ) == true ) {
                m.SetFloat( "_FurScale", furScale_ );
            }
            if ( m.HasProperty( "_FurDrawThreshold" ) == true ) {
                m.SetFloat( "_FurDrawThreshold", (float)i / shellNum_ );
            }
            if ( m.HasProperty( "_FurColorDarkness" ) == true ) {
                m.SetFloat( "_FurColorDarkness", furDepthColor + ( furTopColor - furDepthColor ) * ( float )i / shellNum_ );
            }
            renderer.material = m;
            furMats_.Add( m );

            obj.transform.parent = parent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
        }

        prePos_ = transform.position;

        for ( int i = 0; i < aveNum_; ++i )
            relativeBlows_.Add( Vector3.zero );
        for ( int i = 0; i < randomBlowNum_; ++i )
            randomBlows_.Add( Vector3.zero );
    }

    class GameOverWait : State
    {
        public GameOverWait( FurModel parent )
        {
            parent_ = parent;
        }
        // 内部状態
        override protected State innerUpdate()
        {
            t -= Time.deltaTime;
            if ( t <= 0.0f )
                return new GameOverScaleUp( parent_ );
            return this;
        }
        FurModel parent_;
        float t = 0.50f;
    }
    class GameOverScaleUp : State
    {
        public GameOverScaleUp( FurModel parent )
        {
            parent_ = parent;
        }
        // 内部状態
        override protected State innerUpdate()
        {
            t += Time.deltaTime;
            float d = t / tm;
            scale_ = 1.0f + 1.0f * d + 0.1f * Mathf.Cos( d * 12.0f * Mathf.PI * 2.0f );
            if ( t >= tm ) {
                parent_.transform.localScale = Vector3.one * scale_;
                return new GameOverScaleDown( parent_, scale_ );
            }
            parent_.transform.localScale = Vector3.one * scale_;
            return this;
        }
        FurModel parent_;
        float scale_ = 1.0f;
        float t = 0.0f;
        float tm = 1.3f;
    }
    class GameOverScaleDown : State
    {
        public GameOverScaleDown( FurModel parent, float scale )
        {
            parent_ = parent;
            scale_ = scale;
        }
        // 内部状態
        override protected State innerUpdate()
        {
            scale_ -= 0.15f;
            if ( scale_ <= 1.0f ) {
                parent_.transform.localScale = Vector3.one * scale_;
                return new GameOverFall( parent_ );
            }
            parent_.transform.localScale = Vector3.one * scale_;
            return this;
        }
        FurModel parent_;
        float scale_ = 1.0f;
    }
    class GameOverFall : State
    {
        public GameOverFall( FurModel parent )
        {
            parent_ = parent;
        }

        // 内部初期化
        override protected void innerInit()
        {
            if ( parent_.gameOverFinishCallback_ != null )
                parent_.gameOverFinishCallback_();
        }

        // 内部状態
        override protected State innerUpdate()
        {
            return this;
        }
        FurModel parent_;
    }

    bool bGameOverInit_ = false;
    State gameOverState_ = null;
    void gameOverState()
    {
        if ( bGameOverInit_ == false ) {
            bGameOverInit_ = true;
            gameOverState_ = new GameOverWait( this );
        }
        gameOverState_ = gameOverState_.update();
    }

    // Update is called once per frame
    void Update () {
        if ( bGameOver_ == true ) {
            bCameraChasing_ = false;
            gameOverState();
        }

        // 重力と空気抵抗から加速度を算出
        //  ma = mg - kv
        //   a =  g - kv / m
        // 外部衝撃力で加速度を更新
        Vector3 acc = Vector3.down * gravity_ - dropRegistConst_ * preVelo_;
        foreach ( var i in impactList_ )
            acc += i;
        impactList_.Clear();

        // 速度更新
        preVelo_ += acc;

        // 位置更新
        Vector3 p = transform.localPosition;
        p += preVelo_;
        transform.localPosition = p;

        // 相対的な風の強さを算出
        Vector3 relativeBlow = calcRelativeBlow();
        foreach ( var m in furMats_ ) {
            m.SetVector( "_Blow", relativeBlow );
            m.SetFloat( "_BlowPower", relativeBlow.magnitude * blowPowerEffect_ );
        }

        if ( bCameraChasing_ == true ) {
            // カメラの位置を更新
            Vector3 cameraPos = mainCameraTrans_.localPosition;
            cameraPos.x = prePos_.x;
            cameraPos.y = prePos_.y;
            mainCameraTrans_.localPosition = cameraPos;
        }
    }

    Vector3 calcRelativeBlow()
    {
        Vector3 aveRandowBlow = Vector3.zero;
        randomBlows_[ curRandomBlowIdx_ ] = ( new Vector3( Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f ) ).normalized * randomBlowPower_;
        for ( int i = 0; i < randomBlows_.Count; ++i )
            aveRandowBlow += randomBlows_[ i ];

        curRandomBlowIdx_++;
        if ( curRandomBlowIdx_ >= randomBlowNum_ )
            curRandomBlowIdx_ = 0;

        Vector3 curPos = transform.position;
        Vector3 v = curPos - prePos_;
        relativeBlows_[ curIdx ] = -v;
        Vector3 aveRelativeBlow = Vector3.zero;
        for ( int i = 0; i < relativeBlows_.Count; ++i )
            aveRelativeBlow += relativeBlows_[ i ];

        curIdx++;
        if ( curIdx >= aveNum_ )
            curIdx = 0;

        prePos_ = curPos;

        return aveRandowBlow + blowDir_ + aveRelativeBlow * relativeBlowWeight_;

    }

    List<Material> furMats_ = new List<Material>();
    List<Vector3> relativeBlows_ = new List<Vector3>();
    List<Vector3> randomBlows_ = new List<Vector3>();
    int curIdx = 0;
    int aveNum_ = 20;
    int curRandomBlowIdx_ = 0;
    int randomBlowNum_ = 10;
    float relativeBlowWeight_ = 1.0f / 20.0f;
    Vector3 prePos_;
    Vector3 preVelo_ = Vector3.zero;
    List<Vector3> impactList_ = new List<Vector3>();
    bool bGameOver_ = false;
    bool bCameraChasing_ = true;
}
