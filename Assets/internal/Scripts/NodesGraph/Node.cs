using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Node[] _vectors;

    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

   


    public Node[] GetVectors()
    {
        return _vectors;
    }


}
