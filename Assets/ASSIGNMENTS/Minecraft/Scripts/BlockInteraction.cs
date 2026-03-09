using UnityEngine;

public class BlockInteraction : MonoBehaviour{
    public GameObject blockPrefab;
    public float reachDist = 5f;
    public LayerMask blockLayer;
    private Camera cam;
    private Block currTarget;
    private float breakTime = 0f;
    
    void Start(){
        cam = GetComponentInChildren<Camera>();
    }

    void Update(){
        if (Input.GetMouseButton(0)){
            BreakBlock();
        }
        else{
            breakTime = 0f;
            currTarget = null;
        }

        if (Input.GetMouseButtonUp(1)){
            PlaceBlock();
;        }
    }

    void BreakBlock(){
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, reachDist, blockLayer)){
            Block block = hit.collider.GetComponent<Block>();
            if (block == null) return;

            if (block != currTarget){
                breakTime = 0f;
                currTarget = block;
            }
            breakTime += Time.deltaTime;
            block.TryBreak(breakTime);
        }
        else{
            breakTime = 0f;
            currTarget = null;
        }
    }

    void PlaceBlock(){
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, reachDist, blockLayer)){
            Vector3 placePosition = hit.collider.transform.position + hit.normal;
            Instantiate(blockPrefab, placePosition, Quaternion.identity);
        }
    }
}