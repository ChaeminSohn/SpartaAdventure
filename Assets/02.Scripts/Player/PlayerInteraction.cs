using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractableObject;
    private IInteractable curInteractable;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if(Physics.Raycast(ray, out RaycastHit hit, maxCheckDistance, layerMask))
            {
                if(hit.collider.gameObject != curInteractableObject)
                {
                    curInteractableObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    EventBus.Raise(new PlayerEnteredInteractionRangeEvent(curInteractable));
                }
            }
            else
            {
                if (curInteractableObject != null)
                {
                    EventBus.Raise(new PlayerExitedInteractionRangeEvent(curInteractable));
                    curInteractableObject = null;
                    curInteractable = null;
                }

            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null) 
        {
            curInteractable.OnInteract();
            //curInteractableObject = null;
            //curInteractable = null;
        }
    }
}
