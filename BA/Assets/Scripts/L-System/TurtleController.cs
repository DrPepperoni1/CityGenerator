using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TurtleController : MonoBehaviour {

    private Vector3 savePoint;
    private Quaternion saveRot;
    public int streetLenght;
    [SerializeField]
    private GameObject street;
    public GameObject worldBound;
    [SerializeField]
    private GameObject graphix;
    [SerializeField]
    private PlayableDirector director;
    public bool animating;
    StringInterpreter interpreter;
    Vector3 otherSide;
    public bool start = false;
    public GameObject startStreet;
    public GameObject lastSpawned;
    public GameObject colliding;
    public BoxCollider boxCollider;
    public float streeWidth;
    char nextTask;
    public LayerMask lm;
    public QueryTriggerInteraction qerry;
    public List<TurtleSave> saveStack = new List<TurtleSave>();
	void Start ()
    {
       
	}  
    
    public void Generate(int itterations)
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(1, 1, streetLenght);
        boxCollider.center = new Vector3(0, 0, streetLenght / 2 + 1);
        interpreter = GetComponent<StringInterpreter>();
        worldBound = GameObject.FindGameObjectWithTag("WorldBound");
        otherSide = transform.position + (worldBound.transform.localScale.x * transform.forward);
        interpreter.GenerateString(itterations);
        start = true;
    }

	void Update ()
    {
        if (start)
        {

            


                nextTask = interpreter.NextTask();

                if (nextTask != ' ')
                {
                    if (nextTask == 'F')
                    {
                        MoveForward();
                    }
                    else if (nextTask == '+')
                    {

                        TurnRight(90f);
                    }
                    else if (nextTask == '-')
                    {

                        TurnLeft(90f);
                    }
                    else if (nextTask == '[')
                    {

                        SaveLocation();
                    }
                    else if (nextTask == ']')
                    {

                        LoadLocation();
                    }
                }

            
        }
        

        

	}
    private void spawnStreet()
    {
        
        GameObject newStreet;
        if (checkWorldBounds(transform.position) )
        {
            GameObject potCollision = CheckNextStreet();
            if (potCollision == null)
            {
                newStreet = Instantiate(street, transform.position, transform.rotation);
                lastSpawned = newStreet;
                newStreet.name = "no cillision";
                MeshBuilder mb = newStreet.GetComponent<MeshBuilder>();
                mb.frontRight = new Vector3(streeWidth / 2f, 0, 0);
                mb.frontLeft = new Vector3(-streeWidth / 2f, 0, 0);
                mb.backRight = new Vector3(streeWidth / 2f, 0, streetLenght);
                mb.backLeft = new Vector3(-streeWidth / 2f, 0, streetLenght);
                mb.SetUp();
                CheckNextStreet();
                AttachPoints ap = newStreet.GetComponent<AttachPoints>();
                
                ap.CreateAttachPoints(streetLenght, 5);


            }
            else
            {

                AttachPoints ap =  potCollision.GetComponent<AttachPoints>();
                GameObject closestAp = ap.FindClosest(this.gameObject);
                newStreet = Instantiate(street, transform.position, Quaternion.identity);
                newStreet.name = "Collision";
                newStreet.transform.LookAt(closestAp.transform.position);
                lastSpawned = newStreet;
                MeshBuilder mb = newStreet.GetComponent<MeshBuilder>();
                float dis = Vector3.Distance(closestAp.transform.position, newStreet.transform.position);

                mb.frontLeft = new Vector3(-streeWidth /2f, 0, 0);
                mb.frontRight = new Vector3(streeWidth/ 2, 0, 0);
                mb.backLeft = new Vector3(-streeWidth, 0, dis);
                mb.backRight = new Vector3(streeWidth, 0, dis);
                mb.SetUp();
                start = false;

                AttachPoints thisAp = newStreet.GetComponent<AttachPoints>();
                thisAp.CreateAttachPoints(streetLenght,5);
                Destroy(this.gameObject);

                

            }

        }
        
    }
    private GameObject CheckNextStreet()
    {
        
        
        Collider[] col = Physics.OverlapBox(transform.position + transform.forward * boxCollider.center.z,boxCollider.size/2, Quaternion.identity,lm);
        foreach (var c in col)
        {

            if (c.gameObject != startStreet && c.gameObject != lastSpawned)
            {
                colliding = c.gameObject;
                

                return c.gameObject;

            }
                
        }
        
            return null;
    }
   
    private bool checkWorldBounds(Vector3 obj)
    {
        float rad = worldBound.transform.localScale.x / 2;
        float d = Vector3.Distance(worldBound.transform.position, obj);
        if (d <= rad)
        {
            return true;
        }
        else if (true)
        {
            return false;
        }
        

        
        
    }    
    public void MoveForward()
    {
       
        spawnStreet();
        transform.position += transform.forward * streetLenght;
        //director.Play();
        //Debug.Log("Moving forward");
    }
    public void TurnLeft(float _angle)
    {
        float angle = Random.Range(-_angle, 0);
        transform.Rotate(transform.up, -_angle);

    }
    public void TurnRight(float _angle)
    {
        float angle = Random.Range(0, _angle);
        transform.Rotate(transform.up, _angle);

    }
    public void SaveLocation()
    {

        TurtleSave save = new TurtleSave(transform.position, transform.rotation);
        saveStack.Add(save);


    }
    public void LoadLocation()
    {

        TurtleSave save = saveStack[saveStack.Count-1];
        
        saveStack.Remove(save);
        transform.position = save.location;
        transform.rotation = save.rotation;


    }

    
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
    }
}
