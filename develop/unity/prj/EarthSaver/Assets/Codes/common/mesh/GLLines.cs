using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GLライン描画

public class GLLines : MonoBehaviour {

	[SerializeField]
	Material material_;

	[SerializeField]
	GLLine[] fixLines_;


	static Material lineMaterial_;
	static void createLineMaterial() {
		if ( !lineMaterial_ ) {
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			Shader shader = Shader.Find( "Hidden/Internal-Colored" );
			lineMaterial_ = new Material( shader );
			lineMaterial_.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial_.SetInt( "_SrcBlend", ( int )UnityEngine.Rendering.BlendMode.SrcAlpha );
			lineMaterial_.SetInt( "_DstBlend", ( int )UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha );
			// Turn backface culling off
			lineMaterial_.SetInt( "_Cull", ( int )UnityEngine.Rendering.CullMode.Off );
			// Turn off depth writes
			lineMaterial_.SetInt( "_ZWrite", 0 );
		}
	}

	void OnRenderObject() {
		if ( material_ == null ) {
			createLineMaterial();
			material_ = lineMaterial_;
			return;
		}

		// 保持しているラインを描画
		material_.SetPass( 0 );
		GL.PushMatrix();

		GL.Begin( GL.LINES );
		for ( var n = lines_.First; n != null; n = n.Next ) {
			n.Value.draw();
		}
		foreach ( var n in fixLines_ ) {
			n.draw();
		}
		GL.End();
		GL.PopMatrix();
	}

	public void addLine( GLLine line ) {
		lines_.AddLast( line );
	}

	LinkedList<GLLine> lines_ = new LinkedList<GLLine>(); 
}
