using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is the procedural mesh for the water
/// </summary>
public class Water : MonoBehaviour {

    /// <summary>
    /// The number of quads on the width of the water mesh
    /// </summary>
    [SerializeField]
    public int widthQuads = 0;

    /// <summary>
    /// The number of quads on the height of the water mesh
    /// </summary>
    [SerializeField]
    public int heightQuads = 0;

    /// <summary>
    /// The size of the quads on the water mesh
    /// </summary>
    [SerializeField]
    public float quadSize = 0;

    /// <summary>
    /// The filter to render the generated mesh
    /// </summary>
    private MeshFilter meshFilter;

    /// <summary>
    /// The vertex grid of the generated mesh
    /// </summary>
    private Vector3[] vertices;

    /// <summary>
    /// The center point of the generated mesh
    /// </summary>
    private Vector3 meshCenter;
    public Vector3 MeshCenter
    {
        get
        {
            return meshCenter;
        }
    }

    /// <summary>
    /// All the triangles of the generated mesh
    /// </summary>
    private int[] triangles;

    /// <summary>
    /// All the normals of the generated mesh
    /// </summary>
    private Vector3[] normals;

    /// <summary>
    /// The mesh contrainer for the arrays of vertices, triangles, and normals
    /// </summary>
    private Mesh mesh;

    private void Awake()
    {
        meshFilter = this.GetComponent<MeshFilter>();

        if(meshFilter == null)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
            Debug.LogWarning("No Mesh Filter On: " + this.gameObject.name + " Adding One");
        }

    }

    // Use this for initialization
    void Start () {

        mesh = new Mesh();

        meshCenter = new Vector3((widthQuads / 2) * quadSize, 0, (heightQuads / 2) * quadSize);

        //Create the arrays and lay the vertices down in a grid

        vertices = new Vector3[widthQuads * heightQuads];
        normals = new Vector3[widthQuads * heightQuads];

        List<int> TrisList = new List<int>();

        for (int y = 0; y < heightQuads; y++)
        {
            for(int x = 0; x < widthQuads; x++)
            {
                vertices[x + y * widthQuads] = new Vector3(x * quadSize, 0, y * quadSize) - meshCenter;
            }
        }

        //Create the triangles

        for(int y = 0; y < heightQuads - 1; y++)
        {
            for(int x = 0; x < widthQuads - 1; x++)
            {
                TrisList.Add(x + y * widthQuads + widthQuads);
                TrisList.Add(x + y * widthQuads + 1);
                TrisList.Add(x + y * widthQuads);

                TrisList.Add(x + y * widthQuads + 1);
                TrisList.Add(x + y * widthQuads + widthQuads);
                TrisList.Add(x + y * widthQuads + widthQuads + 1);
                
            }
        }

        triangles = TrisList.ToArray();

        //Set the normals upward
        for(int x = 0; x < vertices.Length; x++)
        {
            normals[x] = Vector3.up;
        }

        //update the mesh

        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.normals = normals;

        meshFilter.mesh = mesh;

	}
	
	// Update is called once per frame
	void Update () {

        mesh.vertices = vertices;

        mesh.triangles = triangles;

        mesh.normals = normals;
        
        meshFilter.mesh = mesh;

    }

    /// <summary>
    /// Sets the vertex at a specific x, y location on the vertex grid to height
    /// </summary>
    /// <param name="xLocation">The x vertex location</param>
    /// <param name="yLocation">The y vertex location</param>
    /// <param name="height">The height the vertice is to be set to</param>
    private void SetVertexHeight(int xLocation, int yLocation, float height)
    {
        if(xLocation >= 0 && xLocation < widthQuads &&
            yLocation >= 0 && yLocation < heightQuads)
        {
            vertices[xLocation + yLocation * widthQuads].y = height;
        }
    }

    /// <summary>
    /// Based on Bresenhams Circle Drawing Algorithm
    /// Used to make the waves in the water mesh
    /// Sets a circle whose center is xCenter, yCenter of vertices to a height
    /// </summary>
    /// <param name="xCenter">The x location of the center of the circle on the vertex grid</param>
    /// <param name="yCenter">The y location of the center of the circle on the vertex grid</param>
    /// <param name="radius">The radius of the circle to be adjusted</param>
    /// <param name="height">The new height of the vertices</param>
    public void LayCircle(int xCenter, int yCenter, int radius, float height)
    {
        int x = radius - 1;
        int y = 0;
        int dx = 1;
        int dy = 1;
        int err = dx - (radius << 1);

        while(x >= y)
        {
            SetVertexHeight(xCenter + x, yCenter + y, height);
            SetVertexHeight(xCenter + y, yCenter + x, height);
            SetVertexHeight(xCenter - y, yCenter + x, height);
            SetVertexHeight(xCenter - x, yCenter + y, height);
            SetVertexHeight(xCenter - x, yCenter - y, height);
            SetVertexHeight(xCenter - y, yCenter - x, height);
            SetVertexHeight(xCenter + y, yCenter - x, height);
            SetVertexHeight(xCenter + x, yCenter - y, height);

            if(err <= 0)
            {
                y++;
                err += dy;
                dy += 2;
            }
            else
            {
                x--;
                dx += 2;
                err += dx - (radius << 1);
            }

        }

    }

    /// <summary>
    /// Gets the current height of a vertex on the vertex grid
    /// </summary>
    /// <param name="xLocation">The x location to be read</param>
    /// <param name="yLocation">The y location to be read</param>
    /// <returns>The height of the vertex, otherwise 0</returns>
    public float GetHeightAtPosition(int xLocation, int yLocation)
    {
        if (xLocation >= 0 && xLocation < widthQuads &&
            yLocation >= 0 && yLocation < heightQuads)
        {
            return vertices[xLocation + yLocation * widthQuads].y;
        }

        return 0;
    }

    /// <summary>
    /// Flattens the entire vertex grid back to 0
    /// </summary>
    public void FlattenMesh()
    {
        for(int x = 0; x < vertices.Length; x++)
        {
            vertices[x].y = 0.0f;
        }
    }

}
