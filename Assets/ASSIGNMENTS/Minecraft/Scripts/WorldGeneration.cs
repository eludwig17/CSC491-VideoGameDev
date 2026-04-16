using UnityEngine;
using System.Collections.Generic;

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

        [Header("Trees")] [Range(0f, 1f)] public float treeChance = 0.05f;
        public float treeDist = 10f;
        
        [Header("Dirt Layers")]
        private int DirtLayers = 3;
        private CubeSpawner cubeSpawner;
        private HashSet<Vector3Int> occupiedBlocks = new HashSet<Vector3Int>();
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
                    Vector3Int pos = new Vector3Int(x, y, z);
                    CubeType type = GetBlockType(y, colHeight);
                    cubeSpawner.SpawnCube(pos, type);
                    occupiedBlocks.Add(pos);
                }
            }
            genTrees();
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
        
        void genTrees() {
            System.Random rng = new System.Random(seed);
            List<Vector2Int> treePositions = new List<Vector2Int>();

            for (int x = 2; x < width - 2; x++)
            for (int z = 2; z < length - 2; z++) {
                float roll = (float)rng.NextDouble();
                if (roll > treeChance) continue;

                bool tooClose = false;
                foreach (var existing in treePositions) {
                    if (Vector2Int.Distance(new Vector2Int(x, z), existing) < treeDist) {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                treePositions.Add(new Vector2Int(x, z));
                int groundY = getColHeight(x, z);
                int trunkHeight = 1 + rng.Next(0, 3); 
                placeTree(x, z, groundY, trunkHeight);
            }
        }
        
        void placeTree(int x, int z, int groundY, int trunkHeight) {
            for (int y = groundY; y < groundY + trunkHeight; y++) {
                placeBlock(x, y, z, CubeType.Log);
            }

            int leafBase = groundY + trunkHeight - 1;
            for (int ly = leafBase; ly <= leafBase + 2; ly++)
            for (int lx = x - 1; lx <= x + 1; lx++)
            for (int lz = z - 1; lz <= z + 1; lz++) {
                if (lx == x && lz == z && ly < groundY + trunkHeight)
                    continue; 
                placeBlock(lx, ly, lz, CubeType.Leaf);
            }
            placeBlock(x, leafBase + 3, z, CubeType.Leaf);
        }

        void placeBlock(int x, int y, int z, CubeType type) {
            Vector3Int pos = new Vector3Int(x, y, z);
            if (occupiedBlocks.Contains(pos)) return;
            occupiedBlocks.Add(pos);
            cubeSpawner.SpawnCube(pos, type);
        }
    }
}