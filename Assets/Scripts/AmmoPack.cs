using UnityEngine;

// 총알을 충전하는 아이템
[CreateAssetMenu(menuName = "Items/AmmoPack", fileName = "New Ammo Pack")]
public class AmmoPack : ScriptableObject, IItem
{

    [SerializeField] private string itemName = "Ammo Pack"; // 아이템 이름
    [SerializeField] private Sprite icon; // 아이템 아이콘
    [SerializeField] private int quantity = 1; // 아이템 수량
    [SerializeField] private int ammoAmount = 30;

    public string Name => itemName; // 아이템 이름 프로퍼티
    public Sprite Icon => icon; // 아이템 아이콘 프로퍼티
    public int Quantity { get => quantity; set => quantity = value; } // 아이템 수량 프로퍼티
    public int AmmoAmount => ammoAmount; // 아이템이 충전할 총알 수 프로퍼티





    public void Use(GameObject target)
    {
        // 전달 받은 게임 오브젝트로부터 PlayerShooter 컴포넌트를 가져오기 시도
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        // PlayerShooter 컴포넌트가 있으며, 총 오브젝트가 존재하면
        if (playerShooter != null && playerShooter.CurrentGun != null)
        {
            // 총의 남은 탄환 수를 ammo 만큼 더합니다.
            playerShooter.CurrentGun.ammoRemain += ammoAmount;
        }

        Quantity--;

    }
}