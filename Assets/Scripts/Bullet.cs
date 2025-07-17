using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30.0f; // 총알 속도
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Rigidbody를 이용한 이동 처리
            rb.velocity = transform.forward * speed;

         
        }
    }

    void Update()
    {
        // Rigidbody가 없는 경우, 프레임마다 이동 처리
        if (rb == null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

          
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체가 플레이어가 아닐 경우 총알 제거
        if (collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
