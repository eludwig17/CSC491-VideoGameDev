using UnityEngine;
using UnityEngine.EventSystems;

public class BirdController : MonoBehaviour{
    [Header("Movement")]
    [SerializeField] private float flapStrength = 8f;
    [SerializeField] private float ceilingBounds = 10f;

    [Header("Wing Animation")] 
    [SerializeField] private Sprite[] flapSprites;
    [SerializeField] private float flapRate = 0.5f;

    [Header("Death Animation")] 
    [SerializeField] private float spinRate = 1000f;
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isAlive = true;
    private bool isDying = false;

    private int currFrame = 0;
    private float frameTime = 0f;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (isDying){
            transform.Rotate(0, 0, -spinRate * Time.deltaTime);
            return;
        }
        if (!isAlive || FlapGameManager.IsGameOver) 
            return;
        if (Input.GetKeyDown(KeyCode.Space) && !IsPointerOverUI()){
            Flap();
        }

        if (transform.position.y >= ceilingBounds){
            Vector3 pos = transform.position;
            pos.y = ceilingBounds;
            transform.position = pos;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
        RotateBird();
        AnimateWing();
    }

    void Flap(){
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(0, flapStrength);
        AudioManager.Instance.playFlap();
        currFrame = 0;
        frameTime = 0f;
    }

    void RotateBird(){
        float rotation = Mathf.Lerp(-15, 45, (rb.linearVelocity.y + 10) / 15);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Pipe") || collision.gameObject.CompareTag("Ground")){
            Die();
        }
        AudioManager.Instance.playHit();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("ScoreZone") && isAlive){
            FlapGameManager.Instance.AddScore();
            AudioManager.Instance.playScore();
        }
    }

    void Die(){
        if (!isAlive)
            return;
        isAlive = false;
        isDying = true;
        rb.linearVelocity = Vector2.zero;
        AudioManager.Instance.playHit();
        Invoke(nameof(endDeath), 1f);
    }

    void endDeath(){
        isDying = false;
        FlapGameManager.Instance.GameOver();
        AudioManager.Instance.playGameOver();
    }
    

    void AnimateWing(){
        if (flapSprites == null || flapSprites.Length == 0)
            return;
        frameTime += Time.deltaTime;
        if (frameTime >= flapRate){
            frameTime = 0f;
            currFrame = (currFrame + 1) % flapSprites.Length;
            sr.sprite = flapSprites[currFrame];
        }
    }

    public void ResetBird(){
        isDying = false;
        currFrame = 0;
        frameTime = 0f;
        rb.gravityScale = 1f;
        if (sr == null) sr = transform.GetComponent<SpriteRenderer>();
        if (flapSprites != null && flapSprites.Length > 0){
            sr.sprite = flapSprites[0];
        }
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        isAlive = true;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector2.zero;
    }
    
    bool IsPointerOverUI(){
        return EventSystem.current.IsPointerOverGameObject();
    }
}