using UnityEngine;

// 체력을 회복하는 아이템
[CreateAssetMenu(menuName = "Items/HealthPack", fileName = "New Health Pack")]
public class HealthPack : ScriptableObject, IItem
{
    [SerializeField] private string itemName = "HealPotion";
    [SerializeField] private Sprite icon;
    [SerializeField] private int quantity = 1;
    [SerializeField] private int healAmount = 50;

    public string Name => itemName;
    public Sprite Icon => icon;
    public int Quantity { get => quantity; set => quantity = value; }



    public void Use(GameObject target, int slotIndex)
    {
        // 전달받은 게임 오브젝트로부터 LivingEntity 컴포넌트 가져오기 시도
        LivingEntity life = target.GetComponent<LivingEntity>();

        // LivingEntity컴포넌트가 있다면
        if (life != null)
        {
            // 체력 회복 실행
            life.RestoreHealth(healAmount);
        }
        // 사용된 아이템이므로, 수량을 감소시킴
        Quantity--;

    }

    public void UseAfterClick(GameObject target, int slotIndex)
    {

    }
}