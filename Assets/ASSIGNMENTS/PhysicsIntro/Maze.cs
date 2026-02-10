using UnityEngine;

public class Maze : MonoBehaviour{

    public float tiltSpeed = 15f;
    public float maxTiltAngle = 15f;

    private float currTiltX = 0f;
    private float currTiltZ = 0f;

    void Update(){
        float inputX = Input.GetAxis("Vertical");
        float inputZ = -Input.GetAxis("Horizontal");
        
        currTiltX += inputX * tiltSpeed * Time.deltaTime;
        currTiltZ += inputZ * tiltSpeed * Time.deltaTime;
        currTiltX = Mathf.Clamp(currTiltX, -maxTiltAngle, maxTiltAngle);
        currTiltZ = Mathf.Clamp(currTiltZ, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(currTiltX, 0f, currTiltZ);
    }
}
