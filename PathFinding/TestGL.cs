using UnityEngine;
using System.Collections;

public class TestGL : MonoBehaviour {

    // When added to an object, draws colored rays from the
    // transform position.
    public int lineCount = 100;
    public float radius = 3.0f;

    static Material lineMaterial;
    Grid gridClass;

    void Start()
    {
        gridClass = GetComponent<Grid>();
    }

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);
        foreach(Node elm in gridClass.grid)
        {
            DrawQuad(elm.worldPosition, gridClass.cubeSize);
        }
        /*GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
        
        for (int i = 0; i < lineCount; ++i)
        {
            float a = i / (float)lineCount;
            float angle = a * Mathf.PI * 2;
            // Vertex colors change from red to green
            GL.Color(new Color(a, 1 - a, 0, 0.8F));
            // One vertex at transform position
            GL.Vertex3(0, 0, 0);
            // Another vertex at edge of circle
            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        GL.End();
        GL.PopMatrix();*/
    }

    public void DrawBox(Vector3 worldPosition, float boxSize)
    {
        float halfBoxSize = boxSize / 2;
        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);


        // Vertex colors change from red to green
        GL.Color(Color.green);
        // One vertex at transform position
        Vector3 Corner = worldPosition;
        //First Corner (0,0,0)
        Corner -= Vector3.one * halfBoxSize;
        GL.Vertex3(Corner.x,Corner.y, Corner.z);
        GL.Vertex3(Corner.x+boxSize, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y + boxSize, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z + boxSize);
        //Second Corner (1,1,0)
        Corner += new Vector3(1,1,0)*boxSize;
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x - boxSize, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y - boxSize, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z + boxSize);
        //Threed Corner (1,0,1)
        Corner += new Vector3(0,-1,1)*boxSize;
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x - boxSize, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y + boxSize, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z - boxSize);
        //Forth Corner (0,1,1)
        Corner += new Vector3(-1, 1, 0) * boxSize;
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x + boxSize, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y - boxSize, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z);
        GL.Vertex3(Corner.x, Corner.y, Corner.z - boxSize);

        GL.End();
        GL.PopMatrix();
    }

    public void DrawQuad(Vector3 worldPosition, float boxSize)
    {
        float halfBoxSize = boxSize / 2;
        
        GL.PushMatrix();

        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(Color.red);
        GL.Vertex3(worldPosition.x+ halfBoxSize, worldPosition.y, worldPosition.z+ halfBoxSize);
        GL.Vertex3(worldPosition.x - halfBoxSize, worldPosition.y, worldPosition.z + halfBoxSize);
        GL.Vertex3(worldPosition.x - halfBoxSize, worldPosition.y, worldPosition.z - halfBoxSize);
        GL.Vertex3(worldPosition.x + halfBoxSize, worldPosition.y, worldPosition.z - halfBoxSize);

        GL.End();
        GL.PopMatrix();
    }
}
