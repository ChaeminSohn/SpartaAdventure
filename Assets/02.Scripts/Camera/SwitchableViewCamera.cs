using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchableViewCamera : MonoBehaviour
{
    private Transform player;

    [Header("1인칭 시점 설정")]
    public Vector3 firstPersonOffset = new Vector3(0f, 0.7f, 0.2f); // 예: 플레이어 눈 위치

    [Header("3인칭 시점 설정")]
    public Vector3 thirdPersonLookAtOffset = new Vector3(0f, 1.2f, 0f); // 예: 플레이어 가슴 높이
    public float thirdPersonDistance = 4.0f;
    public float thirdPersonHeightOffset = 1.0f; // thirdPersonLookAtOffset 기준으로 추가 높이

    [Header("카메라 움직임")]
    public float smoothSpeed = 10f;

    private Camera mainCamera;
    private bool isFirstPersonView = true;
    private Vector3 currentVelocityPos; // 부드러운 이동을 위한 참조 변수 (SmoothDamp)
    private Quaternion currentVelocityRot; // 부드러운 회전을 위한 변수 (Slerp)

    void Start()
    {
        player = transform.root;
        mainCamera = Camera.main;
        if (player == null)
        {
            Debug.LogError("플레이어 Transform이 할당되지 않았습니다. 스크립트를 비활성화합니다.");
            enabled = false;
            return;
        }

        // 초기 시점 설정 (플레이어와 동일한 방향을 보도록 즉시 적용)
        ApplyCameraView(true);
    }



    void LateUpdate()
    {
        if (player == null || mainCamera == null) return;

        ApplyCameraView(false);
    }

    void ApplyCameraView(bool immediate)
    {
        // 목표 바라보기 지점 (플레이어 로컬 좌표 -> 월드 좌표)
        Vector3 lookAtTargetWorldPosition = player.position + player.TransformDirection(thirdPersonLookAtOffset);

        if (isFirstPersonView)
        {
            // 1인칭 카메라 위치 (플레이어 로컬 좌표 -> 월드 좌표)
            // player.TransformDirection은 방향만 변환하므로, 위치 계산은 player.position을 기준으로 함
            Vector3 desiredPosition = player.position +
                                      player.right * firstPersonOffset.x +
                                      player.up * firstPersonOffset.y +
                                      player.forward * firstPersonOffset.z;

            // 1인칭 카메라는 플레이어의 회전을 그대로 따름
            Quaternion desiredRotation = player.rotation;

            if (immediate)
            {
                mainCamera.transform.position = desiredPosition;
                mainCamera.transform.rotation = desiredRotation;
            }
            else
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
                mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, desiredRotation, Time.deltaTime * smoothSpeed);
            }
        }
        else // 3인칭 시점
        {
            // 3인칭 카메라 희망 위치 계산
            // 플레이어의 '앞쪽' 방향을 기준으로 뒤로 물러나고, '월드 위쪽'으로 높이를 조절
            Vector3 offsetDirection = -player.forward * thirdPersonDistance + Vector3.up * thirdPersonHeightOffset;
            Vector3 desiredPosition = lookAtTargetWorldPosition + offsetDirection;

            if (immediate)
            {
                mainCamera.transform.position = desiredPosition;
            }
            else
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            }

            // 항상 목표 지점을 바라보도록 설정
            // 부드러운 LookAt을 원하면 Quaternion.LookRotation과 Slerp를 사용할 수 있으나,
            // 여기서는 즉시 바라보도록 하여 카메라 떨림을 방지할 수 있음.
            // LookAt 이후에 위치를 Lerp하면 흔들릴 수 있으므로, 위치 Lerp 후 LookAt.
            mainCamera.transform.LookAt(lookAtTargetWorldPosition);
        }
    }

    /// <summary>
    /// 외부에서 카메라 시점을 강제로 변경할 때 사용합니다.
    /// </summary>
    /// <param name="switchToFirstPerson">true면 1인칭, false면 3인칭</param>
    public void ForceSetView(bool switchToFirstPerson)
    {
        isFirstPersonView = switchToFirstPerson;
        ApplyCameraView(true); // 즉시 적용
    }

    public void OnViewChange(InputAction.CallbackContext context)
    {
        isFirstPersonView = !isFirstPersonView;
    }
}
