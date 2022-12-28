using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Line : MonoBehaviour
{
    public Edge edge=null;
    private LineRenderer _line;
    private void Awake()
    {
        
        _line = GetComponent<LineRenderer>();
        SetCollider();
    }

    private void SetCollider()
    {
        BoxCollider col = new GameObject("Collider").AddComponent<BoxCollider>();
        col.transform.parent = transform; // Collider is added as child object of line
        Vector3 startPos = _line.GetPosition(0);
        Vector3 endPos = _line.GetPosition(1);
        float lineLength = Vector3.Distance(startPos, endPos); // length of line
        col.size = new Vector3(lineLength, 0.01f, 0.01f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Vector3 midPoint = (startPos + endPos) / 2;
        col.transform.position = midPoint; // setting position of collider object
                                           // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));
        if ((startPos.y < endPos.y && startPos.x > endPos.x) || (endPos.y < startPos.y && endPos.x > startPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);
       col.tag = "line";
    }

    public void SetLine(Edge e)
    {
        edge = e;
    }

    public (Node, Node) GetNodes()
    {
        return edge.Nodes;
    }

    public float GetEdgeValue()
    {
        return edge.Value;
    }
}
