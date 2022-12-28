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

    private IEnumerator AnimateMovement(Vector3[] points)
    {
        foreach (Vector3 point in points)
        {
            Vector3 endPos = point;
            Vector3 startPos = transform.position;
            
            float elapsedTime = 0;

            while (elapsedTime < _movementDuration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / _movementDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        _animateCo = null;
    }


    public (Node, Node)  CurrentNodes = (null,null);

    public void MoveAlongPoints(Vector3[] points)
    {
        Debug.Log(points);
        if (_animateCo != null)
        {
            StopCoroutine(_animateCo);
            _animateCo = null;
        }
        _animateCo = StartCoroutine(AnimateMovement(points));

    }
}
