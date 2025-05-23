using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    [SerializeField] private float minCheckDistance = 3f;
    [SerializeField] private float maxCheckDistance = 20f;
    private float checkDistance;
    public LayerMask layerMask;

    public GameObject curInteractableObject;
    private IInteractable curInteractable;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        checkDistance = minCheckDistance;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerViewChangeEvent>(PlayerViewChangeHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerViewChangeEvent>(PlayerViewChangeHandler);
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if(Physics.Raycast(ray, out RaycastHit hit, checkDistance, layerMask))
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

    private void PlayerViewChangeHandler(PlayerViewChangeEvent args)
    {
        checkDistance = args.isFirstPersonView ? minCheckDistance : maxCheckDistance;
    }
}
