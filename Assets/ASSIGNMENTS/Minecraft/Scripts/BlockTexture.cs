using UnityEngine;

public class BlockTexture : MonoBehaviour {

    MeshFilter meshFilter;
    Mesh mesh;
    
    void Start() {
        
        meshFilter = GetComponent<MeshFilter>();

        mesh = meshFilter.sharedMesh;

        Vector2[] uv = mesh.uv;

        // front - side
        uv[0] = new Vector2(0.333f, 0.25f);
        uv[1] = new Vector2(0.666f, 0.25f);
        uv[2] = new Vector2(0.666f, 0.5f);
        uv[3] = new Vector2(0.333f, 0.5f);

        // back - side
        uv[4] = new Vector2(0.333f, 0.25f);
        uv[5] = new Vector2(0.666f, 0.25f);
        uv[6] = new Vector2(0.666f, 0.5f);
        uv[7] = new Vector2(0.333f, 0.5f);

        // left - side
        uv[8]  = new Vector2(0.333f, 0.25f);
        uv[9]  = new Vector2(0.666f, 0.25f);
        uv[10] = new Vector2(0.666f, 0.5f);
        uv[11] = new Vector2(0.333f, 0.5f);

        // right -side
        uv[12] = new Vector2(0.333f, 0.25f);
        uv[13] = new Vector2(0.666f, 0.25f);
        uv[14] = new Vector2(0.666f, 0.5f);
        uv[15] = new Vector2(0.333f, 0.5f);

        // top - grass
        uv[16] = new Vector2(0.333f, 0.5f);
        uv[17] = new Vector2(0.666f, 0.5f);
        uv[18] = new Vector2(0.666f, 0.75f);
        uv[19] = new Vector2(0.333f, 0.75f);

        // bottom - dirt
        uv[20] = new Vector2(0.333f, 0f);
        uv[21] = new Vector2(0.666f, 0f);
        uv[22] = new Vector2(0.666f, 0.25f);
        uv[23] = new Vector2(0.333f, 0.25f);
        
        mesh.uv = uv;
    }
}