using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    [SerializeField] private  MarkerController _marker;
    [SerializeField] private PlayerHandler _player;

    private GraphManager _graphManager;

    private void Awake()
    {
        _graphManager = GetComponent<GraphManager>();
    }

    public void SetPosition(Vector3 pos)
    {

        (Vector3, Edge) PointEdge = _graphManager.GetNearestPointOnLine(pos);
        if (PointEdge.Item1 != new Vector3(-1, -1, -1))
        {
            pos = PointEdge.Item1;

            if (PointEdge.Item2.CompareNodes(_player.CurrentNodes))
            {

                _player.MovetoPosition(pos);
                return;
            }
            var x = _graphManager.GetPath(PointEdge.Item2, _player.CurrentNodes, _player.transform.position, PointEdge.Item1, new List<Node>(), 0);
            if ((x.Item1.ToArray().Length >= 1))
            {
                _marker.SetMarker(pos);

                _player.MoveAlongNodes(x.Item1.ToArray(), pos, PointEdge.Item2.Nodes);
            }
            else
            {
                Debug.LogError("Path Fail: current nodes "+_player.CurrentNodes+" destination nodes "+ PointEdge.Item2.Nodes);
            }
        }
    }

    public void SetPositionExact(Vector3 pos, Edge edge)
    {
        if (edge.CompareNodes(_player.CurrentNodes))
        {
      
            _player.MovetoPosition(pos);
            return;
        }
        var x=          _graphManager.GetPath(edge, _player.CurrentNodes,_player.transform.position, pos, new List<Node>(), 0);
        if ((x.Item1.ToArray().Length >= 1))
        {
            _marker.SetMarker(pos);
            _player.MoveAlongNodes(x.Item1.ToArray(), pos, edge.Nodes);
        }
        else
        {
            Debug.LogError("Path Fail: current nodes " + _player.CurrentNodes + " destination nodes " + edge.Nodes);
        }
    }

}
