using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapRenderer : MonoBehaviour
{
	[SerializeField]
	Camera cubeMapCamera_;

	static public CubeMapRenderer getInstance()
	{
		return instance_g;
	}

	private void Awake()
	{
		var desc = new RenderTextureDescriptor();
		desc.autoGenerateMips = false;
		desc.bindMS = false;
		desc.colorFormat = RenderTextureFormat.ARGB32;
		desc.depthBufferBits = 32;
		desc.dimension = UnityEngine.Rendering.TextureDimension.Cube;
		desc.enableRandomWrite = false;
		desc.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm;
		desc.height = 1024;
		desc.mipCount = 1;
		desc.msaaSamples = 1;
		desc.useMipMap = false;
		desc.width = 1024;
		desc.volumeDepth = 1;
		renderTexture_ = new RenderTexture( desc );
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
		var res = cubeMapCamera_.RenderToCubemap( renderTexture_, 63 );
    }

	[SerializeField]
	RenderTexture renderTexture_;
	static CubeMapRenderer instance_g;
}
