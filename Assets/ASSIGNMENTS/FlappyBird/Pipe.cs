using UnityEngine;

public class Pipe : MonoBehaviour{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float despawnX = -15f;
    
    void Update(){
        transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        if (transform.position.x< despawnX){
            Destroy(gameObject);
        }
    }
}
