using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public HealthPack itemAsset;
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            InventoryUI ui = FindObjectOfType<InventoryUI>();

            if (inventory != null)
            {
                // 아이템 복사 후 수량 지정
                HealthPack newItem = ScriptableObject.Instantiate(itemAsset);
                newItem.Quantity = quantity;

                // 빈 슬롯 찾아서 등록
                for (int i = 0; i < inventory.itemSlots.Length; i++)
                {
                    if (inventory.itemSlots[i] == null)
                    {
                        inventory.SetItem(i, newItem);
                        ui.RefreshSlot(i);
                        break;
                    }
                }

                Destroy(gameObject); // 아이템 오브젝트 제거
            }
        }
    }
}
