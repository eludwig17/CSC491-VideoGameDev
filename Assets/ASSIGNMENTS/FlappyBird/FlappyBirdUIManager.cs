using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    
    [Header("Volume Settings")]
    public Slider volSlider;
    public TextMeshProUGUI volText;
    
    private FlapGameManager _gameManager;
    private bool _isPaused = false;
    private bool _gameStarted = false;
    
     void Start(){
         _gameManager = FindFirstObjectByType<FlapGameManager>();
         ShowMainMenu();
         Time.timeScale = 0f;
         AudioManager.Instance.RegisterAllButtons();
         AudioManager.Instance.startMusic();
         float saved = PlayerPrefs.GetFloat("Volume", 0.5f);
         volSlider.value = saved;
         UpdateVolText(saved);
         AudioManager.Instance.SetVol(saved);
         volSlider.onValueChanged.AddListener(VolChange);
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
         AudioManager.Instance.stopMusic();
     }

     public void ResumeGame(){
         _isPaused = false;
         Time.timeScale = 1f;
         pauseMenu.SetActive(false);
         AudioManager.Instance.startMusic();
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

     void VolChange(float value){
         AudioManager.Instance.SetVol(value);
         UpdateVolText(value);
         PlayerPrefs.SetFloat("Volume", value);
     }

     void UpdateVolText(float value){
         volText.text = "VOLUME: " + Mathf.RoundToInt(value * 100f) + "%";
     }
     
}