using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts{
    public class WorldGeneration : MonoBehaviour{
        public GameObject blockPrefab;
        public int width = 50;
        public int length = 50;

        public int baseHeight = 5;

        [Header("Perlin Noise")] public int seed = 42;
        [Range(0.01f, 20f)] public float noiseScale = 5f;
        [Header("Entity")]
        public Transform player;
        public Transform zombie;
        
        private int DirtLayers = 3;
        private CubeSpawner cubeSpawner;
        private bool needsReposition = true;

        void Awake(){
            cubeSpawner = GetComponent<CubeSpawner>();
            if (cubeSpawner == null)
                Debug.LogError("worldgeneration needs a cubespawner on same gameobject");
        }

        void Start(){
            GenerateWorld();
        }
        
        private int repositionCountdown = 3;
        void LateUpdate(){
            if (repositionCountdown <= 0) return;
            repositionCountdown--;
            if (repositionCountdown == 0) {
                reposEntity(player, width / 2, length / 2);
                reposEntity(zombie, width / 4, length / 4);
            }
        }
        
        void GenerateWorld() {
            for (int x = 0; x < width; x++)
            for (int z = 0; z < length; z++) {
                int colHeight = getColHeight(x, z);
                for (int y = 0; y < colHeight; y++) {
                    Vector3 pos = new Vector3(x, y, z);
                    CubeType type = GetBlockType(y, colHeight);
                    cubeSpawner.SpawnCube(pos, type);
                }
            }
        }
        
        int getColHeight(int x, int z) {
            float xCoord = (float)x / width * noiseScale + seed;
            float zCoord = (float)z / length * noiseScale + seed;
            float noise = Mathf.PerlinNoise(xCoord, zCoord);
            return baseHeight + Mathf.RoundToInt(noise * noiseScale);
        }

        void reposEntity(Transform entity, int x, int z) {
            if (entity == null) return;
            int h = getColHeight(x, z);
            Rigidbody rb = entity.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = true;
                entity.position = new Vector3(x, h + 3f, z);
                StartCoroutine(ReenablePhysics(rb));
            }
            else {
                entity.position = new Vector3(x, h + 3f, z);
            }
        }
        System.Collections.IEnumerator ReenablePhysics(Rigidbody rb) {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
        }
        
        CubeType GetBlockType(int y, int colHeight) {
            if (y == colHeight - 1) return CubeType.Grass;
            if (y >= colHeight - 1 - DirtLayers) return CubeType.Dirt;
            return CubeType.Stone;
        }
    }
}