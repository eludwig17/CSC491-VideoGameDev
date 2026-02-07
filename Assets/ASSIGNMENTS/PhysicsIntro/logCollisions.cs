using UnityEngine;

public class logCollisions : MonoBehaviour{
   private Rigidbody rb;

   void Start(){
      rb = GetComponent<Rigidbody>();
   }

   void OnCollisionEnter(Collision other){
      Debug.Log($"{gameObject.name} has collided with {other.gameObject.name}");

      if (other.gameObject.tag == "Plane"){
         Debug.Log($"{gameObject.name} has landed on the green ground");
      }
      
      DebugObjectInfo(other);
   }

   void DebugObjectInfo(Collision collision){
      // Objects info
      Debug.Log("Collision Info" +
                $"\nObject: {gameObject.name}" +
                $"\nPosition: {transform.position}"
      );
      if (rb != null){
         Debug.Log($"Velocity: {rb.linearVelocity}" +
                   $"Mass: {rb.mass}");
      }
      // Object collision info
      Debug.Log($"Collided with {collision.gameObject.name}" +
                $"\nObject Position: {collision.gameObject.transform.position}");
      // Collision info
      Debug.Log($"Contact Points: {collision.contactCount}" +
                $"\nRelative Velocity: {collision.relativeVelocity}" +
                $"\nImpulse: {collision.impulse}");
   }
   
}
