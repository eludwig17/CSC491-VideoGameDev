using UnityEngine;

public class BirdController : MonoBehaviour{
    [Header("Movement")]
    [SerializeField] private float flapStrength = 8f;
    
    private Rigidbody2D rb;
    private bool isAlive = true;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if (!isAlive) 
            return;
        if (Input.GetKeyDown(KeyCode.Space)){
            Flap();
        }
        RotateBird();
    }

    void Flap(){
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(0, flapStrength);
    }

    void RotateBird(){
        float rotation = Mathf.Lerp(-15, 45, (rb.linearVelocity.y + 10) / 15);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("Ground")){
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("ScoreZone")){
            // add game manager add score function here
        }
    }

    void Die(){
        isAlive = false;
        Debug.Log("You lost Game Over!");
    }
}