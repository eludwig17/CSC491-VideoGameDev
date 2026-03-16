using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts{
    public class MeshGenerator : MonoBehaviour{
        public Material blankMat;
        private static Mesh _sharedCubeMesh;

        void Start(){
            if (_sharedCubeMesh == null)
                _sharedCubeMesh = GenNewMesh();

            SpawnBlock(new Vector3(0, 0, 0));
        }

        public GameObject SpawnBlock(Vector3 position){
            GameObject block = new GameObject("Block");
            block.transform.position = position;
            block.transform.parent = transform;
            block.AddComponent<BoxCollider>();
            block.AddComponent<Block>();
            MeshFilter meshFilter = block.AddComponent<MeshFilter>();
            meshFilter.mesh = _sharedCubeMesh;
            MeshRenderer meshRenderer = block.AddComponent<MeshRenderer>();
            meshRenderer.material = blankMat;
            block.AddComponent<BlockTexture>();
            return block;
        }

        static Mesh GenNewMesh(){
            Mesh mesh = new Mesh();
            mesh.name = "MinecraftCubeMesh";

            Vector3[] vertices = new Vector3[]{
                // front
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f,  0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f,  0.5f),

                // back
                new Vector3( 0.5f, -0.5f, -0.5f), 
                new Vector3(-0.5f, -0.5f, -0.5f), 
                new Vector3(-0.5f,  0.5f, -0.5f),
                new Vector3( 0.5f,  0.5f, -0.5f), 

                // left
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f, -0.5f),

                // right
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f,  0.5f, -0.5f),
                new Vector3( 0.5f,  0.5f,  0.5f),

                // top
                new Vector3(-0.5f,  0.5f,  0.5f),
                new Vector3( 0.5f,  0.5f,  0.5f),
                new Vector3( 0.5f,  0.5f, -0.5f),
                new Vector3(-0.5f,  0.5f, -0.5f),

                // bottom
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3(-0.5f, -0.5f,  0.5f),
            };

            int[] triangles = new int[]{
                // front
                0, 1, 2,
                0, 2, 3,
                // back
                4, 5, 6,
                4, 6, 7,
                // left
                8, 9, 10,
                8, 10, 11,
                // right
                12, 13, 14,
                12, 14, 15,
                // top
                16, 17, 18,
                16, 18, 19,
                // bottom
                20, 21, 22,
                20, 22, 23,
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = new Vector2[24];
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}