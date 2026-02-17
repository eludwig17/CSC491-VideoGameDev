using UnityEngine;

public class PipeSpawner : MonoBehaviour {
   [Header("Spawn Pipes")]
   [SerializeField] private GameObject pipePrefab;
   [SerializeField] private float spawnInterval = 3f;
   [SerializeField] private float spawnX = 20f;
   
   [Header("Randomize Height")]
   [SerializeField] private float minHeight = -5f;
   [SerializeField] private float maxHeight = 5f;

   private float timer = 0f;
   private bool isSpawning = false;

   void Update(){
      if (!isSpawning || FlapGameManager.IsGameOver)
         return;
      timer += Time.deltaTime;
      if (timer >= spawnInterval){
         Spawn();
         timer = 0f;
      }
   }

   void Spawn(){
      float randomY = Random.Range(minHeight, maxHeight);
      Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);
      Instantiate(pipePrefab, spawnPos, Quaternion.identity);
   }

   public void StopSpawning(){
      isSpawning = false;
   }
   
   public void StartSpawning(){
      isSpawning = true;
   }

}
