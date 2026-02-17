using UnityEngine;

public class Pipe : MonoBehaviour{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float despawnX = -20f;
    
    void Update(){
        if (FlapGameManager.IsGameOver)
            return;
        transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        if (transform.position.x< despawnX){
            Destroy(gameObject);
        }
    }
}
