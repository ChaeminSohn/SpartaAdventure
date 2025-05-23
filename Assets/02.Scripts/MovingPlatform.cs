using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("경로 설정")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [Header("움직임 설정")]
    [SerializeField] float speed = 3.0f;    //이동 속도
    [SerializeField] float arrivalThreshold = 0.1f;     //각 경우 지점에 도달했다고 판단하는 거리
    [SerializeField] float waitTimeAtWaypoint = 1.0f;   //각 경우 지점에 도달 후 대기하는 시간

    private int currentWaypointIndex = 0;
    private Transform platformTransform;
    private float currentWaitTimer = 0f;
    private bool isWaiting = false;

    private void Start()
    {
        platformTransform = transform;

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("경유 지점)이 설정되지 않았습니다! 발판이 움직이지 않습니다.", this);
            enabled = false; // 스크립트 비활성화
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

        if(targetWaypoint == null)  //리스트 중간에 null이 있는 경우 예외처리
        {
            Debug.LogWarning($"경유 지점 리스트의 {currentWaypointIndex}번 인덱스가 비어있습니다. 다음 지점으로 건너<0xEB><0x9B><0x89>니다.", this);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            targetWaypoint = waypoints[currentWaypointIndex]; // 새 목표 설정
            if (targetWaypoint == null)
            { // 새 목표도 null이면 정지
                Debug.LogError("연속된 경유 지점이 비어있어 발판을 정지합니다.", this);
                enabled = false;
                return;
            }
        }

        Vector3 targetPosition = targetWaypoint.position;

        platformTransform.position =
            Vector3.MoveTowards(platformTransform.position, targetPosition, speed * Time.deltaTime);

        if(Vector3.Distance(platformTransform.position, targetPosition) < arrivalThreshold)
        {
            //정확한 위치로 스냅
            platformTransform.position = targetPosition;

            if(waitTimeAtWaypoint > 0)  //대기 시간이 있는 경우
            {
                isWaiting = true;
                currentWaitTimer = waitTimeAtWaypoint;
            }
            else   //대기 시간이 없으면 즉시 다음 경우 지점으로 이동
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }
        }
    }

}
