using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TurtleSave  {
    public Vector3 location;
    public Quaternion rotation;
    public TurtleSave(Vector3 _location, Quaternion _rotation)
    {
        location = _location;
        rotation = _rotation;
    }
	
}
