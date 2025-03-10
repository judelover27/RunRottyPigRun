using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 20;
    public float power = 20f;

    public void OnChildCollision(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 안전한 방식으로 Player 컴포넌트 가져오기
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                // 플레이어 데미지 적용
                player.condition.TakePhysicalDamage(damage);

                // Rigidbody 적용 (안전한 방식)
                Rigidbody rb = player.controller._rigidbody;
                if (rb != null)
                {
                    // 충돌 방향 기반으로 튕겨나가기
                    Vector3 knockbackDirection = collision.contacts[0].normal + Vector3.up;
                    rb.AddForce(knockbackDirection * power, ForceMode.Impulse);
                }
            }
        }
    }
}
