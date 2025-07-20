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
        base.Die(); // onDeath �̺�Ʈ ȣ�� + dead = true ����
        if (destroyeffect != null)
        {
            destroyeffect.SetActive(true);
        }


        Destroy(gameObject); // ��Ż ������Ʈ �ı�
                        
  

    }
}
