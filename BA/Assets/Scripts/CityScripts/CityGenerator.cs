using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GenerationType
{
    Orgnic, Circular,Grid
}
public class CityGenerator : MonoBehaviour
{
    public GenerationType type;

    public GameObject Ground;
    public GameObject IntersectionPF;
    int width, height;
    int numberOfSites;
    public int numberOfLs;

    float groundRad;
    public List<GameObject> lStreets;

    float mainRoadWidth;
    float sideRoadWidth;
    SiteGenerator siteGen;
    VoronoiBuilder vBuilder;


    public GameObject mainRoadPF;
    public GameObject turtlePf;
    public void Generate(int _type, float size, int _numberOfSites, float _mainRoadWidth, float _sideRoadWidth)
    {
        lStreets = new List<GameObject>();
        siteGen = GetComponent<SiteGenerator>();
        vBuilder = GetComponent<VoronoiBuilder>();
        Ground.transform.localScale = new Vector3(size , 0.01f, size);
        groundRad = size / 2f;
        numberOfSites = _numberOfSites;
        mainRoadWidth = _mainRoadWidth;
        sideRoadWidth = _sideRoadWidth;
        type = (GenerationType)_type;


        List<Vector2> sites = new List<Vector2>();
        switch (type)
        {
            case GenerationType.Orgnic:
                sites = siteGen.GenerateRandom(new Vector2(Ground.transform.position.x, Ground.transform.position.z), groundRad, width, height, numberOfSites);
                break;
            case GenerationType.Circular:
                sites = siteGen.GenerateCircular(height, width, groundRad, new Vector2(Ground.transform.position.x,Ground.transform.position.z), 3, numberOfSites);
                break;
            case GenerationType.Grid:
                sites = siteGen.GenerateGrid(new Vector2(Ground.transform.position.x, Ground.transform.position.z), groundRad, 400, 400);
                break;
            default:
                break;
        }



        List<Edge> vEdges = vBuilder.GenerateVoronoi(sites, new Vector2(Ground.transform.position.x, Ground.transform.position.z), groundRad);
        int i = 0;
        for (i = 0; i < vEdges.Count; i++)
        {
            // check to see if the curent edge is entirely out of the worldbound
            if (CheckWholeEdge(vEdges[i]))
            {
                
                continue;
            }
            if (!CheckPositiontoWorldBounds(vEdges[i].start))
            {
                Vector2[] inter = EdgeToCircleIntersection(vEdges[i]);
                

                float dis1 = Vector2.Distance(vEdges[i].start, inter[0]);
                float dis2 = Vector2.Distance(vEdges[i].start, inter[1]);

                if (dis1 < dis2)
                {
                    vEdges[i].start = inter[0];
                }
                else
                {
                    vEdges[i].start = inter[1];
                }
                
            }
            if (!CheckPositiontoWorldBounds(vEdges[i].end))
            {
                Vector2[] inter = EdgeToCircleIntersection(vEdges[i]);
               

                float dis1 = Vector2.Distance(vEdges[i].start, inter[0]);
                float dis2 = Vector2.Distance(vEdges[i].start, inter[1]);

                if (dis1 < dis2)
                {
                    vEdges[i].end = inter[0];
                }
                else
                {
                    vEdges[i].end = inter[1];
                }
            }
            GameObject newMainRoad = Instantiate(mainRoadPF, new Vector3(vEdges[i].start.x, 0.1f, vEdges[i].start.y), Quaternion.identity);
            newMainRoad.GetComponent<MeshBuilderVoronoi>().GenerateMesh(vEdges[i].start, vEdges[i].end, mainRoadWidth);
            newMainRoad.transform.rotation = Quaternion.LookRotation(new Vector3(vEdges[i].ndir.x, 0, vEdges[i].ndir.y));
            newMainRoad.name = vEdges[i].LR;
            newMainRoad.GetComponent<AttachPoints>().CreateAttachPoints(Vector3.Distance(vEdges[i].start, vEdges[i].end), 5);

            Vector2 midPoint = new Vector2((vEdges[i].start.x + vEdges[i].end.x) / 2, (vEdges[i].start.y + vEdges[i].end.y) / 2f);
            Vector2 rotation = Vector2.Perpendicular(vEdges[i].ndir);

            GameObject t = Instantiate(turtlePf, new Vector3(midPoint.x, 0.1f, midPoint.y), Quaternion.LookRotation(new Vector3(rotation.x, 0, rotation.y)));
            TurtleController tc = t.GetComponent<TurtleController>();
            tc.streeWidth = sideRoadWidth;
            tc.startStreet = newMainRoad;
            if (type == GenerationType.Circular)
            {
                float disToMid = Vector2.Distance(midPoint, Ground.transform.position);
                float p = disToMid * 100 / groundRad;
                int itt = 0;
                if (p < 33.3f)
                {
                    itt = 3;
                }
                else if (p < 66.6f)
                {
                    itt = 2;
                }
                else
                {
                    itt = 1;
                }

                tc.Generate(itt); 
            }
            else
            {
                tc.Generate(1);
            }
        }

    }
    private bool CheckWholeEdge(Edge e)
    {
        // return true if start and end of an edge are outside te bounds
        if (!CheckPositiontoWorldBounds(e.start) && !CheckPositiontoWorldBounds(e.end))
        {
            return true;
        }
        return false;
    }
    private bool CheckPositiontoWorldBounds(Vector2 pos)
    {
        // returns true if the position is inside the circle
        if (Vector2.Distance(pos,new Vector2(Ground.transform.position.x,Ground.transform.position.z)) > groundRad)
        {
            return false;
        }
        return true;
    }
    private Vector2 GetRandomPointOnCircle(float r)
    {
        Vector2 point = Random.insideUnitCircle.normalized * r;
        return point;
    }
    private Vector2[] EdgeToCircleIntersection(Edge e)
    {
        Vector2[] intersections = new Vector2[2];

        Vector2 circlePos = new Vector2(Ground.transform.position.x, Ground.transform.position.z);
       
        float m = e.m;
        float b = e.b;
        float m2 = m * m;
        float mb = 2 * m * b;
        float b2 = b * b;
     
        //x^2 + m2 + mb + b2 = groundRad^2
        float groundrad2 = groundRad * groundRad;

        float A = m2 +1;
        float B = mb;
        float C = b2 - groundrad2;

        // Ax^2 + Bx + C = 0

        float det = (B * B) - 4 * A * C;

        float x = (-B + Mathf.Sqrt(det)) / (2 * A);

        float y = m * x + b;

        intersections[0] = new Vector2(x, y);

        x = (-B - Mathf.Sqrt(det)) / (2 * A);

        y = m * x + b;

        intersections[1] = new Vector2(x, y);


        return intersections;
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
