using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder : MonoBehaviour {
    public Vector3 frontRight;
    public Vector3 frontLeft;
    public Vector3 backRight;
    public Vector3 backLeft;


    public MeshCollider mCollider;
    public Mesh mesh;
    private void Start()
    {

        //SetUp();
    }
    public void SetUp()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        mCollider = GetComponent<MeshCollider>();

        // Setting up Vertices
        Vector3[] vertices = new Vector3[]
       {
            backRight,// Back right 0
            frontRight,// Front right 1
            frontLeft, // Front left 2
            backLeft// Back Left 3

       };
        // Connecting the vertices to triangles
        int[] triangles = new int[]
        {
            0,1,2,
            2,3,0
        };
        // Forming the Mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mCollider.sharedMesh = mesh;
        mCollider.enabled = true;
    }
	
	
}
