using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 20;
    public float power = 100f;

    public void OnChildTrigger(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Player"))
        {
            // 안전한 방식으로 Player 컴포넌트 가져오기
            Player player = other.GetComponent<Collider>().GetComponent<Player>();
            if (player != null)
            {
                // 플레이어 데미지 적용
                player.condition.TakePhysicalDamage(damage);

                // Rigidbody 적용 (안전한 방식)
                Rigidbody rb = player.controller._rigidbody;
                if (rb != null)
                {
                    // 충돌 방향 기반으로 튕겨나가기
                    Vector3 knockbackDirection = -(player.transform.forward) + Vector3.up;
                    rb.AddForce(knockbackDirection * power, ForceMode.Impulse);
                }
            }
        }
    }
}
