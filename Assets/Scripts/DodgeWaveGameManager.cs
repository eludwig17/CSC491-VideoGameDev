using UnityEngine;
using System.Collections.Generic;


public class DodgeWaveGameManager : MonoBehaviour{
    public static bool IsGameOver;
    public static bool IsIntermission;
    
    public float waveDuration = 15f;
    public float intermissionDuration = 0.5f;
    public float baseSpeed = 3f;
    public float speedIncreasePerWave = 0.5f;
    
    
    public float GetScore() => _score;
    public float GetHighScore() => _highScore;
    public int GetCurrentWave() => _currentWave;
    
    private int _currentWave = 1;
    private int _obstacleCount = 3;
    private int _sliderCount;
    private float _score;
    private float _highScore;
    private float _currentSpeed;
    private float _waveTimer;
    private float _intermissionTimer;
    private List<GameObject> _obstacles = new List<GameObject>();
    private GameObject _player;
    

    public void InitGame(){
        _player = GameObject.FindGameObjectWithTag("Player");
        
        IsGameOver = false;
        IsIntermission = false;
        _score = 0f;
        _currentWave = 1;
        _currentSpeed = baseSpeed;
        _obstacleCount = 3;
        _sliderCount = 0;
        _waveTimer = 0f;
        _intermissionTimer = 0f;

        ClearAllObstacles();
        SpawnObstacles();
       
        GameObject[] sceneObjects = GameObject.FindGameObjectsWithTag("sceneObjects");
        foreach (GameObject obj in sceneObjects)
            obj.GetComponent<Renderer>().material.color = Color.white;
    
        if (_player != null)
            _player.GetComponent<Renderer>().material.color = Color.black;

        // Debug.Log("Wave " + _currentWave + " - Speed: " + _currentSpeed + " - Obstacles: " + _obstacleCount + " - Sliders: " + _sliderCount);
    }

    void Start(){
        
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.R)){
            InitGame();
            return;
        }
        if (IsGameOver) return;

        if (IsIntermission){
            _intermissionTimer += Time.deltaTime;
            if (_intermissionTimer >= intermissionDuration)
                EndIntermission();
            return;
        }
        _score += Time.deltaTime;
        _waveTimer += Time.deltaTime;
        
        if (_waveTimer >= waveDuration)
            StartIntermission();

        UpdateObstacles();
        CheckCollisions();
    }

    void StartIntermission(){
        IsIntermission = true;
        _intermissionTimer = 0f;
        _currentWave++;
        _currentSpeed = baseSpeed + speedIncreasePerWave * (_currentWave - 1);
        _obstacleCount = Mathf.Min(3 + 2 * (_currentWave - 1), 20);
        
        if (_currentWave % 2 == 0)
            _sliderCount = Mathf.Min(_sliderCount + 1, _obstacleCount);

        Debug.Log("Intermission! Next wave: " + _currentWave);
    }

    void EndIntermission(){
        IsIntermission = false;
        _waveTimer = 0f;

        while (_obstacles.Count < _obstacleCount)
            SpawnSingleObstacle(_obstacles.Count < _sliderCount);

        UpdateSliderStatus();
        Debug.Log("Wave " + _currentWave + " - Speed: " + _currentSpeed + " - Obstacles: " + _obstacleCount + " - Sliders: " + _sliderCount);
    }

    void SpawnObstacles(){
        for (int i = 0; i < _obstacleCount; i++)
            SpawnSingleObstacle(i < _sliderCount);
    }
    
    void SpawnSingleObstacle(bool isSlider){
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obstacle.tag = "Obstacle";
        obstacle.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(8f, 13f), -1.25f);
        obstacle.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

        WaveObstacles controller = obstacle.AddComponent<WaveObstacles>();
        controller.isSlider = isSlider;
        obstacle.GetComponent<Renderer>().material.color = isSlider ? Color.red : Color.white;

        _obstacles.Add(obstacle);
    }

    void UpdateSliderStatus(){
        for (int i = 0; i < _obstacles.Count; i++){
            if (_obstacles[i] == null) continue;
            
            bool shouldBeSlider = i < _sliderCount;
            _obstacles[i].GetComponent<WaveObstacles>().isSlider = shouldBeSlider;
            _obstacles[i].GetComponent<Renderer>().material.color = shouldBeSlider ? Color.red : Color.white;
        }
    }

    void UpdateObstacles(){
        foreach (GameObject obstacle in _obstacles)
            if (obstacle != null)
                obstacle.GetComponent<WaveObstacles>().UpdateMovement(_currentSpeed);
    }

    void CheckCollisions(){
        foreach (GameObject obstacle in _obstacles){
            if (obstacle == null) continue;
            
            float collisionDist = (_player.transform.localScale.x + obstacle.transform.localScale.x) / 2f;
            if (Vector3.Distance(_player.transform.position, obstacle.transform.position) < collisionDist){
                GameOver();
                return;
            }
        }
    }

    void GameOver(){
        IsGameOver = true;
        if(_score > _highScore)
            _highScore = _score;
        Debug.Log("WOMP WOMP GAME OVER!" + "\nYour Score: " + Mathf.FloorToInt(_score) + 
                  "\nYour high score is: " + Mathf.FloorToInt(_highScore) +
                  "\nPress R to restart game!");
    }

    public void ClearAllObstacles(){
        foreach (GameObject obstacle in _obstacles)
                    if (obstacle != null)
                        Destroy(obstacle);
        _obstacles.Clear();
    }
}
