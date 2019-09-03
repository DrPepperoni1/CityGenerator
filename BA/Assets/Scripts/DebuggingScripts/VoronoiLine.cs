using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiLine : MonoBehaviour
{
    
    LineRenderer lr;
    private void Start()
    {
    }

    public void DrawLine(Vector2 start, Vector2 end, float width)
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, new Vector3(start.x, 1 , start.y));
        lr.SetPosition(1, new Vector3(end.x, 1 , end.y));

    }
}
