using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Icosahedron : MonoBehaviour
{
    [SerializeField, Range(0, 5)] // too many triangles for a single mesh past 5 subdivisions
    int subdivisionCount = 0;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Triangle> triangles = new List<Triangle>();

    void Start()
    {
        initializeVertices();
        initializeTriangles();
        for (int i = 0; i < subdivisionCount; i++)
        {
            subdivide();
        }
        drawFaces();
    }

    private void initializeVertices()
    {
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f; // ~1.618034

        vertices.Add(new Vector3(-1, t, 0).normalized);
        vertices.Add(new Vector3(1, t, 0).normalized);
        vertices.Add(new Vector3(-1, -t, 0).normalized);
        vertices.Add(new Vector3(1, -t, 0).normalized);
        vertices.Add(new Vector3(0, -1, t).normalized);
        vertices.Add(new Vector3(0, 1, t).normalized);
        vertices.Add(new Vector3(0, -1, -t).normalized);
        vertices.Add(new Vector3(0, 1, -t).normalized);
        vertices.Add(new Vector3(t, 0, -1).normalized);
        vertices.Add(new Vector3(t, 0, 1).normalized);
        vertices.Add(new Vector3(-t, 0, -1).normalized);
        vertices.Add(new Vector3(-t, 0, 1).normalized);
    }

    private void initializeTriangles()
    {
        triangles.Add(new Triangle(new Vector3[] { vertices[0],  vertices[11], vertices[5] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[0],  vertices[5],  vertices[1] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[0],  vertices[1],  vertices[7] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[0],  vertices[7],  vertices[10] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[0],  vertices[10], vertices[11] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[1],  vertices[5],  vertices[9] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[5],  vertices[11], vertices[4] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[11], vertices[10], vertices[2] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[10], vertices[7],  vertices[6] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[7],  vertices[1],  vertices[8] }));

        triangles.Add(new Triangle(new Vector3[] { vertices[3], vertices[9], vertices[4] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[3], vertices[4], vertices[2] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[3], vertices[2], vertices[6] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[3], vertices[6], vertices[8] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[3], vertices[8], vertices[9] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[4], vertices[9], vertices[5] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[2], vertices[4], vertices[11] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[6], vertices[2], vertices[10] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[8], vertices[6], vertices[7] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[9], vertices[8], vertices[1] }));
    }

    private void subdivide()
    {
        List<Vector3> newVertices = new List<Vector3>();
        List<Triangle> newTriangles = new List<Triangle>();

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];

            List<Triangle> subTriangles = triangle.subdivide();
            List<Vector3> subVertices = triangle.getMidPoints().ToList();

            newTriangles = newTriangles.Concat(subTriangles).ToList();
            newVertices = newVertices.Concat(subVertices).ToList();
        }

        triangles = newTriangles;
        vertices = vertices.Concat(newVertices).ToList();
    }
    private void drawFaces()
    {
        List<int> triangleIndices = new List<int>();

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];
            Vector3Int indices = triangle.getIndices(vertices);
            triangleIndices.Add(indices[0]);
            triangleIndices.Add(indices[1]);
            triangleIndices.Add(indices[2]);
        }

        Vector3[] newVertices = vertices.ToArray();
        int[] newTriangles = triangleIndices.ToArray();

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
        mesh.triangles = newTriangles;
    }

}
