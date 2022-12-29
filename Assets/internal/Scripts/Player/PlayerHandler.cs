using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    [SerializeField] private Node _startNode;
    [SerializeField] float _movementDuration;


    private Coroutine _animateCo = null;


    private void Awake()
    {
        Update2DPosition(_startNode.transform.position.x, _startNode.transform.position.y);
        CurrentNodes = (_startNode, _startNode.GetVectors()[0]);
    }


    private void Update2DPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private IEnumerator AnimateLastMovement(Vector3 pos)
    {
        Vector3 endPos = pos;
        Vector3 startPos = transform.position;

        float elapsedTime = 0;
        
        while (elapsedTime < _movementDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / _movementDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _animateCo = null;
      //  Debug.Log(CurrentNodes.Item1);
       // Debug.Log(CurrentNodes.Item2);



    }

    private IEnumerator AnimateMovement(Node[] points,Vector3 finalPos, (Node,Node) GoalNodes)
    {
        int index = 0;
        Node prev = null;
        foreach (Node point in points)
        {
          //  Debug.Log(point);
            Vector3 endPos = point.transform.position;
            Vector3 startPos = transform.position;
            
            float elapsedTime = 0;
            if (index > 0)
            {
                CurrentNodes = (prev, point);
                prev = point;
            }
            while (elapsedTime < _movementDuration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / _movementDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if (index == 0)
            {
                prev = point;
            }
            index++;
        }
        _animateCo = null;

        _animateCo = StartCoroutine(AnimateLastMovement(finalPos));

        CurrentNodes = GoalNodes;
    }


    public (Node, Node)  CurrentNodes = (null,null);

    public void MoveAlongNodes(Node[] Nodes, Vector3 finalPos, (Node, Node) GoalNodes)
    {
       // Debug.Log(Nodes);
        if (_animateCo != null)
        {
            return;
            StopCoroutine(_animateCo);
            _animateCo = null;
        }
        _animateCo = StartCoroutine(AnimateMovement(Nodes,finalPos,GoalNodes));

    }


    public void MovetoPosition(Vector3 finalPos)
    {
        if (_animateCo != null)
        {
            return;

            StopCoroutine(_animateCo);
            _animateCo = null;
        }
        _animateCo = StartCoroutine(AnimateLastMovement(finalPos));
    }
}
