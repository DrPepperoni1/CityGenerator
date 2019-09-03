using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoints : MonoBehaviour
{

    public GameObject attachmentPointPF;
    public List<GameObject> aps;

    public void CreateAttachPoints(float length, int amountOfPoints)
    {
        aps = new List<GameObject>();
        float spacePerPoints = length / (amountOfPoints-1);
        float forwardPos = 0f;
        for (int i = 0; i < amountOfPoints; i++)
        {
            GameObject newPoint = Instantiate(attachmentPointPF, transform.position + transform.forward * forwardPos, Quaternion.identity);
            newPoint.transform.parent = transform;
            aps.Add(newPoint);
            forwardPos += spacePerPoints;
        }
    }
    public GameObject FindClosest(GameObject obj)
    {
        GameObject closest = null;
        float dis = float.MaxValue;

        for (int i = 0; i < aps.Count; i++)
        {

            float curDis = Vector3.Distance(obj.transform.position, aps[i].transform.position);
            if (curDis < dis)
            {
                dis = curDis;
                closest = aps[i];
            }
        }

        return closest;
    }

}
