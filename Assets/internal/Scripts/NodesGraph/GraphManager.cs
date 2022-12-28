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


    private Node FindNearestNodeToPoint(Vector3 pos)
    {
        var distance = float.MaxValue;
        Node NearestNode = null;
        foreach (Node n in _nodes)
        {
            
            float curDistance =Vector3.Distance(pos,n.transform.position);
            if (curDistance < distance)
            {
                distance = curDistance;
                NearestNode = n;
            }
        }
        return NearestNode;
    
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

    public Node[] GetPath(Edge desiredEdge,(Node,Node) currentEdge, Vector3 FinalPos)
    {

        Node[] Path = null;



        return Path;
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
