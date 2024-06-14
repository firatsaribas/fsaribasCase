using UnityEngine;
using System.Collections.Generic;

public class MeshSplitter : MonoBehaviour
{
    public MeshFilter meshFilter; // Assign the MeshFilter in the inspector
    public float xFraction = 0.5f; // Fraction along the X axis to split the mesh

    void Start()
    {
        Mesh originalMesh = meshFilter.mesh;
        List<Mesh> splitMeshes = SplitMeshAlongXFraction(originalMesh, xFraction);

        if (splitMeshes.Count == 2)
        {
            // Create two new GameObjects to hold the split meshes
            CreateMeshObject("Left Mesh", splitMeshes[0]);
            CreateMeshObject("Right Mesh", splitMeshes[1]);
        }
    }

    List<Mesh> SplitMeshAlongXFraction(Mesh originalMesh, float fraction)
    {
        List<Vector3> leftVertices = new List<Vector3>();
        List<Vector3> rightVertices = new List<Vector3>();
        List<int> leftTriangles = new List<int>();
        List<int> rightTriangles = new List<int>();
        List<Vector3> boundaryVertices = new List<Vector3>();

        Vector3[] vertices = originalMesh.vertices;
        int[] triangles = originalMesh.triangles;

        float splitX = Mathf.Lerp(GetMinX(vertices), GetMaxX(vertices), fraction);

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            bool v0Left = v0.x <= splitX;
            bool v1Left = v1.x <= splitX;
            bool v2Left = v2.x <= splitX;

            if (v0Left && v1Left && v2Left)
            {
                AddTriangle(leftVertices, leftTriangles, v0, v1, v2);
            }
            else if (!v0Left && !v1Left && !v2Left)
            {
                AddTriangle(rightVertices, rightTriangles, v0, v1, v2);
            }
            else
            {
                // Handle split triangles
                List<Vector3> left, right;
                SplitTriangle(v0, v1, v2, v0Left, v1Left, v2Left, splitX, out left, out right, boundaryVertices);
                AddTriangles(leftVertices, leftTriangles, left);
                AddTriangles(rightVertices, rightTriangles, right);
            }
        }

        Mesh leftMesh = CreateMesh(leftVertices, leftTriangles);
        Mesh rightMesh = CreateMesh(rightVertices, rightTriangles);

        CloseMesh(leftMesh, boundaryVertices, true);
        CloseMesh(rightMesh, boundaryVertices, false);

        return new List<Mesh> { leftMesh, rightMesh };
    }

    float GetMinX(Vector3[] vertices)
    {
        float minX = float.MaxValue;
        foreach (Vector3 vertex in vertices)
        {
            if (vertex.x < minX)
            {
                minX = vertex.x;
            }
        }
        return minX;
    }

    float GetMaxX(Vector3[] vertices)
    {
        float maxX = float.MinValue;
        foreach (Vector3 vertex in vertices)
        {
            if (vertex.x > maxX)
            {
                maxX = vertex.x;
            }
        }
        return maxX;
    }

    void SplitTriangle(Vector3 v0, Vector3 v1, Vector3 v2, bool v0Left, bool v1Left, bool v2Left, float splitX, out List<Vector3> left, out List<Vector3> right, List<Vector3> boundaryVertices)
    {
        left = new List<Vector3>();
        right = new List<Vector3>();

        Vector3 left0, left1, right0, right1;
        if (v0Left)
        {
            if (v1Left)
            {
                left0 = v0; left1 = v1; right0 = v2; right1 = v2;
            }
            else if (v2Left)
            {
                left0 = v0; left1 = v2; right0 = v1; right1 = v1;
            }
            else
            {
                left0 = v0; left1 = v0; right0 = v1; right1 = v2;
            }
        }
        else
        {
            if (v1Left)
            {
                if (v2Left)
                {
                    left0 = v1; left1 = v2; right0 = v0; right1 = v0;
                }
                else
                {
                    left0 = v1; left1 = v1; right0 = v0; right1 = v2;
                }
            }
            else
            {
                left0 = v2; left1 = v2; right0 = v0; right1 = v1;
            }
        }

        Vector3 intersection1 = IntersectXPlane(left0, right0, splitX);
        Vector3 intersection2 = IntersectXPlane(left1, right1, splitX);

        boundaryVertices.Add(intersection1);
        boundaryVertices.Add(intersection2);

        if (v0Left)
        {
            left.Add(v0); left.Add(intersection1); left.Add(intersection2);
            right.Add(intersection1); right.Add(v1); right.Add(v2);
            right.Add(intersection2); right.Add(intersection1); right.Add(v2);
        }
        else
        {
            right.Add(v0); right.Add(intersection1); right.Add(intersection2);
            left.Add(intersection1); left.Add(v1); left.Add(v2);
            left.Add(intersection2); left.Add(intersection1); left.Add(v2);
        }
    }

    Vector3 IntersectXPlane(Vector3 left, Vector3 right, float splitX)
    {
        float t = (splitX - left.x) / (right.x - left.x);
        return Vector3.Lerp(left, right, t);
    }

    void AddTriangle(List<Vector3> vertices, List<int> triangles, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        int index = vertices.Count;
        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);
        triangles.Add(index);
        triangles.Add(index + 1);
        triangles.Add(index + 2);
    }

    void AddTriangles(List<Vector3> vertices, List<int> triangles, List<Vector3> triangleVertices)
    {
        for (int i = 0; i < triangleVertices.Count; i += 3)
        {
            AddTriangle(vertices, triangles, triangleVertices[i], triangleVertices[i + 1], triangleVertices[i + 2]);
        }
    }

    Mesh CreateMesh(List<Vector3> vertices, List<int> triangles)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    void CreateMeshObject(string name, Mesh mesh)
    {
        GameObject newObj = new GameObject(name);
        newObj.AddComponent<MeshFilter>().mesh = mesh;
        newObj.AddComponent<MeshRenderer>().material = meshFilter.GetComponent<MeshRenderer>().material;
    }

    void CloseMesh(Mesh mesh, List<Vector3> boundaryVertices, bool isLeft)
    {
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);

        if (isLeft)
        {
            boundaryVertices.Reverse();
        }

        int baseIndex = vertices.Count;
        vertices.AddRange(boundaryVertices);

        // Create a fan of triangles to close the mesh along the boundary
        for (int i = 0; i < boundaryVertices.Count - 1; i++)
        {
            triangles.Add(baseIndex);
            triangles.Add(baseIndex + i);
            triangles.Add(baseIndex + i + 1);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
