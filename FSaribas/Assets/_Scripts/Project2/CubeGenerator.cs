using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeGenerator : MonoBehaviour
{
    public Material cubeMaterial; // Assign the material in the inspector

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateCubeMesh();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = cubeMaterial; // Set the material
    }

    Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh
        {
            vertices = new Vector3[]
            {
                // Front face
                new Vector3(-0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f, -0.5f,  0.5f),
                new Vector3( 0.5f,  0.5f,  0.5f),
                new Vector3(-0.5f,  0.5f,  0.5f),
                // Back face
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f, -0.5f, -0.5f),
                new Vector3( 0.5f,  0.5f, -0.5f),
                new Vector3(-0.5f,  0.5f, -0.5f)
            },
            triangles = new int[]
            {
                // Front face
                0, 2, 1,
                0, 3, 2,
                // Back face
                4, 5, 6,
                4, 6, 7,
                // Left face
                0, 7, 3,
                0, 4, 7,
                // Right face
                1, 2, 6,
                1, 6, 5,
                // Top face
                3, 7, 6,
                3, 6, 2,
                // Bottom face
                0, 1, 5,
                0, 5, 4
            },
            uv = new Vector2[]
            {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Front face
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Back face
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Left face
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Right face
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), // Top face
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)  // Bottom face
            }
        };

        mesh.RecalculateNormals();

        return mesh;
    }
}
