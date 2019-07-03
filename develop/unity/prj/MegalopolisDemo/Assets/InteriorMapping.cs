using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorMapping : MonoBehaviour
{
	[SerializeField]
	Renderer renderer_;

	// [SerializeField]
	// float billHeight_ = 16.0f;  // ビルの高さ

    [SerializeField]
    float height1Floor_ = 3.0f; // 1フロアの高さ（3mが基準らしい）
    public float Height1Floor { set { height1Floor_ = value; bUpdate_ = true; } get { return height1Floor_; } }

    [ SerializeField]
    float roomWidth_ = 5.0f;    // 部屋の横幅
    public float RoomWidth { set { roomWidth_ = value; bUpdate_ = true; } get { return roomWidth_; } }

    [SerializeField]
    float roomDepth_ = 5.0f;    // 部屋の奥行幅
    public float RoomDepth { set { roomDepth_ = value; bUpdate_ = true; } get { return roomDepth_; } }

    [SerializeField]
	Vector4 roomSep_ = new Vector4( 3, 8, 4, 0 );	// ビルの部屋分割数
    public Vector4 RoomSep { set { roomSep_ = value; bUpdate_ = true; } get { return roomSep_; } }

    [SerializeField]
    Vector4 outWallTickness_ = new Vector4( 0.85f, 0.95f, 0.85f, 0.0f );    // 窓枠外壁の厚み率
    public Vector4 OutWallTickness { set { outWallTickness_ = value; bUpdate_ = true; } get { return outWallTickness_; } }

    [SerializeField]
    Color outWallColor_ = Color.gray;   // 窓枠外壁の色
    public Color OutWallColor { set { outWallColor_ = value; bUpdate_ = true; } get { return outWallColor_; } }

    [SerializeField]
    float windowTransRate_ = 0.70f;
    public float WindowTransRate { set { windowTransRate_ = value; bUpdate_ = true; } get { return windowTransRate_; } }

    private void OnValidate() {
        // ビルの高さとスケールを調整
        float billHeight = height1Floor_ * roomSep_.y;
        var pos = transform.position;
		pos.y = billHeight * 0.5f;
		transform.position = pos;
		var scale = transform.localScale;
		scale.y = billHeight;
		transform.localScale = scale;

		// 部屋数を調整
//		var mat = renderer_.material;
//		mat.SetVector( "_RoomSep", roomSep_ );
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( bUpdate_ == false )
            return;

		// 部屋数を調整
		var mat = renderer_.material;
		mat.SetVector( "_RoomSep", roomSep_ );

        // 窓枠外壁厚み率
        mat.SetVector( "_OutWallTickness", outWallTickness_ );

        // 窓枠外壁の色
        mat.SetColor( "_OutWallColor", outWallColor_ );

        // 
        mat.SetFloat( "_WindowTransRate", windowTransRate_ );

        // ビルの高さとスケールを調整
        float billHeight = height1Floor_ * roomSep_.y;
        float billDepth = roomDepth_ * roomSep_.z;
        float billWidth = roomWidth_ * roomSep_.x;
        var pos = transform.position;
        pos.y = billHeight * 0.5f;
        transform.position = pos;
        var scale = transform.localScale;
        scale.x = billWidth;
        scale.y = billHeight;
        scale.z = billDepth;
        transform.localScale = scale;
    }
    bool bUpdate_ = true;
}
