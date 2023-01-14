using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    [SerializeField] private Node _startNode;
    [SerializeField] float _movementSpeed;
    [SerializeField] Transform _character;
   

    private Coroutine _animateCo = null;


    private BaseInteractable[] _interactables;


    private void Awake()
    {
        Update2DPosition(_startNode.transform.position.x, _startNode.transform.position.y);
        CurrentNodes = (_startNode, _startNode.GetVectors()[0]);
       var inters=  GameObject.FindGameObjectsWithTag("Interact");
        List<BaseInteractable> scripts = new List<BaseInteractable>();
        foreach (var i in inters)
        {
            scripts.Add(i.GetComponent<BaseInteractable>());
        }
        _interactables = scripts.ToArray();
    }

    private void CheckInteractables()
    {
        foreach (var obj in _interactables)
        {
            if((transform.position - obj.transform.position).magnitude <= obj.GetDistanceRequirement())
            {
                if (!obj.IsDetected())
                {
                    obj.Detect();
                }
            }
        }
    }

    private void Update()
    {
        CheckInteractables();
    }


    private void Update2DPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private IEnumerator AnimateLastMovement(Vector3 pos)
    {
        AudioManager.PlayWalking();

        Vector3 endPos = new Vector3(pos.x,pos.y, transform.position.z);
        float AngleRad = Mathf.Atan2(endPos.y - transform.position.y, endPos.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        _character.eulerAngles = new Vector3(90, 0,90-AngleDeg);
        _character.localEulerAngles= new Vector3(90, _character.localEulerAngles.y, _character.localEulerAngles.z);

        while (Vector3.Distance(endPos,transform.position)>0)
        {
            if (GameManager.GetState() == State.Paused)
            {
                AudioManager.EnableAudioEffects(false);

                yield return null;
                continue;
            }
            AudioManager.EnableAudioEffects(true);

            transform.position = Vector3.MoveTowards(transform.position, endPos, _movementSpeed * Time.deltaTime);
            yield return null;
        }
        _animateCo = null;
        AudioManager.StopWalking();




    }

    private IEnumerator AnimateMovement(Node[] points,Vector3 finalPos, (Node,Node) GoalNodes)
    {
        AudioManager.PlayWalking();
        int index = 0;
        Node prev = null;
        foreach (Node point in points)
        {
            Vector3 endPos = new Vector3(point.transform.position.x,  point.transform.position.y, transform.position.z);
            Vector3 startPos = transform.position;
            float AngleRad = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x);
            // Get Angle in Degrees
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            // Rotate Object
            _character.eulerAngles = new Vector3(90, 0, 90 - AngleDeg);
            _character.localEulerAngles = new Vector3(90, _character.localEulerAngles.y, _character.localEulerAngles.z);

            if (index > 0)
            {
                CurrentNodes = (prev, point);
                prev = point;
            }
            while (Vector3.Distance(endPos, transform.position) > 0)
            {
                if (GameManager.GetState() == State.Paused)
                {
                    AudioManager.EnableAudioEffects(false);

                    yield return null;
                    continue;
                }
                AudioManager.EnableAudioEffects(true);

                transform.position = Vector3.MoveTowards(transform.position, endPos, _movementSpeed * Time.deltaTime);
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
        if (_animateCo != null)
        {
            return;
            
        }
        _animateCo = StartCoroutine(AnimateMovement(Nodes,finalPos,GoalNodes));

    }


    public void MovetoPosition(Vector3 finalPos)
    {
        if (_animateCo != null)
        {
            return;

            
        }
        _animateCo = StartCoroutine(AnimateLastMovement(finalPos));
    }

    public bool CoroutineRunning()
    {
        return _animateCo != null;
    }
}
