using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeRadius : MonoBehaviour
{
    public float distance = 10.0f;
    public float angle = 30.0f;
    public float height = 1.0f;
    public Color meshColor = Color.red;

    Mesh mesh;
    Mesh CreateConeMesh()
    {
        Mesh mesh = new Mesh();

        int nofTriangles = 8;
        int nofVertices = nofTriangles * 3;

        Vector3[] vertices = new Vector3[nofVertices];
        int[] triangles = new int[nofVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        // left side 
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        // far side
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft
            ;
        vertices[vert++] = bottomLeft;

        // top
        vertices[vert++] = topLeft;
        vertices[vert++] = topRight;
        vertices[vert++] = topCenter;

        // bottom
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        for(int i=0; i<nofVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateConeMesh();
    }

    private void OnDrawGizmos()
    {
        if(mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }
}
