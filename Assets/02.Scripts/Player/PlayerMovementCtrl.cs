using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementCtrl : MonoBehaviour
{
    private Vector2 movementInput;
    public LayerMask groundLayer;

    [Header("Look")]
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float lookSensivity;
    //[SerializeField] private Camera firstPersonCamera;
    public float currentPitch { get; private set; } = 0f;
    private Vector2 mouseDelta;
    private float camXRot;

    [Header("스태미나 소모량")]
    [SerializeField] private float runStaminaSec = 3f;
    [SerializeField] private float jumpStaminaCost = 20f;

    [SerializeField] private LayerMask platformLayerMask;
    private Transform currentlyAttachedPlatform;

    private PlayerStat stat;
    private PlayerAnimationCtrl animationCtrl;
    private Rigidbody rb;

    

    private bool isRunning = false;
    private float lastRunStaminaCostTime;

    private bool isCurrentlyGrounded = true;
    private bool wasGroundedLastFrame = true;
    private bool isNormalJumpAirborne = false;
    private bool canMove = true;

    private Coroutine staminaRegenCoroutine;
    private Coroutine airborneCoroutine;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stat = GetComponent<PlayerStat>();
        animationCtrl = GetComponent<PlayerAnimationCtrl>();
        if(rb == null || stat == null)
        {
            Debug.LogWarning(this.name + ": Player Essential Component Not Found");
            this.enabled = false;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        staminaRegenCoroutine = StartCoroutine(StaminaGenRoutine());
    }

    private float lastCheckTime;
    private void Update()
    {
        if (Time.time < lastCheckTime + 0.1f)
        {
            return;
        }
        lastCheckTime = Time.time;

        isCurrentlyGrounded = IsGrounded();

        if (isCurrentlyGrounded)
        {
            if(!wasGroundedLastFrame)   // 막 착지했을 경우
            {   
                if (airborneCoroutine == null && !isNormalJumpAirborne)
                {
                    StartCoroutine(LandingRoutine());
                }
                isNormalJumpAirborne = false;
            }
        }
        else  //공중에 떠있는 경우
        {
            if (wasGroundedLastFrame)   //방금 땅에서 떨어진 경우
            {
                if (!isNormalJumpAirborne)
                {
                    animationCtrl.Fall();
                }
            }
            else  //계속 공중에 떠있는 경우
            {
                if (!isNormalJumpAirborne)
                {
                    animationCtrl.Fall();
                }
            }
        }

        wasGroundedLastFrame = isCurrentlyGrounded;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        //Look();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= stat.Speed;
        if (isRunning && isCurrentlyGrounded)
        {
            dir *= 1.5f;
            if(Time.time > lastRunStaminaCostTime + 0.5f)
            {
                lastRunStaminaCostTime = Time.time;
                stat.UseStamina(runStaminaSec);
            }
        }
        dir.y = rb.velocity.y;

        rb.velocity = dir;
    }

    private void Look()
    {
        // y축 기준으로만 회전
        camXRot += mouseDelta.y * lookSensivity;
        camXRot = Mathf.Clamp(camXRot, minXLook, maxXLook);
        //currentPitch += mouseDelta.y;
        //currentPitch = Mathf.Clamp(camXRot, minXLook, maxXLook);
        //firstPersonCamera.transform.localEulerAngles = new Vector3(-camXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensivity, 0);
    }

    public void Jump(float jumpPower)
    {
        animationCtrl.Jump();
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // y속도 초기화 후 점프
        rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);

        //새 점프를 실행하므로 기존 공중상태 코루틴이 있다면 정지
        if(airborneCoroutine != null)
        {
            StopCoroutine(airborneCoroutine);
            airborneCoroutine = null;
        }


        //큰 점프인지 확인
        if (jumpPower > stat.JumpPower)
        {
            airborneCoroutine = StartCoroutine(BigJumpAirborneRoutine());
        }
        else
        {
            StartCoroutine(NormalJumpRoutine());
        }
    }

        public void OnMove(InputAction.CallbackContext context)
    {   
        if(context.phase == InputActionPhase.Performed)
        {   //입력이 수행될 때(버튼을 누르고 있을 떄) 동작 실행
            animationCtrl.Move();
            movementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {   //입력 취소(버튼을 땔 떄)
            movementInput = Vector2.zero;
            animationCtrl.Stop();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }  
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && isCurrentlyGrounded)
        {   //입력 시작(버튼 클릭)
            stat.UseStamina(jumpStaminaCost);
            Jump(stat.JumpPower);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && IsGrounded())
        {
            animationCtrl.Run();
            isRunning = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            animationCtrl.RunStop();
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

        RaycastHit hit;
        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray, out hit, 0.1f, groundLayer))
            {
                if (Physics.Raycast(ray, out hit, 0.1f, platformLayerMask))
                {   //플랫폼에 닿았을 경우
                    //움직이는 발판에 올라있을 경우, 같이 움직이기 위해 부모 설정
                    if(currentlyAttachedPlatform != hit.transform)
                    {
                        transform.SetParent(hit.transform);
                        currentlyAttachedPlatform = hit.transform;
                        Debug.Log("플레이어 발판에 부모 설정됨 " + hit.transform.name);
                    }
                }
                else if(currentlyAttachedPlatform != null)
                {   //플랫폼에서 내려왔을 경우, 부모 해제
                    transform.SetParent(null);
                    Debug.Log("플레이어 발판에서 부모 해제됨: " + currentlyAttachedPlatform.name);
                    currentlyAttachedPlatform = null;

                }
                return true;
            }
            else
            {
                if (currentlyAttachedPlatform != null)
                {   //플랫폼에서 내려왔을 경우, 부모 해제
                    transform.SetParent(null);
                    Debug.Log("플레이어 발판에서 부모 해제됨: " + currentlyAttachedPlatform.name);
                    currentlyAttachedPlatform = null;
                }
            }
        }
            return false;
    }

    IEnumerator StaminaGenRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);      //1초마다 스태미나 자연 회복

            if (!isRunning)
            {
                Debug.Log("스태미나 회복 코루틴");
                stat.RecoverStamina(5.0f);
            }
        }
    }

    IEnumerator NormalJumpRoutine()
    {
        Debug.Log("Start Normal Jump Coroutine");
        isNormalJumpAirborne = true;
        yield return new WaitForSeconds(1.0f);
        isNormalJumpAirborne = false;
    }
    IEnumerator BigJumpAirborneRoutine()
    {
        animationCtrl.Fall();
        yield return new WaitForFixedUpdate();
        while (!IsGrounded())
        {
            yield return new WaitForEndOfFrame();
        }
        animationCtrl.Land();
        airborneCoroutine = null;
    }

    IEnumerator LandingRoutine()
    {
        animationCtrl.Land();
        canMove = false;
        yield return new WaitForSeconds(2.0f);
        canMove = true;
    }
}
