using UnityEngine;

public class Controller : MonoBehaviour {
    
    void Update(){
        Ray ray = new Ray();
        ray.origin = new Vector3(0, 0, 0);
        ray.direction = new Vector3(0, 1, 0);
        
        Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit)){
            Debug.Log("point: " + hit.point);
            Debug.Log("name: " + hit.collider.transform.name);
        }
        
    }
    
    
    
    
}
