using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstLine : MonoBehaviour {

    [SerializeField]
    Transform rot_;

    [SerializeField]
    GameObject line_;

    [SerializeField]
    MeshRenderer renderer_;

    public void setLine( Vector3 start, Vector3 end, float diameterScale )
    {
        // Y軸はstart-endライン
        Vector3 y = ( end - start ).normalized;

        // 仮Zはstart-end中点と原点
        Vector3 pos = ( start + end ) * 0.5f;
        Vector3 z = -pos.normalized;

        // X軸
        Vector3 x = Vector3.Cross( y, z ).normalized;

        // Z軸
        z = Vector3.Cross( x, y ).normalized;

        // 位置と回転とスケールを適用
        float scale = ( end - start ).magnitude * 0.5f;
        var q = Quaternion.LookRotation( z, y );
        transform.localPosition = pos;
        rot_.transform.localRotation = q;
        line_.transform.localScale = new Vector3( diameterScale, scale, diameterScale );
    }

    public void setColorScale(float scale)
    {
        colorScale_ = scale;
        setColor( new Vector3( baseColor_.r, baseColor_.g, baseColor_.b ), alpha_ );
    }

    public void setColor(Vector3 color, float alpha)
    {
        baseColor_ = new Color( color.x, color.y, color.z, alpha );
        alpha_ = alpha;
        var mat = renderer_.material;
        var c = baseColor_ * colorScale_;
        c.a = alpha;
        mat.color = c;
        renderer_.material = mat;
    }

    public void setAlpha(float alpha)
    {
        alpha_ = alpha;
        var mat = renderer_.material;
        var color = mat.color;
        color.a = alpha;
        mat.color = color;
        renderer_.material = mat;
    }

    public void backupRotation()
    {
        backupRot_ = transform.localRotation;
    }

    public void backupQuestionRotation()
    {
        questionRot_ = transform.localRotation;
    }

    public Quaternion getBackupRotation()
    {
        return backupRot_;
    }

    public Quaternion getQuestionRotation()
    {
        return questionRot_;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Color baseColor_;
    float alpha_ = 1.0f;
    float colorScale_ = 1.0f;
    Quaternion backupRot_ = Quaternion.identity;
    Quaternion questionRot_ = Quaternion.identity;
}
