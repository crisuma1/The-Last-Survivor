using System.Collections;
using System.Collections.Generic;
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
        public Text quantityText;
    }

    public SlotUI[] slots = new SlotUI[10];  // slot0 ~ slot9

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((i == 9) ? KeyCode.Alpha0 : (KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                inventory.UseItemAtSlot(i, player);
                RefreshSlot(i);
            }
        }
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
