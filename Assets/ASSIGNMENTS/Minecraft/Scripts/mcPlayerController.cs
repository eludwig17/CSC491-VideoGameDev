using UnityEngine;

public class mcPlayerController : MonoBehaviour {
    [Header("Player Movement")] 
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float jump = 5f;
    
    [Header("Mouse Controls")] 
    public float mouseSense = 5f;
    public float minPinchClamp = -90f;
    public float maxPinchClamp = 90f;
    
    [Header("Ground Checker")]
    public float groundCheckDist = 1.1f;
    public LayerMask groundMask;
    
    private Rigidbody rb;
    private Transform camTransform;
    private float pitch = 0f;
    
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        camTransform = GetComponentInChildren<Camera>().transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        Jump();
    }

    void FixedUpdate(){
        MouseMovement();
        Movement();
    }

    void MouseMovement(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSense;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense;
        transform.Rotate(Vector3.up * mouseX);
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPinchClamp, maxPinchClamp);
        camTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void Movement(){
        float horizonal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 
        Vector3 moveDir = transform.forward * vertical + transform.right * horizonal;
        
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 vel = moveDir * speed;
        vel.y = rb.linearVelocity.y;
        rb.linearVelocity = vel;
    }

    void Jump(){
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }
    
    bool IsGrounded(){
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDist, groundMask);
    }
}
