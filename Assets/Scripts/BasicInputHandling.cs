using UnityEngine;

public class BasicInputHandling : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Space button was pressed");
        }
        
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        
        Debug.Log("xAxis: " + xAxis + " yAxis: " + yAxis);
    }
}
