using UnityEngine;

namespace ASSIGNMENTS.Minecraft.Scripts{
	public class CubeSpawner : MonoBehaviour{
		[Tooltip("shared material for the uv mapping with the atlas map")]
		public Material atlasMaterial;

		private CubeMeshManager meshManager;

		void Awake(){
			meshManager = new CubeMeshManager();
		}

		public GameObject SpawnCube(Vector3 position, CubeType type){
			GameObject block = new GameObject($"Block_{type}");
			block.layer = LayerMask.NameToLayer("Block");
			block.transform.position = position;
			block.transform.parent = transform;

			block.AddComponent<BoxCollider>();
			block.AddComponent<Block>();

			MeshFilter mf = block.AddComponent<MeshFilter>();
			mf.mesh = meshManager.GetMesh(type); // reused, not duplicated

			MeshRenderer mr = block.AddComponent<MeshRenderer>();
			mr.material = atlasMaterial; // one shared material

			return block;
		}
	}
}