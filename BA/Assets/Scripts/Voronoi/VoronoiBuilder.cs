using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoronoiBuilder : MonoBehaviour
{
    public GameObject VoronoiLinePF;
    public int numberOfSites;
    public int width, height;
    List<Vector2> sites;
    
    public float sweepLine;
    public List<Event> queue;
    public Node firstInBeach;
    public GameObject sitePF;

    public List<Edge> edges;
    
    
   
    

    void Start()
    {

        

    }
    
    public List<Edge> GenerateVoronoi(List<Vector2> _sites, Vector2 groundPos, float r)
    {

        edges = new List<Edge>();
        sites = _sites;

        queue = new List<Event>();
        

        foreach (var site in sites)
        {
            //Instantiate(sitePF, new Vector3(site.x, 1, site.y), Quaternion.identity);
            InsertEventToQueue(new Event(new Vector2(site.x, site.y), false, "original queue"));
        }
        firstInBeach = new Node(queue[0].position,false, "firstInBeach");
        
        
        
        queue.RemoveAt(0);
        sweepLine = firstInBeach.position.y;
        
        Node n = firstInBeach;
        while (queue.Count > 0)
        {
            Event e = queue[0];
            sweepLine = e.position.y;

            


            if (e.type == false)
            {
                AddToBeachLine(e);

            }
            else
            {
                RemoveFromBeachLine(e);

            }
            
            
            queue.RemoveAt(0);
        }
        FinishRemainingEdges(r,groundPos);
        //DrawVoronoi();

        return edges;
        
    }
    

    
    void DrawEdgeLine(Node n)
    {
        Color c = Color.green;
        //n.lineRenderer.startColor = c;
        //n.lineRenderer.endColor = c;
        n.lineRenderer.material.color = c;
        n.lineRenderer.positionCount = 2;
        float endX = getXOfEdge(n);
        float endY = GetY(n.previous.position, new Vector2(endX, 0f));
        if (!float.IsNaN(endX) && !float.IsNaN(endY))
        {

            n.lineRenderer.SetPosition(1, new Vector3(endX, 2, endY));
        }
        
        else
        {
            n.lineRenderer.SetPosition(0, new Vector3(n.edge.start.x,2, n.edge.start.y));
            n.lineRenderer.SetPosition(1, new Vector3(n.edge.start.x + n.edge.dir.x * 100, 2, n.edge.start.y + n.edge.dir.y * 100));
            n.lineRenderer.transform.position = new Vector3(n.edge.start.x, 1, n.edge.start.y);
        }
    }
    void FinishRemainingEdges(float groundR, Vector2 groundPos)
    {
        // Finishes all unfinished edges
        Node node = firstInBeach;
        while (node != null)
        {
            if (node.type == true)
            {
                // find point of circle in the direction of the edge
                Edge e = node.edge;
                e.end = e.start;
                float disToCenter = Vector2.Distance(groundPos, e.end);
                e.end = e.start + e.ndir * groundR /10;
                edges.Add(e);
                //if (disToCenter < groundR)
                //{
                //    // inside 
                //}
                //else if(disToCenter > groundR)
                //{
                //    //outside
                //}
                //else
                //{
                //    //perfect
                //}
                //while (true)
                //{

                //}
            }
            node = node.next;
        }

    }
    void ResetEdgesOutsideOfBounds()
    {
        //resets start or end of an edge that is outside of the ground to be right on the edge
    }
    private void DrawVoronoi()
    {
        
        Debug.Log("drawing voronoi");
        int i = 0;
        for (i = 0; i < edges.Count; i++)
        {
            GameObject voronoiLine = Instantiate(VoronoiLinePF, new Vector3(edges[i].start.x, 1,edges[i].start.y),Quaternion.identity);
            voronoiLine.GetComponent<VoronoiLine>().DrawLine(edges[i].start, edges[i].end, .5f);
            voronoiLine.GetComponent<EdgeData>().FillData(edges[i].m, edges[i].b, edges[i].start, edges[i].dir, edges[i].controllPoint, edges[i].LR);
        }
        Debug.Log("edges at draw " + i);
    }
    private void AddToBeachLine(Event e)
    {
        
        Node newNode = new Node(e.position, false, "add to beach new node");
        
       
        
      
        Node above = FindNodeOverPoint(e.position);
        
        
        Vector2 start = new Vector2(e.position.x, GetY(above.position, e.position));
        
        Edge leftEdge = new Edge(start, e.position, above.position, "left");
        Edge rightEdge = new Edge(start, above.position, e.position, "right" );

        leftEdge.adjacent = rightEdge;
        rightEdge.adjacent = leftEdge;

        Node leftNode = new Node(leftEdge, true, "add to beach left edge");
        
        
        Node rightNode = new Node(rightEdge, true,"add to beach right edge");
        

        Node duplicate = new Node(above.position,false, "duplicate node");
        
        duplicate.SetNext(above.next);
        above.SetNext(leftNode);
        leftNode.SetNext(newNode);
        newNode.SetNext(rightNode);
        rightNode.SetNext(duplicate);

        if (above.cirlce != null)
        {
            queue.Remove(above.cirlce);
            above.cirlce = null;
        }

        CircleCheck(above);
        CircleCheck(duplicate);
    }
    private void RemoveFromBeachLine(Event e)
    {
        
        Edge leftEdge = e.node.previous != null ? e.node.previous.edge : null;
        Edge rightEdge = e.node.next != null ? e.node.next.edge : null;

        Vector2? intersection = GetEdgeIntersectionPoint(leftEdge, rightEdge);

        if (intersection == null)
        {
            return;
        }
        
        leftEdge.end = intersection.Value;
        rightEdge.end = intersection.Value;

        edges.Add(leftEdge);
        
        edges.Add(rightEdge);

        Node leftSite = e.node.previous.previous;
        Node rightSite = e.node.next.next;
        
        if (leftSite != null && rightSite != null)
        {
            

            Node newNode = new Node(new Edge(intersection.Value, rightSite.position, leftSite.position, "none"), true, "remove from beach");          
            newNode.SetNext(rightSite);
            leftSite.SetNext(newNode);

            if (leftSite.cirlce != null)
            {
                queue.Remove(leftSite.cirlce);
                leftSite.cirlce = null;
            }
            if (rightSite.cirlce != null)
            {
                queue.Remove(rightSite.cirlce);
                rightSite.cirlce = null;
            }

            CircleCheck(newNode.previous);
            CircleCheck(newNode.next);
        }
    }

    private void CircleCheck(Node n)
    {

        Edge left = n.previous != null ? n.previous.edge : null;
        Edge right = n.next != null ? n.next.edge : null;

        Vector2? intersection = GetEdgeIntersectionPoint(left, right);

        if (intersection == null)
        {
            return;
        }
        float rad = Vector2.Distance(n.position, intersection.Value); // calculating the radius for the circlecheck


        // if the sweepLine has already passed the bottom of the circle
        if (intersection.Value.y + rad < sweepLine)
        {
            return;
        }
        if (float.IsNaN(intersection.Value.x)    || float.IsNaN(intersection.Value.y))
        {
            Debug.LogError("intersection at infinity while circlecheck");
        }

        Event circleEvent = new Event(new Vector2(intersection.Value.x, intersection.Value.y + rad), true, n,"circle Check" );
        n.cirlce = circleEvent;
        InsertEventToQueue(circleEvent);

    }


    // returns a vector2 of an intersection point of two edges
    private Vector2? GetEdgeIntersectionPoint(Edge left, Edge right)
    {
        if (left == null || right == null)
        {
            return null;
        }
        
        if (left.m == right.m) // in this case the edges are paralell
        {
            Debug.Log("the left and right edge are parallel");
            return null;
        }
        if (left.adjacent == right)
        {
            Debug.Log("left and right were connected");
        }
        float x = 0;
        float y = 0;
        if (float.IsInfinity(left.m) || float.IsNaN(left.m))
        {
            x = left.b;
            y = right.m * x + right.b;
        }
        else if (float.IsInfinity(right.m)||float.IsNaN(right.m))
        {
            x = right.b;
            y = left.m * x + left.b;
        }
        else
        {
            x = (right.b - left.b) / (left.m - right.m);
            y = left.m * x + left.b;

        }
        

        //// this are rays instead of lines. it has to be checked if the intersectionpoint is in the direction of the two edges
        if (((x - left.start.x) / left.dir.x) < 0)
        {

            return null;
        }
        if (((y - left.start.y) / left.dir.y) < 0)
        {

            return null;
        }

        if (((x - right.start.x) / right.dir.x) < 0)
        {

            return null;
        }
        if (((y - right.start.y) / right.dir.y) < 0)
        {
            return null;
        }

        // if all of this checks fail then a vector with x and y is returned
  
        return new Vector2(x, y);


    }

    private List<Vector2> GenerateRandomSites()
    {
        List<Vector2> rndSites = new List<Vector2>();

        

        for (int i = 0; i < numberOfSites;)
        {
            float randX = Random.Range(0, width);
            float randY = Random.Range(0, height);
            bool apart = true;
            Vector2 newSite = new Vector2(randX, randY);
            foreach (var site in rndSites)
            {
                if (Vector3.Distance(newSite,site) < 25f)
                {
                    apart = false;
                }
            }
            if (apart ==true)
            {

                rndSites.Add(newSite);
                i++; 
            }


        }
        return rndSites;
        
    }
    private void InsertEventToQueue(Event e)
    {
        
        if (queue.Count == 0)
        {
            queue.Add(e);
        }
        else
        {
            for (int j = 0; j < queue.Count; j++)
            {
                if (e.position.y < queue[j].position.y)
                {
                    queue.Insert(j, e);
                    return;
                }

            }
            queue.Add(e);
        }
    }
    private float getXOfEdge(Node n)
    {
        if (n.edge.dir.x == 0)
        {
            return n.edge.start.x;
        }
        // finds the current endpoint of a edge by calculating the intersectionpoint of the parabolas to its right and left
        Vector2 lp = n.previous.position;
        Parabola left = GetParabola(lp);
        Vector2 rp = n.next.position;
        Parabola right = GetParabola(rp);


        float la = left.A;
        float lb = left.B;
        float lc = left.C;

        float ra = right.A;
        float rb = right.B;
        float rc = right.C;

        float a = la - ra ;
        float b = lb - rb ;
        float c = lc - rc;

        float discriminant = b * b - 4 * a * c;

        float testx1 = -(lb - rb) + Mathf.Sqrt(Mathf.Pow(lb - rb,2) - 4 * (la - ra) * (lc - rc));
        float testx2 = -(lb - rb) - Mathf.Sqrt(Mathf.Pow(lb - rb, 2) - 4 * (la - ra) * (lc - rc));

        float x1 = testx1 / (2 * (la - ra));
        float x2 = testx2 / (2 * (la - ra));

        float interX;
        if (n.edge.dir.x > 0)
        {
            interX = Mathf.Max(x1, x2);
        }
        else
        {
            interX = Mathf.Min(x1, x2);
        }
        return interX;


    }
    private Node FindNodeOverPoint(Vector2 current)
    {
        
        
        if (firstInBeach == null)
        {
            return null;
        }
        Node site = firstInBeach;
        Node edge = site.next;
        float x = 0;
        

        
        while (edge != null)
        {
            x = getXOfEdge(edge);
            if (current.x > x)
            {
                site = edge.next;
                edge = site.next;
            }
            else
            {
                return site;
            }
        }
        return site;
	

	}


    private Parabola GetParabola(Vector2 focus)
    {


        float h = focus.x;
        // k is the half way of the distance between the focus and directrix
        float yDis = Vector2.Distance(focus, new Vector2(h, sweepLine));
        float k = focus.y + yDis / 2;
        // p is the distance between the focus and vertex !!! becaus the parabola is alway opening towards negative y p will always be negative
        float p = Vector2.Distance(focus, new Vector2(h, k)) * -1 ;
        // (x - h)² = 4p(y - k)
        float a;
        float b;
        float c;
        if (p == 0)
        {
            a = 0;
            b = 0;
            c = focus.x * focus.x;
        }
        else
        {

            a = 1 / (4 * p); 
            float hh = h*2*-1;
            float pp = 4 * p;
            b = hh / pp;

            c = ((h * h) + 4 * p * k) / (4 * p);
        }
        return new Parabola(a, b, c);

      

    }
    private float GetY(Vector2 focus, Vector2 eventPos)
    {
        Parabola p = GetParabola(focus);
        
        
        float y = p.A * (eventPos.x * eventPos.x) + p.B * eventPos.x + p.C;
       

        return y;
    }
    void DrawParabolaLine(Node n)
    {
        n.lineRenderer.positionCount = 400;
        Parabola p = GetParabola(n.position);
        n.lineRenderer.transform.position = new Vector3(n.position.x, 1, n.position.y);
        for (int x = -200; x < 200; x++)
        {
            float y = p.A * (x * x) + p.B * x + p.C;
            
            n.lineRenderer.SetPosition(x + 200, new Vector3(x, 1, y));
            n.lineRenderer.transform.name = n.generatedBy;
        }
    }

}
[System.Serializable]
public class Node
{
    public Vector2 position;
    public string generatedBy;
    public Edge edge;
    public bool type; // false = site , true = edge
    public Node next = null;
    public Node previous = null; 
    public Event cirlce;
    public LineRenderer lineRenderer;
    public Node(Vector2 _postition, bool _type, string generated)
    {
         this.position = _postition;
         this.type = _type;
        next = null;
        previous = null;
        generatedBy = generated;
   
    }
    public Node (Edge _edge, bool _type, string generated)
    {
        this.edge = _edge;
        this.type = _type;
        next = null;
        previous = null;
        generatedBy = generated;

    }
    public void SetNext(Node _next)
    {
        if (_next != null)
        {

            _next.previous = this;
        }
            next = _next; 
    }


}
[System.Serializable]
public class Edge
{
    public Vector2 start;
    public float m; // steigung
    public float b;
    public Vector2 dir;
    public Vector2 ndir;
    public Edge adjacent;
    public Vector2 end = new Vector2(12121212f,12121212f);
    public Vector2 controllPoint;
    public string LR = "";
    public Edge(Vector2 _start, Vector2 _left, Vector2 _right, string _LR)
    {
        start = _start;
        LR = _LR;
        
        dir = new Vector2(_right.x- _left.x  , _right.y - _left.y );
        
        //dir = Vector2.Perpendicular(dir);
        
        ndir.x = dir.y;
        ndir.y = -dir.x;
        dir = ndir; 
        Vector2 normDir = 1f / dir.magnitude * dir;
        controllPoint = start + normDir * 0.5f;
        controllPoint.x = start.x + normDir.x;
        controllPoint.y = start.y + normDir.y ;
        float xDiff = 0;
        float yDiff = 0;
        if (dir.x > 0)
        {
            yDiff = controllPoint.y - start.y;
            xDiff = controllPoint.x - start.x;
        }
        else if (dir.x < 0)
        {
            yDiff = start.y - controllPoint.y;
            xDiff = start.x - controllPoint.x;
        }
        m = yDiff / xDiff;
        b = _start.y - m * start.x;
        if (float.IsInfinity(m)||float.IsNaN(m))
        {
            b = start.x;
        }
        

        
        if (float.IsNaN(m)|| float.IsInfinity(m)||float.IsNaN(b)||float.IsInfinity(b))
        {
            Debug.Log("asndkadj");
        }
        adjacent = null;
        
    }
    
    
}
public class Parabola
{
    public float A;
    public float B;
    public float C;
    public Parabola (float _A, float _B, float _C)
    {
         A = _A;
         B = _B;
         C = _C;
    }

}
[System.Serializable]
public class Event
{
    public Vector2 position;
    public Node node;
    public Event circle;
    public bool type; // true == circle , false == site
    public string generatedBy;
    public Event(Vector2 pos, bool _type, string _generated)
    {
        generatedBy = _generated;
        position = pos;
        type = _type;
    }public Event(Vector2 pos, bool _type, Node n, string _generated)
    {
        generatedBy = _generated;
        position = pos;
        type = _type;
        node = n;
        
    }
}
