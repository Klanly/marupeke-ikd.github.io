using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapRenderer : MonoBehaviour
{
	[SerializeField]
	Camera cubeMapCamera_ = null;

	static public CubeMapRenderer getInstance()
	{
		return instance_g;
	}

	public void toClear()
	{
		bClear_ = true;
		Destroy( cubeMapCamera_.gameObject );
	}

	private void Awake()
	{
		if ( renderTexture_ == null ) {
			renderTexture_ = new RenderTexture( 1024, 1024, 24 );
			renderTexture_.dimension = UnityEngine.Rendering.TextureDimension.Cube;
		}
		instance_g = this;
	}

	public RenderTexture getRenderTexture()
	{
		return renderTexture_;
	}

	void Start()
    {
        
    }

    void Update()
    {
		if ( bClear_ == false ) {
			var res = cubeMapCamera_.RenderToCubemap( renderTexture_, 63 );
		}
	}

	[SerializeField]
	RenderTexture renderTexture_;

	static CubeMapRenderer instance_g;
	bool bClear_ = false;
}
