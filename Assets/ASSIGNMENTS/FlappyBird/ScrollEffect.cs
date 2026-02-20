using UnityEngine;

public class ScrollEffect : MonoBehaviour {
    
    [SerializeField] public float speed = 3f;
    private float spriteWidth;
    private Vector3 startPos;

    void Start(){
        startPos = transform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update(){
        if (FlapGameManager.IsGameOver)
            return;
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x <= startPos.x - spriteWidth)
            transform.position = startPos;
    }
}
