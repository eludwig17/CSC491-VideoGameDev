using UnityEngine;
using TMPro;

public class FlappyBirdUIManager : MonoBehaviour {
    [Header("Main Menu")]
    public GameObject mainMenuPanel;
    public GameObject quitButton;

    [Header("HUD")]
    public GameObject hudPage; 
    public TextMeshProUGUI scoreText;

    [Header("Pause Menu")]
    public GameObject pauseMenu; 

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverHighScoreText; 
    
    private FlapGameManager _gameManager;
    private bool _isPaused = false;
    private bool _gameStarted = false;
    
     void Start(){
         _gameManager = FindFirstObjectByType<FlapGameManager>();
         ShowMainMenu();
         Time.timeScale = 0f;
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
         Time.timeScale = 0f;
         
         mainMenuPanel.SetActive(true);
         hudPage.SetActive(false);
         pauseMenu.SetActive(false);
         gameOverPanel.SetActive(false);
         quitButton.SetActive(true);

         FlapGameManager.IsGameOver = true;
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
     }

     void UpdateGameOverScreen(){
         if (gameOverPanel == null) 
             return;
         if (!_gameStarted)
             return;
         
         if (FlapGameManager.IsGameOver && !gameOverPanel.activeSelf){
             gameOverPanel.SetActive(true);
             if (gameOverHighScoreText != null)
                 gameOverHighScoreText.text = "High Score: " + Mathf.FloorToInt(_gameManager.GetHighScore());
         }
         else if (!FlapGameManager.IsGameOver && gameOverPanel.activeSelf){
             gameOverPanel.SetActive(false);
         }
     }

     public void PlayGame(){
         
     }

     public void MainMenu(){
         _gameStarted = false;
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
         if (FlapGameManager.IsGameOver)
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
         quitButton.SetActive(true);
     }
     
     public void HideQuitPage(){
         quitButton.SetActive(false);
     }

     public void QuitGame(){
         Application.Quit();
         #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #endif
     }
}