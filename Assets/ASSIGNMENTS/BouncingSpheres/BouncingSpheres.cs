using UnityEngine;

public class BouncingSpheres : MonoBehaviour
{
    public float Speed = 4f;
    public float reverseDist = 1.0f;

    private Vector3 moveDirection;
    private GameObject[] spheres;
    private GameObject[] walls;

    void Start(){
        moveDirection = transform.position.x < 0f ? Vector3.right : Vector3.left;
        spheres = GameObject.FindGameObjectsWithTag("Sphere");
        walls = GameObject.FindGameObjectsWithTag("Wall");
    }
    
    void Update()
    {
        if (reverse()){
            moveDirection = -moveDirection;
        }
        transform.position += moveDirection * Speed * Time.deltaTime;
    }

    bool reverse(){
        foreach (GameObject sphere in spheres){
            if (sphere == gameObject)
                continue;
            if (Vector3.Distance(transform.position, sphere.transform.position) < reverseDist)
                return true;
        }
        
        foreach (GameObject wall in walls){
            if (Vector3.Distance(transform.position, wall.transform.position) < reverseDist)
                return true;
        }
        return false;
    }
}
