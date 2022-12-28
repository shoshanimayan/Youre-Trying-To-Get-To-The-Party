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
        pos = PointEdge.Item1;
        _marker.SetMarker(pos);

        _graphManager.GetPath(PointEdge.Item2,_player.CurrentNodes, PointEdge.Item1);
    }

    public void SetPositionExact(Vector3 pos, Edge edge)
    {
        
        _marker.SetMarker(pos);
        _graphManager.GetPath(edge, _player.CurrentNodes, pos);

    }

}
