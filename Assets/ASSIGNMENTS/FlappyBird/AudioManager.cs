using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour{

    public static AudioManager Instance;
    
    [Header("Sound Effects")]
    public AudioClip flapSound;
    public AudioClip gameOverSound;
    public AudioClip scoreSound;
    public AudioClip uiSound;
    public AudioClip hitSound; 
    
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float musicVolume = 0.5f; 

    
    private AudioSource musicSource;
    private AudioSource effectSource;

    void Awake(){
        if (Instance == null)
            Instance = this;
        else{
            Destroy(gameObject);
            return; 
        }
        effectSource = gameObject.AddComponent<AudioSource>();
        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.clip = backgroundMusic;
    }
    
    public void playFlap()   => effectSource.PlayOneShot(flapSound);
    public void playScore()  => effectSource.PlayOneShot(scoreSound);
    public void playHit()    => effectSource.PlayOneShot(hitSound);
    public void playGameOver()  => effectSource.PlayOneShot(gameOverSound);
    public void playClick()  => effectSource.PlayOneShot(uiSound);

    public void startMusic() => musicSource.Play();
    public void stopMusic()  => musicSource.Stop();

    public void RegisterAllButtons(){
        foreach (Button btn in FindObjectsByType<Button>(FindObjectsSortMode.None))
            btn.onClick.AddListener(playClick);
    }
}
