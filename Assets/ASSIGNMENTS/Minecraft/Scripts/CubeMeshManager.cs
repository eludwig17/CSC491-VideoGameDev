using UnityEngine;
using System.Collections.Generic;

namespace ASSIGNMENTS.Minecraft.Scripts{
    public class CubeMeshManager{
        private const int AtlasSize = 16;
        private const float UvUnit = 1f / AtlasSize;
        
        /*
         * In the mesh (col,row)
         * block - top, side, bottom
         * grass (0,0) (3,0) (2,0)
         * dirt (2,0) (2,0) (2,0) 
         * stone (1,1) (1,1) (1,1)
         * log (5,1) (4,1) (5,1)
         * lead (4,3) (4,3) (4,3)
         */        
       
        private static readonly Dictionary<CubeType, CubeUV> cubeUVs =
            new Dictionary<CubeType, CubeUV>{
                {
                    CubeType.Grass, new CubeUV(
                        top: new Vector2Int(0, 0), 
                        side: new Vector2Int(3, 0),
                        bottom: new Vector2Int(2, 0)) 
                },{
                    CubeType.Dirt, new CubeUV(
                        top: new Vector2Int(2, 0),
                        side: new Vector2Int(2, 0),
                        bottom: new Vector2Int(2, 0))
                },{
                    CubeType.Stone, new CubeUV(
                        top: new Vector2Int(1, 1),
                        side: new Vector2Int(1, 1),
                        bottom: new Vector2Int(1, 1))
                }, {
                    CubeType.Log, new CubeUV(
                        top: new Vector2Int(5, 1),
                        side: new Vector2Int(4, 1),
                        bottom: new Vector2Int(5, 1))
                }, {
                    CubeType.Leaf, new CubeUV(
                        top: new Vector2Int(4,3),
                        side: new Vector2Int(4, 3),
                        bottom: new Vector2Int(4, 3))
                }, {
                    CubeType.Iron, new CubeUV(
                        top: new Vector2Int(1,2),
                        side: new Vector2Int(1, 2),
                        bottom: new Vector2Int(1, 2))
                }, {
                    CubeType.Coal, new CubeUV(
                        top: new Vector2Int(2,2),
                        side: new Vector2Int(2, 2),
                        bottom: new Vector2Int(2, 2))
                }, {
                    CubeType.Diamond, new CubeUV(
                        top: new Vector2Int(2,3),
                        side: new Vector2Int(2, 3),
                        bottom: new Vector2Int(2, 3))
                }, 
            };

        private Dictionary<CubeType, Mesh> meshCache = new Dictionary<CubeType, Mesh>();
        
        public Mesh GetMesh(CubeType type){
            if (!meshCache.TryGetValue(type, out Mesh mesh)){
                mesh = createCubeMesh(type);
                meshCache[type] = mesh;
            }

            return mesh;
        }

        Mesh createCubeMesh(CubeType type){
            Mesh mesh = BuildCubeMesh();
            mesh.name = $"CubeMesh_{type}";
            mesh.uv = calcUVs(type, new Vector2[24]);
            return mesh;
        }

        // cube geometry for mesh
        Mesh BuildCubeMesh(){
            Mesh mesh = new Mesh();

            mesh.vertices = new Vector3[]{
                //front
                new(-0.5f, -0.5f, 0.5f), new(0.5f, -0.5f, 0.5f),
                new(0.5f, 0.5f, 0.5f), new(-0.5f, 0.5f, 0.5f),
                // back   
                new(0.5f, -0.5f, -0.5f), new(-0.5f, -0.5f, -0.5f),
                new(-0.5f, 0.5f, -0.5f), new(0.5f, 0.5f, -0.5f),
                // left  
                new(-0.5f, -0.5f, -0.5f), new(-0.5f, -0.5f, 0.5f),
                new(-0.5f, 0.5f, 0.5f), new(-0.5f, 0.5f, -0.5f),
                // right  
                new(0.5f, -0.5f, 0.5f), new(0.5f, -0.5f, -0.5f),
                new(0.5f, 0.5f, -0.5f), new(0.5f, 0.5f, 0.5f),
                // top    
                new(-0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f),
                new(0.5f, 0.5f, -0.5f), new(-0.5f, 0.5f, -0.5f),
                // bottom 
                new(-0.5f, -0.5f, -0.5f), new(0.5f, -0.5f, -0.5f),
                new(0.5f, -0.5f, 0.5f), new(-0.5f, -0.5f, 0.5f),
            };

            mesh.triangles = new int[]{
                0, 1, 2, 0, 2, 3, // front
                4, 5, 6, 4, 6, 7, // back
                8, 9, 10, 8, 10, 11, // left
                12, 13, 14, 12, 14, 15, // right
                16, 17, 18, 16, 18, 19, // top
                20, 21, 22, 20, 22, 23, // bottom
            };

            mesh.RecalculateNormals();
            return mesh;
        }

        // uv slots
        Vector2[] calcUVs(CubeType type, Vector2[] uvs){
            CubeUV uv = cubeUVs[type];
            for (int face = 0; face < 4; face++){
                writeFaceUV(uvs, face * 4, uv.side);
            }
            writeFaceUV(uvs, 16, uv.top);
            writeFaceUV(uvs, 20, uv.bottom);
            return uvs;
        }
        
        void writeFaceUV(Vector2[] uvs, int startIdx, Vector2Int tile){
            float u0 = tile.x * UvUnit;
            float u1 = (tile.x + 1) * UvUnit;
            float v0 = 1f - (tile.y + 1) * UvUnit;
            float v1 = 1f - tile.y * UvUnit; 

            uvs[startIdx + 0] = new Vector2(u0, v0);
            uvs[startIdx + 1] = new Vector2(u1, v0);
            uvs[startIdx + 2] = new Vector2(u1, v1);
            uvs[startIdx + 3] = new Vector2(u0, v1);
        }
    }
}