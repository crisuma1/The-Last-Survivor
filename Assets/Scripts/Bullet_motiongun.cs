using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_motiongun : MonoBehaviour
{
    public float motiongunDamage = 10f; // �Ѿ��� �ִ� ������

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �÷��̾ �ƴ� ��� �Ѿ� ����
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
        // �÷��̾�� �浹���� ���
        else if (collision.gameObject.tag == "Player")
        {
            // �÷��̾��� LivingEntity ������Ʈ�� ������
            LivingEntity player = collision.gameObject.GetComponent<LivingEntity>();
            if (player != null)
            {
                // �÷��̾�� 10�� �������� ��
                player.OnDamage(motiongunDamage, transform.position, Vector3.forward);
            }
            // �Ѿ� ����
            Destroy(this.gameObject);
        }
    }
}
