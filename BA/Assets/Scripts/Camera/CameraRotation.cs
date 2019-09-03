using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {

	[SerializeField]
    private float horizontalRotationSpeed;

    [SerializeField]
    private float verticleRotationSpeed;

    [SerializeField]
    private Transform cam;
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(0, 1, 0) * horizontalRotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
       
            cam.Rotate(new Vector3(-1, 0, 0) * verticleRotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
        }
    }
}
