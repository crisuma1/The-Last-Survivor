using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ZombieLeftHandHitbox : MonoBehaviour
{
   
    private bool isHitBoxActive; //히트박스가켜져있는지

    private bool hasDamaged; // 이번 공격에서 데미지 줬는지

    [SerializeField]private Zombie zombie;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitBoxOn()
    {
        isHitBoxActive = true;
        hasDamaged = false;
        this.gameObject.SetActive(true);
    }

    public void HitBoxOff()
    {
        isHitBoxActive = false;
        this.gameObject.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            
            Debug.Log("hit");


            // 공격 실행
            if (isHitBoxActive && !hasDamaged)
            {
                hasDamaged = true;

                Debug.Log("Damgedgogo");
                // 상대방의 피격 위치와 피격 방향을 계산
                LivingEntity attackTarget = other.GetComponent<LivingEntity>();
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (transform.position - other.transform.position).normalized;
                attackTarget.OnDamage(zombie.damage, hitPoint, hitNormal);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            
            Debug.Log("out");
        }
    }
}
