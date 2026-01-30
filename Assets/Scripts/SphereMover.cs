using UnityEngine;

public class SphereMover : MonoBehaviour
{
    public float Speed = 2.5f;
    public float WaitTime = 0.5f;
    public float MinX = -4.0f;
    public float MaxX = 4.0f;

    private float waitTimer = 0f;
    private bool isMoving = true;
    
    void Update(){
        if (waitTimer > 0){
            waitTimer -= Time.deltaTime;
            return;
        }
        transform.position += Vector3.right * (isMoving ? 1 : -1) * Speed * Time.deltaTime;
        if (transform.position.x >= MaxX || transform.position.x <= MinX){
            isMoving = !isMoving;
            waitTimer = WaitTime;
        }
    }
}
