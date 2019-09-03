using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteGenerator : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

     public List<Vector2> GenerateRandom ( Vector2 groundPos, float groundR,int width, int height, int numberOfSites)
     {
        List<Vector2> s = new List<Vector2>();
        for (int i = 0; i < numberOfSites; )
        {

            float rndX = Random.Range(groundPos.x - groundR, groundPos.x + groundR);
            float rndY = Random.Range(groundPos.y - groundR,  groundPos.y + groundR);
            if (Vector2.Distance(new Vector2(groundPos.x, groundPos.y), new Vector2(rndX, rndY)) < groundR)
            {
                s.Add(new Vector2(rndX, rndY));
                i++;
            }
        }

        return s;
     }
    public List<Vector2> GenerateGrid(Vector2 groundPos, float groundR, int width, int height)
    {
        List<Vector2> s = new List<Vector2>();

        for (int x = 0; x < width; x = x + 10)
        {
            for (int y = 0; y < height; y = y + 10)
            {
                if (Vector2.Distance(new Vector2(groundPos.x,groundPos.y),new Vector2(x,y)) < groundR)
                {
                    s.Add(new Vector2(x, y));

                }
            }
        }

        return s;
    }
    public List<Vector2> GenerateCircular(int height, int width, float groundR, Vector2 GroundPos, int circles , int pointsPerCircle)
    {
        List<Vector2> s = new List<Vector2>();
        s.Add(GroundPos);
        float tempR = groundR / circles;
        for (int i = 0; i < circles ; i++)
        {
            for (int j = 0; j < pointsPerCircle; j++)
            {
                Vector2 pos = Random.insideUnitCircle * tempR;

                pos += GroundPos;
                s.Add(pos);
            }
            tempR += groundR / circles;
        }


        return s;
    }
    
}
