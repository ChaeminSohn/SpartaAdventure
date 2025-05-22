using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchableViewCamera : MonoBehaviour
{
    private Transform player;

    [Header("1��Ī ���� ����")]
    public Vector3 firstPersonOffset = new Vector3(0f, 0.7f, 0.2f); // ��: �÷��̾� �� ��ġ

    [Header("3��Ī ���� ����")]
    public Vector3 thirdPersonLookAtOffset = new Vector3(0f, 1.2f, 0f); // ��: �÷��̾� ���� ����
    public float thirdPersonDistance = 4.0f;
    public float thirdPersonHeightOffset = 1.0f; // thirdPersonLookAtOffset �������� �߰� ����

    [Header("ī�޶� ������")]
    public float smoothSpeed = 10f;

    private Camera mainCamera;
    private bool isFirstPersonView = true;
    private Vector3 currentVelocityPos; // �ε巯�� �̵��� ���� ���� ���� (SmoothDamp)
    private Quaternion currentVelocityRot; // �ε巯�� ȸ���� ���� ���� (Slerp)

    void Start()
    {
        player = transform.root;
        mainCamera = Camera.main;
        if (player == null)
        {
            Debug.LogError("�÷��̾� Transform�� �Ҵ���� �ʾҽ��ϴ�. ��ũ��Ʈ�� ��Ȱ��ȭ�մϴ�.");
            enabled = false;
            return;
        }

        // �ʱ� ���� ���� (�÷��̾�� ������ ������ ������ ��� ����)
        ApplyCameraView(true);
    }



    void LateUpdate()
    {
        if (player == null || mainCamera == null) return;

        ApplyCameraView(false);
    }

    void ApplyCameraView(bool immediate)
    {
        // ��ǥ �ٶ󺸱� ���� (�÷��̾� ���� ��ǥ -> ���� ��ǥ)
        Vector3 lookAtTargetWorldPosition = player.position + player.TransformDirection(thirdPersonLookAtOffset);

        if (isFirstPersonView)
        {
            // 1��Ī ī�޶� ��ġ (�÷��̾� ���� ��ǥ -> ���� ��ǥ)
            // player.TransformDirection�� ���⸸ ��ȯ�ϹǷ�, ��ġ ����� player.position�� �������� ��
            Vector3 desiredPosition = player.position +
                                      player.right * firstPersonOffset.x +
                                      player.up * firstPersonOffset.y +
                                      player.forward * firstPersonOffset.z;

            // 1��Ī ī�޶�� �÷��̾��� ȸ���� �״�� ����
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
        else // 3��Ī ����
        {
            // 3��Ī ī�޶� ��� ��ġ ���
            // �÷��̾��� '����' ������ �������� �ڷ� ��������, '���� ����'���� ���̸� ����
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

            // �׻� ��ǥ ������ �ٶ󺸵��� ����
            // �ε巯�� LookAt�� ���ϸ� Quaternion.LookRotation�� Slerp�� ����� �� ������,
            // ���⼭�� ��� �ٶ󺸵��� �Ͽ� ī�޶� ������ ������ �� ����.
            // LookAt ���Ŀ� ��ġ�� Lerp�ϸ� ��鸱 �� �����Ƿ�, ��ġ Lerp �� LookAt.
            mainCamera.transform.LookAt(lookAtTargetWorldPosition);
        }
    }

    /// <summary>
    /// �ܺο��� ī�޶� ������ ������ ������ �� ����մϴ�.
    /// </summary>
    /// <param name="switchToFirstPerson">true�� 1��Ī, false�� 3��Ī</param>
    public void ForceSetView(bool switchToFirstPerson)
    {
        isFirstPersonView = switchToFirstPerson;
        ApplyCameraView(true); // ��� ����
    }

    public void OnViewChange(InputAction.CallbackContext context)
    {
        isFirstPersonView = !isFirstPersonView;
    }
}
