using UnityEngine;

public class MazeWinZone : MonoBehaviour{
    public GameObject winText;
    void Start(){
        if(winText != null)
            winText.SetActive(false);
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            if(winText != null)
                winText.SetActive(true);
            
            Time.timeScale = 0f;
        }
    }
}
