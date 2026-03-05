using UnityEngine;

namespace ASSIGNMENTS.IntroToRaycasts{
    public class Controller : MonoBehaviour {
        [Header("Layer Masks")]
        public LayerMask pickupLayer;
        public LayerMask environmentLayer;
        
        [Header("Ray Settings")]
        public float maxDistance = 15f;
        
        [Header("Holding Settings")]
        public float holdingHeight = 2f;
        
        [Header("Keybinds")]
        public KeyCode pickup = KeyCode.E;
        public KeyCode drop = KeyCode.Q;

        private SimplePickupSystem _pickupSystem;
        private Camera _camera;

        private Ray _ray;
        private RaycastHit _pickupHit;
        private RaycastHit _environmentHit;
        private bool _hitPickup;
        private bool _hitEnvironment;
        private bool _isHolding;

        void Awake(){
            _pickupSystem = GetComponent<SimplePickupSystem>();
            _camera = Camera.main;
            if (_camera == null)
                Debug.LogError("Controller cannot find the main camera!");
        }

        void Update(){
            CastRay();
            TakeInput();
            UpdateObjectPos();
            DebugRay();
        }

        void CastRay(){
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            _hitPickup = Physics.Raycast(_ray, out _pickupHit, maxDistance, pickupLayer);
            _hitEnvironment = Physics.Raycast(_ray, out _environmentHit, maxDistance, environmentLayer);
        }

        void TakeInput(){
            if (Input.GetKeyDown(pickup) && !_isHolding){
                if (_hitPickup){
                    if (IsInLayerMask(_pickupHit.collider.gameObject, pickupLayer)){
                        _pickupSystem.Pickup(_pickupHit.collider.gameObject);
                        _isHolding = true;
                    }
                }
            }

            if (Input.GetKeyDown(drop) && _isHolding){
                _pickupSystem.Drop();
                _isHolding = false;
            }
        }

        void UpdateObjectPos(){
            if (!_isHolding) 
                return;
            if (_hitEnvironment){
                _pickupSystem.UpdatePickupPosition(_environmentHit.point + Vector3.up * holdingHeight);
            } 
            else{
                _pickupSystem.UpdatePickupPosition(_ray.GetPoint(maxDistance * 0.5f) + Vector3.up * holdingHeight);
            }
        }

        void DebugRay(){
            Debug.Log("Hit Pickup: " + _hitPickup + " | Hit Environment: " + _hitEnvironment);

            
            Color rayColor;
            float drawLength = maxDistance;

            if (_hitPickup){
                rayColor = Color.green;
                drawLength = _pickupHit.distance;
            } 
            else if (_hitEnvironment){
                rayColor = Color.yellow;
                drawLength = _environmentHit.distance;
            } 
            else{
                rayColor = Color.red;
            }

            Debug.DrawRay(_ray.origin, _ray.direction * drawLength, rayColor);
        }

        bool IsInLayerMask(GameObject obj, LayerMask mask){
                return ((1 << obj.layer) & mask) != 0;
        }  
    }
}
