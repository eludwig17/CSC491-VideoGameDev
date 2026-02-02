using UnityEngine;

public class GameLogic : MonoBehaviour{
    public float FallSpeed = 3f;
    public float CollisionDist = 1f;

    private GameObject player;
    private bool gameOver = false;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position += Vector3.down * FallSpeed * Time.deltaTime;
        if (player != null && !gameOver){
            if (Vector3.Distance(transform.position, player.transform.position) < CollisionDist){
                Debug.Log("Game Over! You died... rip");
                gameOver = true;
            }
        }
        
        if (transform.position.y < -10f){
            Destroy(gameObject);
        }
    }
}
