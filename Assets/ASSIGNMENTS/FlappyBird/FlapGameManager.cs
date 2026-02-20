using UnityEngine;

public class FlapGameManager : MonoBehaviour {
    public static FlapGameManager Instance{ get; private set; }

    private int _currScore;
    private int _highScore;
    public float GetScore() => _currScore;
    public float GetHighScore() => _highScore;
    
    public static bool IsGameOver;
    
    private GameObject _player;
    private PipeSpawner _pipeSpawner;

    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Start(){
        _pipeSpawner = FindObjectOfType<PipeSpawner>();
    }
    
    public void InitGame(){
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null){
            _player.transform.position = Vector3.zero;
            Rigidbody2D rb = _player.GetComponent<Rigidbody2D>();
            if (rb != null){
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        BirdController bird = _player.GetComponent<BirdController>();
        if (bird != null){
            bird.ResetBird();
        }

        ClearPipes();
        if (_pipeSpawner != null){
            _pipeSpawner.StartSpawning();
        }
        
        IsGameOver = false;
        _currScore = 0;
        Time.timeScale = 1f;
    }

    void ClearPipes(){
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");
        foreach (GameObject pipe in pipes){
            Destroy(pipe);
        }
        GameObject[] scoreZones = GameObject.FindGameObjectsWithTag("ScoreZone");
        foreach (GameObject zone in scoreZones){
            Destroy(zone);
        }
    }
    
    public void AddScore(){
        _currScore++;
        Debug.Log("Current Score: " + _currScore);
    }

    public void GameOver(){
        if (IsGameOver)
            return;
        IsGameOver = true;
        if (_currScore > _highScore){
            _highScore = _currScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.Save();
        }

        if (_pipeSpawner != null){
            _pipeSpawner.StopSpawning();
        }
    }
}
