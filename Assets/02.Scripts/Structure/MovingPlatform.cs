using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Movement")]
    public Transform pointA; // 시작 지점
    public Transform pointB; // 도착 지점
    public float speed = 2f; // 이동 속도
    public float waitTime = 1f; // 멈추는 시간

    private Vector3 targetPosition;
    private bool movingToB = true; // 현재 이동 방향

    void Start()
    {
        targetPosition = pointB.position; // 초기 타겟을 B로 설정
        StartCoroutine(MovePlatform());
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            // 플랫폼 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 목표 지점에 도착하면 대기 후 방향 변경
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                yield return new WaitForSeconds(waitTime);
                movingToB = !movingToB;
                targetPosition = movingToB ? pointB.position : pointA.position;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform); // 플레이어를 플랫폼의 자식으로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); // 플레이어를 다시 원래 상태로
        }
    }
}
