using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts{
    public class WorldGeneration : MonoBehaviour{
        public GameObject blockPrefab;
        public int width = 16;
        public int length = 16;
        public int height = 3;

        private CubeSpawner cubeSpawner;

        void Awake(){
            cubeSpawner = GetComponent<CubeSpawner>();
            if (cubeSpawner == null)
                Debug.LogError("worldgeneration needs a cubespawner on same gameobject");
        }

        void Start(){
            GenerateWorld();
        }

        void GenerateWorld(){
            for (int x = 0; x < width; x++)
            for (int z = 0; z < length; z++)
            for (int y = 0; y < height; y++){
                Vector3 pos = new Vector3(x, y, z);
                CubeType type = GetBlockType(x, y, z);
                cubeSpawner.SpawnCube(pos, type);
            }
        }


        CubeType GetBlockType(int x, int y, int z){
            if (y == height - 1) return CubeType.Grass;
            if (y == height - 2) return CubeType.Dirt;
            return CubeType.Stone;
        }
    }
}