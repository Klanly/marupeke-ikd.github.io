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
			Shader shader = Shader.Find( "Hidden/Internal-Colored" );
			lineMaterial_ = new Material( shader );
			lineMaterial_.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial_.SetInt( "_SrcBlend", ( int )UnityEngine.Rendering.BlendMode.SrcAlpha );
			lineMaterial_.SetInt( "_DstBlend", ( int )UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha );
			lineMaterial_.SetInt( "_Cull", ( int )UnityEngine.Rendering.CullMode.Off );
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
        LinkedListNode<GLLine> node = lines_.First;
        while ( node != null ) {
            var obj = node.Value;
            if ( obj.draw() == false ) {
                var removeNode = node;
                node = node.Next;
                lines_.Remove( removeNode );
            } else {
                node = node.Next;
            }
        }
        if ( fixLines_ != null ) {
            foreach ( var n in fixLines_ ) {
                n.draw();
            }
        }
        GL.End();
		GL.PopMatrix();
	}

	public void addLine( GLLine line ) {
		lines_.AddLast( line );
	}

	LinkedList<GLLine> lines_ = new LinkedList<GLLine>(); 
}


// グローバルなGLLines
public class StaticGLLines {
    protected StaticGLLines() {
        var obj = new GameObject( "StaticGLLines" );
        glLines_ = obj.AddComponent<GLLines>();
    }
    public static StaticGLLines getInstance() {
        return instance_g;
    }
    public void addLine( GLLine line ) {
        glLines_.addLine( line );
    }
    static StaticGLLines instance_g = new StaticGLLines();
    GLLines glLines_;
}