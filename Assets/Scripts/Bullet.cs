using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30.0f; // �Ѿ� �ӵ�
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Rigidbody�� �̿��� �̵� ó��
            rb.velocity = transform.forward * speed;

         
        }
    }

    void Update()
    {
        // Rigidbody�� ���� ���, �����Ӹ��� �̵� ó��
        if (rb == null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

          
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �÷��̾ �ƴ� ��� �Ѿ� ����
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
