using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CircularMeshExtruder : MonoBehaviour
{
    private float radius;
    private int subdivisions;

    public bool drawGizmos;
    public Material material;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        vertices = null;
        meshFilter.mesh = mesh;
        meshRenderer.material = material;
        meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    // void Start()
    // {
    //     meshFilter = GetComponent<MeshFilter>();
    //     meshRenderer = GetComponent<MeshRenderer>();
    //     mesh = new Mesh();
    //     vertices = null;
    //     meshFilter.mesh = mesh;
    //     meshRenderer.material = material;
    //     meshCollider = GetComponent<MeshCollider>();
    //     if (meshCollider != null)
    //     {
    //         meshCollider.sharedMesh = mesh;
    //     }
    // }

    public void GenerateMesh(Vector3[] positions,Vector3[] normals, float _radius, int _subdivisions)
    {
        if (positions.Length < 2) return;
        mesh.Clear();

        radius = _radius;
        subdivisions = _subdivisions;

        vertices = new Vector3[(positions.Length * subdivisions) + 2]; //+2 because we will stich the 2 ends with a single vertice
        triangles = new int[((positions.Length - 1) * subdivisions * 2 * 3) + (subdivisions * 2 * 3)];//subdivision*2 for the end triangles

        //so we will start by counting the angular increment required for creating a circle of given subdivision at each point;

        float angularIncrement = 360f / subdivisions;

        //next for each pair of points, calculate the direction vector, and define

        for (int i = 0; i < vertices.Length - 2; i += subdivisions) //subtracting -2 because we will manually add end vertices later
        {
            //find the normal vector to the direction vector;
            // we will use a dot product to find the perpendicular plane, and then populate that plane with vertices by rotating an arbitrary vector on that plane
            Vector3 directionVector;
            if (i == vertices.Length - subdivisions - 2)
            {
                directionVector = positions[((i) / subdivisions)] - positions[(i) / subdivisions - 1];
            }
            else
            {
                directionVector = positions[(i / subdivisions) + 1] - positions[i / subdivisions];
            }

            //src-> https://sciencing.com/vector-perpendicular-8419773.html
            // float v3 = -(directionVector.x + directionVector.y) / directionVector.z;
            // Vector3 arbitraryPerpendicularVector = new Vector3(v1,v2,v3);

            Vector3 arbitraryPerpendicularVector = normals[(i / subdivisions)];
            // Vector3 arbitraryPerpendicularVector = Vector3.forward;

            for (float j = 0, k = 0; j < 360; j += angularIncrement, k++)
            {
                vertices[i + (int)k] = positions[i / subdivisions] + arbitraryPerpendicularVector.normalized * radius;

                //rotating perpendicular vector by angularIncrement
                arbitraryPerpendicularVector = Quaternion.AngleAxis(angularIncrement, directionVector) * arbitraryPerpendicularVector;
            }
        }

        //lets add the end vertices

        Vector3 dir = positions[0] - positions[1];
        vertices[vertices.Length - 2] = positions[0];// + dir.normalized * radius * 0.5f;

        dir = positions[positions.Length - 1] - positions[positions.Length - 2];
        vertices[vertices.Length - 1] = positions[positions.Length - 1];// + dir.normalized * radius * 0.5f;

        //now lets create faces
        int currentVert = 0;
        int tris = 0;
        for (int j = 0; j < positions.Length - 1; j++)
        {
            for (int i = 0; i < subdivisions; ++i)
            {
                triangles[tris + 0] = currentVert + 0;
                triangles[tris + 1] = currentVert + 1;
                triangles[tris + 2] = currentVert + subdivisions + 1;
                triangles[tris + 3] = currentVert + 0;
                triangles[tris + 4] = currentVert + subdivisions + 1;
                triangles[tris + 5] = currentVert + subdivisions;

                if (i == subdivisions - 1)
                {
                    triangles[tris + 0] = currentVert + 0;
                    triangles[tris + 1] = currentVert + 1 - subdivisions;
                    triangles[tris + 2] = currentVert + 1;
                    triangles[tris + 3] = currentVert + 0;
                    triangles[tris + 4] = currentVert + 1;
                    triangles[tris + 5] = currentVert + subdivisions;
                }

                tris += 6;
                currentVert++;
            }
        }
        currentVert = vertices.Length - 2;

        //now lets cover both the sides

        //we are gonna need first subdivision number of vertices and last subdivision number of vertices
        //front face
        for (int i = 0; i < subdivisions; i++)
        {
            triangles[tris + 0] = currentVert;
            triangles[tris + 1] = i + 1;
            triangles[tris + 2] = i;

            if (i == subdivisions - 1)
            {
                triangles[tris + 0] = currentVert;
                triangles[tris + 1] = 0;
                triangles[tris + 2] = i;
            }
            tris += 3;
        }
        currentVert++;
        // back face
        for (int i = vertices.Length - 2 - subdivisions; i < vertices.Length - 2; i++)
        {
            triangles[tris + 0] = currentVert;
            triangles[tris + 1] = i;
            triangles[tris + 2] = i + 1;

            if (i == vertices.Length - 2 - 1)
            {
                triangles[tris + 0] = currentVert;
                triangles[tris + 1] = i;
                triangles[tris + 2] = i - subdivisions + 1;
            }
            tris += 3;
        }

        UpdateMesh();
        // if(updateCollider){
        //     UpdateCollider();
        // }
    }

    void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // mesh.uv = uv;
        mesh.RecalculateNormals();
        if (meshCollider)
        {
            UpdateCollider();
        }
    }

    void UpdateCollider()
    {
        if (meshCollider == null)
        {
            meshCollider = GetComponent<MeshCollider>();
            if (meshCollider == null)
                meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        meshCollider.sharedMesh = mesh;
    }

    void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            if (vertices != null)
            {
                for (int i = 0; i < vertices.Length; ++i)
                {
                    Gizmos.DrawSphere(vertices[i], 0.05f);
                }
            }
        }
    }
}