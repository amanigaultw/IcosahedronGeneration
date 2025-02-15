using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrawIcosahedron : MonoBehaviour
{
    [SerializeField]
    Material material;

    [SerializeField]
    int subdivisionCount = 0;

    [SerializeField]
    bool showVertices = true;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Triangle> triangles = new List<Triangle>();

    void Start()
    {
        initializeVertices();
        initializeTriangles();
        draw();
    }

    private void draw()
    {
        for (int i = 0; i < subdivisionCount; i++)
        {
            subdivide();
        }

        drawFaces();
        
        if (showVertices)
        {
            drawVertices(vertices);
        }

        gameObject.transform.localScale = Vector3.one * 4f * (subdivisionCount + 1f);
        gameObject.transform.position = new Vector3(0, 0, (subdivisionCount + 1f) * 10f);
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
        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle triangle = triangles[i];
            GameObject gameObject = triangle.Instantiate(material);
            gameObject.name = "Triangle " + i;
            gameObject.transform.parent = this.gameObject.transform;
        }
    }

    private void drawVertices(List<Vector3> vertices)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObject.name = "vertice " + i;
            gameObject.transform.localScale = Vector3.one / (10f * (subdivisionCount + 1));
            gameObject.transform.position = vertices[i];
            gameObject.transform.parent = this.gameObject.transform;
        }
    }

}
