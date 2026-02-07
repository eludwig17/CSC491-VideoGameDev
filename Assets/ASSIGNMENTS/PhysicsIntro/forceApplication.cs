using UnityEngine;

public class forceObjects : MonoBehaviour {
    [Header("Force Settings")]
    [SerializeField] private float forceAmount = 15f;  
    [SerializeField] private ForceMode forceMode = ForceMode.Force;  
    
    private Rigidbody rb;
    private Vector3 startPosition;
    
    void Start(){
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
    
    void Update(){
        if (Input.GetKey(KeyCode.Space)){
            ApplyForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.W)){
            ApplyForce(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S)){
            ApplyForce(Vector3.back);
        }
        if (Input.GetKey(KeyCode.A)){
            ApplyForce(Vector3.left);
        }
        if (Input.GetKey(KeyCode.D)){
            ApplyForce(Vector3.right);
        }
    }
    
    void ApplyForce(Vector3 direction){
        if (rb != null){
            rb.AddForce(direction * forceAmount);
        }
    }
}