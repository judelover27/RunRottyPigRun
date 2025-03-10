using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : StructureObject
{
    private bool isOpen = false; 
    public Transform door; 
    public float openAngle = 90f; 
    public float duration = 1.0f; // 열리는 속도

    private Coroutine doorCoroutine; // 코루틴 저장 (중복 실행 방지)

    public override void OnInteract()
    {
        ToggleOpen();
    }

    public void ToggleOpen()
    {
        if (doorCoroutine != null) StopCoroutine(doorCoroutine); // 기존 코루틴 중지
        doorCoroutine = StartCoroutine(RotateDoor(isOpen ? 0f : openAngle));
        isOpen = !isOpen; // 상태 변경
    }

    IEnumerator RotateDoor(float targetAngle)
    {
        float time = 0f;
        Quaternion startRotation = door.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0); // Y축 기준 회전

        while (time < duration)
        {
            door.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        door.rotation = targetRotation; // 최종 각도 고정
    }
}
