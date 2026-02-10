using UnityEngine;

public class PlayerRespawn : MonoBehaviour{
    public float fall = -15f;
    private Vector3 startPos;
    
    void Start(){
        startPos = transform.position;
    }

    void Update(){
        if (transform.position.y < fall){
            Respawn();
        }
    }

    void Respawn(){
        transform.position = startPos;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null){
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
