using UnityEngine;

public class WorldGeneration : MonoBehaviour{
    public GameObject blockPrefab;
    public int width = 16;
    public int length = 16;
    public int height = 3;

    void Start(){
        GenerateWorld();
    }

    void GenerateWorld(){
        for (int x = 0; x < width; x++){
            for (int z = 0; z < length; z++){
                for (int y = 0; y < height; y++){
                    Vector3 position = new Vector3(x, y, z);
                    Instantiate(blockPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
