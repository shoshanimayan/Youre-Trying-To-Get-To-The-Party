using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private NavigationController _navController;


    private void Awake()
    {
        _navController = GetComponent<NavigationController>();
    }
   


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           
            var screenPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Plane xy = new Plane(Vector3.forward, new Vector3(-30,0,0));
            float distance;
            xy.Raycast(ray, out distance);

            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, distance))
            {
                if (raycastHit.collider != null && raycastHit.collider.tag=="Interact")
                {
                    Debug.Log(raycastHit.collider.gameObject);
                    return;
                }
                if (raycastHit.collider.tag == "line")
                {
                    _navController.SetPositionExact(new Vector3(raycastHit.point.x, raycastHit.point.y, 0), raycastHit.collider.transform.parent.GetComponent<Line>().edge);
                    return;
                }
            }

            var p = ray.GetPoint(distance);
            if (p != null)
            {
                _navController.SetPosition(new Vector3(p.x, p.y, 0));
            }

        }
    }

}
