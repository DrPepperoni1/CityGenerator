using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    // Pan Movement
    [SerializeField]
    private float panSpeed = 20f;
    [SerializeField]
    private float panBorder = 10f;
    //Scrolling
    [SerializeField]
    private float minY = 20;
    [SerializeField]
    private float maxY = 120;
    [SerializeField]
    private float scrollSpeed = 100;
    
   
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w") /*|| Input.mousePosition.y >= Screen.height - panBorder*/)
        {
            pos += transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") /*|| Input.mousePosition.y <= panBorder*/)
        {
            pos -= transform.forward * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") /*|| Input.mousePosition.x >= Screen.width - panBorder*/)
        {
            pos += transform.right * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") /*|| Input.mousePosition.x <=  panBorder*/)
        {
            pos -= transform.right * panSpeed * Time.deltaTime;
        }

        

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);



        transform.position = pos;
    }
}
