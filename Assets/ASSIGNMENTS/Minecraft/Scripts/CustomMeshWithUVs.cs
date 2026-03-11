using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomPlaneUV : MonoBehaviour
{
	public Material material; // Assign your image in the inspector

	void Start()
	{
		Mesh mesh = new Mesh();
		mesh.name = "CustomPlane";

		// Define 4 vertices of a plane
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-0.5f, -0.5f, 0), // bottom-left
			new Vector3(0.5f, -0.5f, 0),  // bottom-right
			new Vector3(0.5f, 0.5f, 0),   // top-right
			new Vector3(-0.5f, 0.5f, 0)   // top-left
		};

		// Define two triangles
		int[] triangles = new int[]
		{
			0, 2, 1, // first triangle
			0, 3, 2  // second triangle
		};

		// UVs map the full image to the plane
		Vector2[] uvs = new Vector2[]
		{
			new Vector2(0, 0), // bottom-left
			new Vector2(1, 0), // bottom-right
			new Vector2(1, 1), // top-right
			new Vector2(0, 1)  // top-left
		};

		// Assign to mesh
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();

		// Assign mesh to MeshFilter
		MeshFilter mf = GetComponent<MeshFilter>();
		mf.mesh = mesh;

		// Create a simple material with the assigned texture
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Material mat = new Material(Shader.Find("Standard"));
		mr.material = material;
	}
}
