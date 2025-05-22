using UnityEngine;
using UnityEngine.InputSystem;

public class CameraCtrl : MonoBehaviour
{
    private Transform playerBody; // �÷��̾� ĳ������ Transform (Y�� ȸ�� ��ü)

    [Header("���콺 �� ȸ�� ����")]
    [SerializeField] private float mouseSensitivity = 200f;
    [SerializeField] private float minPitch = -80f; // �ּ� ���� ����
    [SerializeField] private float maxPitch = 80f;  // �ִ� ���� ����

    [Header("1��Ī ���� ����")]
    [Tooltip("firstPersonMountPoint�κ����� ���� ��ġ ������. MountPoint�� �Ӹ���� (0,0,0)�� ������ �����մϴ�.")]
    [SerializeField] private Vector3 firstPersonLocalPosition = new Vector3(0f, 0f, 0f); // MountPoint ���� ��ġ
    [SerializeField] private LayerMask firstPersonLayer;
    private Transform firstPersonParent;
    

    [Header("3��Ī ���� ����")]
    [Tooltip("�÷��̾��� ��� ������ �������� ī�޶� ��ġ���� (�÷��̾� ���� ��ǥ ������)")]
    [SerializeField] private Vector3 thirdPersonTargetOffset = new Vector3(0f, 1.2f, 0f);
    [SerializeField] private float thirdPersonDistance = 3.5f;
    [SerializeField] private LayerMask thirdPersonLayer;
    // 3��Ī �� Pitch�� ���콺 �Է��� �״�� ���. ���� Height ���� ��� Pitch�� ����.


    private float yaw = 0.0f;   // ���� Y�� ȸ���� (�¿�)
    private float pitch = 0.0f; // ���� X�� ȸ���� (����)
    private bool isFirstPerson = true;
    private Vector2 mouseDelta;
    private Camera cam;

    void Start()
    {
        playerBody = transform.root;
        firstPersonParent = transform.parent;
        cam = GetComponent<Camera>(); // �� ��ũ��Ʈ�� ī�޶� ������Ʈ�� �־�� �մϴ�.

        if (playerBody == null)
        {
            Debug.LogError("Player Body Transform�� �Ҵ���� �ʾҽ��ϴ�!");
            enabled = false;
            return;
        }

        // �ʱ� Yaw�� �÷��̾��� ���� Y�� ȸ������ ����
        yaw = playerBody.eulerAngles.y;
        // �ʱ� Pitch�� ���� ī�޶��� X�� ȸ������ �����ų� 0���� ����
        // (ī�޶� �ʱ⿡ Ư�� ������ ������ �ִٸ� �ش� ������, �ƴϸ� 0)
        pitch = transform.localEulerAngles.x;
        if (pitch > 180) pitch -= 360; // ������ -180 ~ 180 ������ ��ȯ

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        ApplyCameraView(); // �ʱ� ���� ����
    }

    void Update()
    {
        yaw += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        pitch -= mouseDelta.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // Pitch �� ���� ����
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // Pitch �� ���� ����
    }

    void LateUpdate()
    {
        // LateUpdate���� ī�޶� ��ġ/ȸ�� ���� ���� (�÷��̾� �̵� ��)
        ApplyCameraView();
    }

    void ApplyCameraView()
    {
        // �÷��̾� ��ü Y�� ȸ�� ���� (1��Ī, 3��Ī ����)
        playerBody.rotation = Quaternion.Euler(0, yaw, 0);

        if (isFirstPerson)
        {
            // 1��Ī ���� 
            transform.parent = firstPersonParent;
            transform.localPosition = firstPersonLocalPosition;
            transform.localRotation = Quaternion.Euler(pitch, 0, 0); // ���� X�� ȸ���� Pitch ����
        }
        else
        {
            //  3��Ī ���� 
            if (transform.parent != null)
            {
                transform.SetParent(null); // ���� �������� ��ġ �����ϱ� ���� �θ� ����
            }

            // ī�޶� �ٶ� ��ǥ ���� (�÷��̾� ���� ������ -> ���� ��ǥ)
            Vector3 lookAtTargetWorld = playerBody.position + playerBody.TransformDirection(thirdPersonTargetOffset);

            // ��ǥ ī�޶� ȸ�� (���콺 Yaw�� Pitch ��� ���)
            Quaternion desiredRotation = Quaternion.Euler(pitch, yaw, 0);

            // ��ǥ ī�޶� ��ġ (��ǥ �������� (ȸ���� * �Ÿ�) ��ŭ �ڷ� �̵�)
            Vector3 desiredPosition = lookAtTargetWorld - (desiredRotation * Vector3.forward * thirdPersonDistance);

            transform.position = desiredPosition;
            transform.LookAt(lookAtTargetWorld); // �׻� ��ǥ ������ �ٶ󺸵��� ����
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