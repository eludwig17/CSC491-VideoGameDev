using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DodgeWaveUI : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject mainMenuPanel;
    public GameObject quitPage;

    [Header("HUD")]
    public GameObject hudPage; 
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;

    [Header("Pause Menu")]
    public GameObject pauseMenu; 

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverHighScoreText; 
    
    private DodgeWaveGameManager _gameManager;
    private bool _isPaused = false;
    private bool _gameStarted = false;
    
    void Start(){
        _gameManager = FindFirstObjectByType<DodgeWaveGameManager>();
        ShowMainMenu();
    }

    void Update(){
        if (_gameStarted){
            UpdateHUD();
            UpdateGameOverScreen();
            PauseInput();
        }
    }

    void ShowMainMenu(){
        _gameStarted = false;
        _isPaused = false;
        Time.timeScale = 1f;
        
        mainMenuPanel.SetActive(true);
        hudPage.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        quitPage.SetActive(false);

        DodgeWaveGameManager.IsGameOver = true;
    }

    public void StartGame(){
        _gameStarted = true;
        mainMenuPanel.SetActive(false);
        hudPage.SetActive(true);
        _gameManager.InitGame();
    }

    void UpdateHUD(){
        if (_gameManager == null)
            return;
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(_gameManager.GetScore());
        if (waveText != null)
            waveText.text = "Wave: " + _gameManager.GetCurrentWave();
       
    }

    void UpdateGameOverScreen(){
        if (gameOverPanel == null) return;
        
        if (DodgeWaveGameManager.IsGameOver && !gameOverPanel.activeSelf){
            gameOverPanel.SetActive(true);
            if (gameOverHighScoreText != null)
                gameOverHighScoreText.text = "High Score: " + Mathf.FloorToInt(_gameManager.GetHighScore());
        }
        else if (!DodgeWaveGameManager.IsGameOver && gameOverPanel.activeSelf){
            gameOverPanel.SetActive(false);
        }
    }

    public void PlayGame(){
        
    }

    public void MainMenu(){
        if (_gameManager != null){
            _gameManager.ClearAllObstacles();
        }
        ShowMainMenu();
    }

    public void PauseGame(){
        _isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame(){
        _isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    void PauseInput(){
        if (DodgeWaveGameManager.IsGameOver)
            return;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        
    } 
    
    public void RestartGame(){
        Time.timeScale = 1f;
        _isPaused = false;
        _gameStarted = true;
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        
        _gameManager.InitGame();
    }

    public void ShowQuitPage(){
        quitPage.SetActive(true);
    }
    
    public void HideQuitPage(){
        quitPage.SetActive(false);
    }

    public void QuitGame(){
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}
