using UnityEngine;

public class portalHealth : LivingEntity
{
    public GameObject destroyeffect;

    protected override void OnEnable()
    {
        base.OnEnable();
        startingHealth = 200f;
    }

    public override void Die()
    {
        base.Die(); // onDeath 이벤트 호출 + dead = true 설정
        if (destroyeffect != null)
        {
            destroyeffect.SetActive(true);
        }


        Destroy(gameObject); // 포탈 오브젝트 파괴
                        
  

    }
}
