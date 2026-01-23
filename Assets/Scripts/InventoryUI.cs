using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject player;

    [System.Serializable]
    public class SlotUI
    {
        public Image iconImage;
        public TextMeshProUGUI quantityText;
    }

    public SlotUI[] slots = new SlotUI[10];  // slot0 ~ slot9

    void Start()
    {
        RefreshAll();
    }

    public void RefreshSlot(int index)
    {
        var item = inventory.itemSlots[index];
        if (item != null && item.Quantity > 0)
        {
            slots[index].iconImage.sprite = item.Icon;
            slots[index].iconImage.enabled = true;
            slots[index].quantityText.text = item.Quantity.ToString();
        }
        else
        {
            slots[index].iconImage.enabled = false;
            slots[index].quantityText.text = "";
        }
    }

    public void RefreshAll()
    {
        for (int i = 0; i < 10; i++)
        {
            RefreshSlot(i);
        }
    }
}
