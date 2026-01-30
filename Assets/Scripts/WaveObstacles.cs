using UnityEngine;

public class WaveObstacles : MonoBehaviour
{
    public bool isSlider;
    public float sliderAmplitude = 3f;
    public float sliderFrequency = 2;

    private float _sineTime;
    private float _centerX;

    void Start(){
        _centerX = transform.position.x;
        _sineTime = Random.Range(0f, Mathf.PI * 2f);
    }

    public void UpdateMovement(float speed){
        if (transform.position.y < -6f)
            ResetObstacle();
        
        if (DodgeWaveGameManager.IsIntermission)
            return;

        transform.position += Time.deltaTime * speed * Vector3.down;

        if (isSlider){
            _sineTime += Time.deltaTime * sliderFrequency;
            float newX = Mathf.Clamp(_centerX + Mathf.Sin(_sineTime) * sliderAmplitude, -5f, 5f);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    void ResetObstacle(){
        _centerX = Random.Range(-5f, 5f);
        float newScale = Random.Range(0.5f, 1.5f);
        transform.localScale = Vector3.one * newScale;
        transform.position = new Vector3(_centerX, 8f, -1.25f);
        _sineTime = Random.Range(0f, Mathf.PI * 2f);
    }
}
