using UnityEngine;

public class FlapGameManager : MonoBehaviour {
    public static  FlapGameManager Instance{ get; private set; }

    private int currScore = 0;

    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void AddScore(){
        currScore++;
        Debug.Log("Current Score: " + currScore);
    }

    public void GameOver(){
        Debug.Log("Final score is " + currScore);
    }
}
