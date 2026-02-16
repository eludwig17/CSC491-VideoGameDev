using UnityEngine;

public class FlapGameManager : MonoBehaviour {
    public static FlapGameManager Instance{ get; private set; }

    private int _currScore;
    
    public static bool IsGameOver;
    
    public float GetScore() => _currScore;
    public float GetHighScore() => _highScore;
    
    private float _highScore;
    private GameObject _player;
    

    public void InitGame(){
        _player = GameObject.FindGameObjectWithTag("Player");
        IsGameOver = false;
        _currScore = 0;
    }
    

    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void AddScore(){
        _currScore++;
        Debug.Log("Current Score: " + _currScore);
    }

    public void GameOver(){
        IsGameOver = true;
        //Debug.Log("Final score is " + currScore);
    }
}
