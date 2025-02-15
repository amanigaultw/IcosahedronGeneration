using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{

    private Vector3[] vertices = new Vector3[3];

    public Triangle (Vector3[] vertices)
    {
        this.vertices = vertices;
    }

    public Vector3[] getVertices()
    {
        return vertices;
    }

    public GameObject Instantiate(Material material)
    {
        GameObject gameObject = new GameObject();

        gameObject.AddComponent<MeshRenderer>();
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = material;

        gameObject.AddComponent<MeshFilter>();
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.triangles = new int[] { 0, 1, 2 };

        return gameObject;
    }

    public List<Triangle> subdivide()
    {
        List<Triangle> triangles = new List<Triangle>();
        Vector3[] midPoints = getMidPoints();

        triangles.Add(new Triangle(new Vector3[] { vertices[0], midPoints[0], midPoints[1] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[1], midPoints[2], midPoints[0] }));
        triangles.Add(new Triangle(new Vector3[] { vertices[2], midPoints[1], midPoints[2] }));
        triangles.Add(new Triangle(new Vector3[] { midPoints[0], midPoints[2], midPoints[1] }));

        return triangles;
    }

    public Vector3[] getMidPoints()
    {
        Vector3[] midPoints = new Vector3[3];

        midPoints[0] = ((vertices[0] + vertices[1]) / 2).normalized;
        midPoints[1] = ((vertices[0] + vertices[2]) / 2).normalized;
        midPoints[2] = ((vertices[1] + vertices[2]) / 2).normalized;

        return midPoints;
    }
}
