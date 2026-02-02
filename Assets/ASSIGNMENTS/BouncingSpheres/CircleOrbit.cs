using System;
using UnityEngine;

public class CircleOrbiti : MonoBehaviour{
    public float radius = 2.0f;

    void Update()
    {
        transform.position = new Vector3(Mathf.Cos(Time.time) * radius, 0f, Mathf.Sin(Time.time) * radius);    
    }
}
