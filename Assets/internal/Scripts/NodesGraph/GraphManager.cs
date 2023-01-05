using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GraphManager : MonoBehaviour
{

    [Header("Map")]
    [SerializeField] private Material _mapMaterial;
    
    private LineRenderer[] _lines;
    private Edge[] _edges;
    private Node[] _nodes;


    private void Awake()
    {
        _nodes = (Node[]) FindObjectsOfType(typeof( Node));
        List < Edge > edges= new List<Edge>();
        foreach (Node n in _nodes)
        {
            foreach (Node v in n.GetVectors())
            {
                var e = new Edge((n, v), Vector3.Distance(n.transform.position, v.transform.position));
                if (!edges.Any(ed =>ed==e))
                {
                    edges.Add(e);
                }
            }
        }
        _edges = edges.ToArray();
        DrawMap();
    }


    private void DrawMap()
    {
        GameObject MapObject = new GameObject("map");

        List<LineRenderer> lines = new List<LineRenderer>();
        foreach (Edge e in _edges)
        {
            GameObject gObject = new GameObject("linesObject");
            gObject.transform.parent = MapObject.transform;
            LineRenderer line = gObject.AddComponent<LineRenderer>();

            line.material = _mapMaterial;
            line.positionCount = 2;
            line.startWidth = .035f;
            line.endWidth = .035f;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.textureMode = LineTextureMode.Tile;
            line.SetPositions(e.GetPositions());
            lines.Add(line);
            gObject.AddComponent<Line>().SetLine( e);

        }

        _lines = lines.ToArray();
    }


    private (Vector3,Edge) RaycastAround(Vector3 pos)
    {
        Vector3 newPos = new Vector3(-1,-1,-1);
         int RaysToShoot = 30;
        Edge newEdge = null;
        float angle = 0;
        float distance = float.MaxValue;
        for (int i=0; i<RaysToShoot; i++) {
        
        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        angle += 2 * Mathf.PI / RaysToShoot;
 
        Vector3 dir = new Vector3(pos.x + x, pos.y + y, 0);
        RaycastHit hit;
        if (Physics.Raycast (pos, dir, out hit)) {
                if (hit.distance < distance && hit.transform.tag=="line")
                {

                    distance = hit.distance;
                    newPos = hit.point;
                    newEdge = hit.collider.transform.parent.GetComponent<Line>().edge;
                }
         }
     }
      
        return (newPos,newEdge);
    }
   

    public (Vector3, Edge) GetNearestPointOnLine(Vector3 pos)
    {


        (Vector3, Edge) scan = RaycastAround(pos);
        return scan;
    }

    public (List<Node>, float Distance) GetPath(Edge desiredEdge,(Node,Node) currentEdge,Vector3 startPos,  Vector3 FinalPos ,List<Node> path, float distance)
    {

        List<Node> Path1 = path;
        float newDistance1 = distance;
        List<Node> Path2 = path;
        float newDistance2 = distance;

        if (currentEdge.Item1.GetVectors().Length == 0 && currentEdge.Item2.GetVectors().Length == 0)
        {
            return (path, distance);

        }

        if (distance == 0)
        {
            newDistance1 = Vector3.Distance(currentEdge.Item1.transform.position, startPos);
            newDistance2 = Vector3.Distance(currentEdge.Item2.transform.position, startPos);

        }

      
        Node first = currentEdge.Item1;
        float distance1 = float.MaxValue;
        List<Node> tempPath1= new List<Node>();
        tempPath1.AddRange(Path1);
       
        if (!tempPath1.Contains(first))
        {
            tempPath1.Add(first);
            if (first == desiredEdge.Nodes.Item2 || first == desiredEdge.Nodes.Item1)
            {
                if (distance1 > Vector3.Distance(first.transform.position, startPos))
                {
                    distance1 = Vector3.Distance(first.transform.position, startPos);
                    var result = new List<Node>();
                    result.AddRange(tempPath1);
                    Path1 = result;
                }
            }
            foreach (Node n in first.GetVectors())
            {
                if (!tempPath1.Contains(n) && n!= currentEdge.Item2)
                {
                    if (n == desiredEdge.Nodes.Item2 || n == desiredEdge.Nodes.Item1)
                    {
                        if (distance1 > Vector3.Distance(first.transform.position, n.transform.position)+newDistance1)
                        {
                            distance1 = Vector3.Distance(first.transform.position, n.transform.position);
                            var result = new List<Node>();
                            result.AddRange(tempPath1);
                            result.Add(n);
                            Path1 = result;
                        }
                    }
                    else
                    {
                        var result = new List<Node>();
                        result.AddRange(tempPath1);
                        result.Add(n);
                        (List<Node>, float Distance) explore = GetPath(desiredEdge, (first, n), startPos, FinalPos, result, newDistance1+(Vector3.Distance(first.transform.position,n.transform.position)));
                        if (distance1 > explore.Item2)
                        {
                            distance1 = explore.Item2;
                            Path1 = explore.Item1;
                        }
                    }

                }
            }
        }

    
        Node second = currentEdge.Item2;
        float distance2 = float.MaxValue;
        List<Node> tempPath2 = new List<Node>();
        tempPath2.AddRange(Path2);
       
        if (!tempPath2.Contains(second) )
        {
            tempPath2.Add(second);
            if (second == desiredEdge.Nodes.Item2 || second == desiredEdge.Nodes.Item1)
            {
                if (distance2 > Vector3.Distance(second.transform.position, startPos))
                {
                    distance2 = Vector3.Distance(second.transform.position, startPos);
                    var result = new List<Node>();
                    result.AddRange(tempPath2);
                    Path2 = result;
                   
                    string z = "";

                    foreach (Node nn in Path2.ToArray())
                    {
                        z += nn.gameObject.name + " ";
                    }

                }
            }
            foreach (Node n in second.GetVectors())
            {
                if (!tempPath2.Contains(n) && n != currentEdge.Item1)
                {
                    if (n == desiredEdge.Nodes.Item2 || n == desiredEdge.Nodes.Item1)
                    {
                     
                        if (distance2 > Vector3.Distance(second.transform.position, n.transform.position)+newDistance2)
                        {
                            distance2 = Vector3.Distance(second.transform.position, n.transform.position);
                            var result = new List<Node>();
                            result.AddRange(tempPath2);
                            result.Add(n);
                            Path2 = result;
                           
                            string z = "";

                            foreach (Node nn in Path2.ToArray())
                            {
                                z += nn.gameObject.name + " ";
                            }

                        }
                    }
                    else
                    {
                        var result = new List<Node>();
                        result.AddRange(tempPath2);
                        result.Add(n);
                        (List<Node>, float Distance) explore = GetPath(desiredEdge, (second, n), startPos, FinalPos, result, newDistance2 + (Vector3.Distance(second.transform.position, n.transform.position)));
                        if (distance2 > explore.Item2)
                        {
                            distance2 = explore.Item2;
                            Path2 = explore.Item1;
                          
                            string z = "";

                            foreach (Node nn in Path2.ToArray())
                            {
                                z += nn.gameObject.name + " ";
                            }
                        }
                    }

                }
            }
        }


        newDistance1 += distance1;
        newDistance2 += distance2;
     


        if (newDistance1 == newDistance2)
        {
            if (Path1.Count == Path2.Count)
            {
                return (Path1, newDistance1);
            }
            else if (Path1.Count < Path2.Count)
            {
                return (Path1, newDistance1);

            }
            else {
                return (Path2, newDistance2);

            }
        }
        else if (newDistance1 < newDistance2)
        {
            return (Path1, newDistance1);
        }
        else
        {
            return (Path2, newDistance2);
        }
    }

}

public class Edge
{
    public (Node, Node) Nodes;
    public float Value;
    public Edge((Node, Node) nodes, float value)
    {
        Nodes = nodes;
        Value = value;

    }

    public override int GetHashCode() => Nodes.GetHashCode();


    public static bool operator ==(Edge e1, Edge e2)
    {
        if (e1 is null && e2 is null) return true;
        if (e1 is null || e2 is null) return false;
        HashSet<Node> h1 = new HashSet<Node> { e1.Nodes.Item1, e1.Nodes.Item2 };
        HashSet<Node> h2 = new HashSet<Node> { e2.Nodes.Item1, e2.Nodes.Item2 };

        return h1.SetEquals(h2);
    }

    public override bool Equals(object obj) => this.Equals(obj as Edge);
    public bool Equals(Edge e)
    {
        if (e == null) return false;
        return this == e;
    }


    public bool CompareNodes((Node, Node) nodes)
    {
        HashSet<Node> h1 = new HashSet<Node> { Nodes.Item1, Nodes.Item2 };
        HashSet<Node> h2 = new HashSet<Node> { nodes.Item1, nodes.Item2 };
        return h1.SetEquals(h2);
    }

    public Vector3[] GetPositions()
    {
        Vector3[] p = { Nodes.Item1.transform.position, Nodes.Item2.transform.position };
        return p;
    }
    public Vector3[] GetPositionsZModified(float zMod)
    {
        Vector3[] p = { Nodes.Item1.transform.position, Nodes.Item2.transform.position };
        p[0].z += zMod;
        p[1].z += zMod;

        return p;
    }

    public static bool operator !=(Edge e1, Edge e2) => !(e1 == e2);


}
