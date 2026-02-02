using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public float minX = -5f;
    public float maxX = 5f;
    
    void Update(){
        if (DodgeWaveGameManager.IsGameOver)
            return;
        float moveInput = 0f;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            moveInput = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            moveInput = 1f;
        }    
        transform.position += new Vector3(moveInput * speed * Time.deltaTime, 0, 0);

        float clamped =  Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clamped, transform.position.y, transform.position.z);
    }
}
