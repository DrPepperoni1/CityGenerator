using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilderVoronoi : MonoBehaviour {

    public Mesh mesh;
    public MeshCollider mCollider;
	public void GenerateMesh(Vector2 start, Vector2 end, float width)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        mCollider = GetComponent<MeshCollider>();
        float halfWidth = width / 2;

        float disToEnd = Vector2.Distance(start, end);

        Vector3 startRight = new Vector3( halfWidth, 0, 0);
        Vector3 startLeft = new Vector3(-halfWidth, 0, 0);

        Vector3 endRight = new Vector3( halfWidth, 0, disToEnd);
        
        Vector3 endLeft = new Vector3(-halfWidth,0, disToEnd);

        Vector3[] vertecies = new Vector3[]
        {
            startRight,
            endRight,
            endLeft,
            startLeft
        };
        int[] triangles = new int[]
        {
            0,3,1,
            3,2,1
        };
        mesh.Clear();
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mCollider.sharedMesh = mesh;
        
    }
}
