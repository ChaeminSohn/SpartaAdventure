using UnityEngine;
using UnityEngine.InputSystem;

public class CameraCtrl : MonoBehaviour
{
    private Transform playerBody; // 플레이어 캐릭터의 Transform (Y축 회전 주체)

    [Header("마우스 및 회전 설정")]
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private float minPitch = -80f; // 최소 상하 각도
    [SerializeField] private float maxPitch = 80f;  // 최대 상하 각도

    [Header("1인칭 시점 설정")]
    [Tooltip("firstPersonMountPoint로부터의 로컬 위치 오프셋. MountPoint가 머리라면 (0,0,0)에 가깝게 설정합니다.")]
    [SerializeField] private Vector3 firstPersonLocalPosition = new Vector3(0f, 0f, 0f); // MountPoint 기준 위치
    [SerializeField] private LayerMask firstPersonLayer;
    private Transform firstPersonParent;
    

    [Header("3인칭 시점 설정")]
    [Tooltip("플레이어의 어느 지점을 기준으로 카메라가 위치할지 (플레이어 로컬 좌표 오프셋)")]
    [SerializeField] private Vector3 thirdPersonTargetOffset = new Vector3(0f, 1.2f, 0f);
    [SerializeField] private float thirdPersonDistance = 3.5f;
    [SerializeField] private LayerMask thirdPersonLayer;
    // 3인칭 시 Pitch는 마우스 입력을 그대로 사용. 별도 Height 설정 대신 Pitch로 조절.


    private float yaw = 0.0f;   // 현재 Y축 회전값 (좌우)
    private float pitch = 0.0f; // 현재 X축 회전값 (상하)
    private bool isFirstPerson = true;
    private Vector2 mouseDelta;
    private Camera cam;

    void Start()
    {
        playerBody = transform.root;
        firstPersonParent = transform.parent;
        cam = GetComponent<Camera>(); // 이 스크립트는 카메라 오브젝트에 있어야 합니다.

        if (playerBody == null)
        {
            Debug.LogError("Player Body Transform이 할당되지 않았습니다!");
            enabled = false;
            return;
        }

        // 초기 Yaw는 플레이어의 현재 Y축 회전값을 따름
        yaw = playerBody.eulerAngles.y;
        // 초기 Pitch는 현재 카메라의 X축 회전값을 따르거나 0으로 시작
        // (카메라가 초기에 특정 각도를 가지고 있다면 해당 값으로, 아니면 0)
        pitch = transform.localEulerAngles.x;
        if (pitch > 180) pitch -= 360; // 각도를 -180 ~ 180 범위로 변환

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        ApplyCameraView(); // 초기 시점 적용
    }

    void Update()
    {
        yaw += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        pitch -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // Pitch 값 범위 제한
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // Pitch 값 범위 제한
    }

    void LateUpdate()
    {
        // LateUpdate에서 카메라 위치/회전 최종 적용 (플레이어 이동 후)
        ApplyCameraView();
    }

    void ApplyCameraView()
    {
        // 플레이어 몸체 Y축 회전 적용 (1인칭, 3인칭 공통)
        playerBody.rotation = Quaternion.Euler(0, yaw, 0);

        if (isFirstPerson)
        {
            // 1인칭 시점 
            transform.parent = firstPersonParent;
            transform.localPosition = firstPersonLocalPosition;
            transform.localRotation = Quaternion.Euler(pitch, 0, 0); // 로컬 X축 회전만 Pitch 적용
        }
        else
        {
            //  3인칭 시점 
            if (transform.parent != null)
            {
                transform.SetParent(null); // 월드 기준으로 위치 설정하기 위해 부모 해제
            }

            // 카메라가 바라볼 목표 지점 (플레이어 로컬 오프셋 -> 월드 좌표)
            Vector3 lookAtTargetWorld = playerBody.position + playerBody.TransformDirection(thirdPersonTargetOffset);

            // 목표 카메라 회전 (마우스 Yaw와 Pitch 모두 사용)
            Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0);

            // 목표 카메라 위치 (목표 지점에서 (회전값 * 거리) 만큼 뒤로 이동)
            Vector3 desiredPosition = lookAtTargetWorld - (desiredRotation * Vector3.forward * thirdPersonDistance);

            transform.position = desiredPosition;
            transform.LookAt(lookAtTargetWorld); // 항상 목표 지점을 바라보도록 설정
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
    public void OnViewChange(InputAction.CallbackContext context)
    {

        isFirstPerson = !isFirstPerson;
        EventBus.Raise(new PlayerViewChangeEvent(isFirstPerson));
        cam.cullingMask = isFirstPerson ? firstPersonLayer : thirdPersonLayer;
    }
}