using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_motiongun : MonoBehaviour
{
    public float motiongunDamage = 10f; // 총알이 주는 데미지

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체가 플레이어가 아닐 경우 총알 제거
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
        // 플레이어와 충돌했을 경우
        else if (collision.gameObject.tag == "Player")
        {
            // 플레이어의 LivingEntity 컴포넌트를 가져옴
            LivingEntity player = collision.gameObject.GetComponent<LivingEntity>();
            if (player != null)
            {
                // 플레이어에게 10의 데미지를 줌
                player.OnDamage(motiongunDamage, transform.position, Vector3.forward);
            }
            // 총알 제거
            Destroy(this.gameObject);
        }
    }
}
