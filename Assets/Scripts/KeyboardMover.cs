using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour{
    public float Speed = 2f;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
            transform.position += new Vector3(0, Speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.A)){
            transform.position += new Vector3(-Speed * Time.deltaTime, 0, 0);

        }

        if (Input.GetKey(KeyCode.S)){
            transform.position += new Vector3(0, -Speed * Time.deltaTime, 0);

        }

        if (Input.GetKey(KeyCode.D)){
            transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);

        }
    }
}
