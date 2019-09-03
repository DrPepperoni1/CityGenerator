using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeData : MonoBehaviour
{
    public float m;
    public float b;
    public Vector2 start;
    public Vector2 direction;
    public Vector2 controllPoint;
    public string LR;

    public void FillData(float _m, float _b, Vector2 _start, Vector2 _direction, Vector2 _controllPoint , string _LR)
    {
        m = _m;
        b = _b;
        start = _start;
        direction = _direction;
        controllPoint = _controllPoint;
        LR = _LR;
        if (m == 0 && b == 0 && start == new Vector2(0, 0)) 
        {
            Debug.Log("strange line");
        }
    }
}
