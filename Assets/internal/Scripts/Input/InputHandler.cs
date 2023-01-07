using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(NavigationController))]
[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(UIManager))]


public class InputHandler : MonoBehaviour
{

    private NavigationController _navController;
    private UIManager _uiManager;

   
    private bool _navigated;

    private void Awake()
    {
        _navController = GetComponent<NavigationController>();
        _uiManager = GetComponent<UIManager>();
    }


    private void HintCheck()
    {
        if (!_navigated)
        {
            _uiManager.HideHint();
            _navigated = true;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.GetState() != State.Paused)
            {
                _uiManager.TogglePause(true);
            }
            else
            {
                _uiManager.TogglePause(false);

            }
            return;
        }

        if (GameManager.CanTouch())
        {

            if (Input.GetButtonDown("Fire1"))
            {
                if (GameManager.GetState() == State.Map)
                {
                    var screenPos = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(screenPos);
                    Plane xy = new Plane(Vector3.forward, new Vector3(-30, 0, 0));
                    float distance;
                    xy.Raycast(ray, out distance);

                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit, distance))
                    {
                        if (raycastHit.collider != null && raycastHit.collider.tag == "Interact")
                        {
                            var interactable = raycastHit.collider.gameObject.GetComponent<BaseInteractable>();
                            if (interactable.IsInteractable())
                            {
                                Debug.Log(interactable.gameObject);
                                interactable.OnClicked();

                                return;
                            }
                        }
                        if (raycastHit.collider.tag == "line")
                        {
                            HintCheck();
                            _navController.SetPositionExact(new Vector3(raycastHit.point.x, raycastHit.point.y, 0), raycastHit.collider.transform.parent.GetComponent<Line>().edge);
                            return;
                        }
                    }

                    var p = ray.GetPoint(distance);
                    if (p != null)
                    {
                        HintCheck();
                        _navController.SetPosition(new Vector3(p.x, p.y, 0));
                    }
                }
                else if (GameManager.GetState() == State.Text)
                {
                    _uiManager.SetTextMode(false);
                }

            }
        }
    }

}
