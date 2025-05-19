using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementCtrl : MonoBehaviour
{
    [SerializeField] private bool running = false;
    private Vector2 movementInput;
    public LayerMask groundLayer;

    [Header("Look")]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float lookSensivity;
    [SerializeField] private Camera firstPersonCamera;
    private Vector2 mouseDelta;
    private float camXRot;

    private PlayerStat stat;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stat = GetComponent<PlayerStat>();
        if(rb == null || stat == null)
        {
            Debug.LogWarning(this.name + ": Player Essential Component Not Found");
            this.enabled = false;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Move()
    {
        
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= stat.Speed;
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    private void Look()
    {
        camXRot += mouseDelta.y * lookSensivity;
        camXRot = Mathf.Clamp(camXRot, minXLook, maxXLook);
        firstPersonCamera.transform.localEulerAngles = new Vector3(-camXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {   
        if(context.phase == InputActionPhase.Performed)
        {   //입력이 수행될 때(버튼을 누르고 있을 떄) 동작 실행
            movementInput = context.ReadValue<Vector2>();   
        }
        else if(context.phase == InputActionPhase.Canceled)
        {   //입력 취소(버튼을 땔 떄)
            movementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }  
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {   //입력 시작(버튼 클릭)
            rb.AddForce(Vector2.up * stat.JumpPower, ForceMode.Impulse);   
        }
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {   //플레이어의 앞, 뒤, 좌, 우 아래방향으로 레이캐스팅하여 땅에 닿았는지 확인
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        foreach(Ray ray in rays)
        {
            if (Physics.Raycast(ray, 0.1f, groundLayer))
            {
                return true;
            }
        }
        return false;
    }
}
