using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BaseInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _requiredDistance = 0.3f;


    protected bool _detected;


    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnDetected()
    {

    }

    public virtual void  Initialize()
    {
    
    }

    public virtual void OnClicked()
    { 
    
    }

    public float GetDistanceRequirement()
    {
        return _requiredDistance;
    }

    protected bool _interactable;

    public bool IsInteractable()
    {
        return _interactable;
    }

    public bool IsDetected()
    {
        return _detected;
    }

    public void Detect()
    {

        Detected = true;
    }

    public virtual  bool Detected { get; set; }

    
}
