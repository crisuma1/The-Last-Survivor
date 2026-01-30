using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // 어떤 아이템이든 드래그할 수 있게 ScriptableObject로 받음
    public ScriptableObject itemAsset;
    public int quantity = 1;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered!");

        if (!other.CompareTag("Player")) return;

        var inventory = other.GetComponent<Inventory>();
        var ui = FindObjectOfType<InventoryUI>();
        var ph = other.GetComponent<PlayerHealth>();
        if (inventory == null) return;

        // SO 인스턴스 복제 후 IItem으로 캐스팅
        var soInstance = ScriptableObject.Instantiate(itemAsset);
        var newItem = soInstance as IItem;
        if (newItem == null)
        {
            Debug.LogError($"[ItemPickup] {itemAsset.name} 은(는) IItem을 구현하지 않았습니다.");
            return;
        }

        newItem.Quantity = quantity;

        // 빈 슬롯에 넣기 (0 → 9)
        for (int i = 0; i < inventory.itemSlots.Length; i++)
        {
            if (inventory.itemSlots[i] == null)
            {
                inventory.SetItem(i, newItem);
                ui?.RefreshSlot(i);
                ph.PlayPickupSfx();
                break;
            }
        }

        Destroy(gameObject);
    }
}
