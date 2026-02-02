using UnityEngine;

public class GameObstacle : MonoBehaviour{

    public float Interval = 2f;
    public float OriginHeight = 10f;
    public float MinX = -5f;
    public float MaxX = 5f;
    public float FallSpeed = 3f;

    private float timer;
    
    public static bool isGameOver = false;
    
    void Update(){
        if (isGameOver)
            return;
        timer += Time.deltaTime;
        if (timer > Interval){
            CreateObstacle();
            timer = 0f;
        }
    }

    void CreateObstacle(){
        float randomX = Random.Range(MinX, MaxX);
        Vector3 OriginPos =  new Vector3(randomX, OriginHeight, 0);
        
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.transform.position = OriginPos;
        obstacle.tag = "Obstacle";
        
        GameLogic gameLogic = obstacle.AddComponent<GameLogic>();
        gameLogic.FallSpeed = FallSpeed;
    }
}
