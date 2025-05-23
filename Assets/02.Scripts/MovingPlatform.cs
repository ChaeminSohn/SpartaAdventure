using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("��� ����")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [Header("������ ����")]
    [SerializeField] float speed = 3.0f;    //�̵� �ӵ�
    [SerializeField] float arrivalThreshold = 0.1f;     //�� ��� ������ �����ߴٰ� �Ǵ��ϴ� �Ÿ�
    [SerializeField] float waitTimeAtWaypoint = 1.0f;   //�� ��� ������ ���� �� ����ϴ� �ð�

    private int currentWaypointIndex = 0;
    private Transform platformTransform;
    private float currentWaitTimer = 0f;
    private bool isWaiting = false;

    private void Start()
    {
        platformTransform = transform;

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("���� ����)�� �������� �ʾҽ��ϴ�! ������ �������� �ʽ��ϴ�.", this);
            enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        platformTransform.position = waypoints[0].position;
    }

    private void Update()
    {
        if (isWaiting)
        {
            currentWaitTimer -= Time.deltaTime;
            if(currentWaitTimer <= 0) 
            {
                isWaiting = false;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
            else
            {
                return;
            }
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        if(targetWaypoint == null)  //����Ʈ �߰��� null�� �ִ� ��� ����ó��
        {
            Debug.LogWarning($"���� ���� ����Ʈ�� {currentWaypointIndex}�� �ε����� ����ֽ��ϴ�. ���� �������� �ǳ�<0xEB><0x9B><0x89>�ϴ�.", this);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetWaypoint = waypoints[currentWaypointIndex]; // �� ��ǥ ����
            if (targetWaypoint == null)
            { // �� ��ǥ�� null�̸� ����
                Debug.LogError("���ӵ� ���� ������ ����־� ������ �����մϴ�.", this);
                enabled = false;
                return;
            }
        }

        Vector3 targetPosition = targetWaypoint.position;

        platformTransform.position =
            Vector3.MoveTowards(platformTransform.position, targetPosition, speed * Time.deltaTime);

        if(Vector3.Distance(platformTransform.position, targetPosition) < arrivalThreshold)
        {
            //��Ȯ�� ��ġ�� ����
            platformTransform.position = targetPosition;

            if(waitTimeAtWaypoint > 0)  //��� �ð��� �ִ� ���
            {
                isWaiting = true;
                currentWaitTimer = waitTimeAtWaypoint;
            }
            else   //��� �ð��� ������ ��� ���� ��� �������� �̵�
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
        }
    }

}
