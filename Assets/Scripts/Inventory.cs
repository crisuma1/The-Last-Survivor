using UnityEngine;

public class Inventory : MonoBehaviour
{
    public IItem[] itemSlots = new IItem[10];
    // Start is called before the first frame update
    public void UseItemAtSlot(int index, GameObject player)
    {
        if (index < 0 || index >= itemSlots.Length) return;




        IItem item = itemSlots[index];
        if (item != null && item.Quantity > 0)
        {
            item.Use(player, index);

            if (item.Quantity <= 0)
            {
                itemSlots[index] = null;
            }
        }



    }


    public void SetItem(int index, IItem item)
    {
        itemSlots[index] = item;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
