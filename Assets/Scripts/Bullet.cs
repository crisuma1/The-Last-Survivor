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

        Destroy(gameObject, 7f);
    }

    void Update()
    {
        // Rigidbody가 없는 경우, 프레임마다 이동 처리
        if (rb == null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

         
        }
    }

   private void OnTriggerEnter(Collider other)
{
   // Debug.Log("충돌한 오브젝트: " + other.gameObject.name);

    // 플레이어는 무시
    if (other.CompareTag("Player")) return;

    Destroy(gameObject);
}
}
