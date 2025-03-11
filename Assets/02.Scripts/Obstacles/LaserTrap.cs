using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : MonoBehaviour
{
    [Header("Laser")]
    public Transform laserStart; 
    public float laserRange = 10f; 
    public LayerMask detectionLayer; 
    public bool isActive = true; 
    public float toggleInterval = 3f; 

    [Header("Trap")]
    public int damage = 20;
    public Arrow[] arrows;
    public Vector3[] arrowPosition;
    public Rigidbody[] arrowRigidbodies;
    public float arrowSpeed = 30f;

    private void Start()
    {
        arrows = GetComponentsInChildren<Arrow>();
        arrowPosition = new Vector3[arrows.Length];
        arrowRigidbodies = new Rigidbody[arrows.Length];

        for (int i = 0; i < arrows.Length; i++)
        {
            arrowRigidbodies[i] = arrows[i].GetComponent<Rigidbody>();
            arrowPosition[i] = arrows[i].transform.position; // 각 Arrow의 위치 저장
        }

        StartCoroutine(ToggleLaser()); // 일정 간격으로 레이저 활성/비활성
    }

    void Update()
    {
        if (isActive)
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        RaycastHit hit;
        Vector3 laserDirection = transform.forward;

        if (Physics.Raycast(laserStart.position, laserDirection, out hit, laserRange, detectionLayer))
        {
            //Debug.DrawRay(laserStart.position, laserDirection * hit.distance, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                for (int i = 0; i < arrows.Length; i++)
                {
                    arrowRigidbodies[i].AddForce(laserDirection*arrowSpeed, ForceMode.Impulse);
                    arrowRigidbodies[i].useGravity = true;
                }

                Invoke("ResetArrow", 3f);
            }
        }
        else
        {
            Debug.DrawRay(laserStart.position, laserDirection * laserRange, Color.green);
        }
    }

    void ResetArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrowRigidbodies[i].useGravity = false;
            arrowRigidbodies[i].velocity = Vector3.zero;
            arrows[i].transform.position = arrowPosition[i];
        }
    }

    IEnumerator ToggleLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(toggleInterval);
            isActive = !isActive;
        }
    }
}
