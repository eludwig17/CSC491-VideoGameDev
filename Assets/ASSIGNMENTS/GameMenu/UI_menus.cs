using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {
    public GameObject mainMenu, optionsMenu, creditsMenu, quitPopup;

    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;

    public Toggle fullscreenToggle;
    public TextMeshProUGUI fullscreenText;

    public TMP_Dropdown resolutionDropdown;
    public TextMeshProUGUI resolutionText;

    void Start(){
        ShowMainMenu();
        UpdateVolume();
        UpdateFullscreen();
        UpdateResolution();
    }

    public void ShowMainMenu(){
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitPopup.SetActive(false);
    }

    public void ShowOptions(){
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void ShowCredits(){
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ShowQuitPopup() => quitPopup.SetActive(true);
    public void HideQuitPopup() => quitPopup.SetActive(false);

    public void UpdateVolume(){
        volumeText.text = "Volume: " + Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }

    public void UpdateFullscreen(){
        fullscreenText.text = "Fullscreen: " + (fullscreenToggle.isOn ? "On" : "Off");
    }

    public void UpdateResolution(){
        resolutionText.text = "Resolution: " + resolutionDropdown.options[resolutionDropdown.value].text;
    }

    public void QuitGame(){
        Application.Quit();
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}