using UnityEngine;

public class Inventory : MonoBehaviour
{
    public IItem[] itemSlots = new IItem[10];
    private int maxCapacity = 2;
    [SerializeField] private InventoryUI ui;
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
        CompactAndMerge();
    }

    public void UseItemOutofSlot(int index, GameObject player)
    {
        if (index < 0 || index >= itemSlots.Length) return;

        IItem item = itemSlots[index];
        if (item != null && item.Quantity > 0)
        {
            item.UseAfterClick(player, index);

            if (item.Quantity <= 0)
            {
                itemSlots[index] = null;
            }
        }

        CompactAndMerge();
    }



    public void CompactAndMerge()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            //  뒤에서 첫 번째 null 아닌 슬롯 찾아서 i로 이동
            if (itemSlots[i] == null)
            {
                int next = FindNextNonNull(i + 1);
                if (next != -1)
                {
                    itemSlots[i] = itemSlots[next];
                    itemSlots[next] = null;
                    i--; // 바로뒤와병합되는지확인
                }
                continue;
            }

            //  뒤 슬롯들 중 같은 아이템을 찾아 max capacity 허용 범위 내에서 끌어옴
            for (int j = i + 1; j < itemSlots.Length; j++)
            {
                if (itemSlots[j] == null) continue;
                if (itemSlots[j].Name != itemSlots[i].Name) continue;

                int canTake = maxCapacity - itemSlots[i].Quantity;
                if (canTake <= 0) break;

                int take = Mathf.Min(canTake, itemSlots[j].Quantity);
                itemSlots[i].Quantity += take;
                itemSlots[j].Quantity -= take;

                if (itemSlots[j].Quantity <= 0)
                    itemSlots[j] = null;
            }
        }

        ui.RefreshAll();
    }


    private int FindNextNonNull(int start)
    {
        for (int i = start; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                return i;
            }
        }
        return -1;
    }



    public void SetItem(int index, IItem item)
    {
        itemSlots[index] = item;
    }

    public bool TryAddToSlot(int index, IItem newItem)
    {
        IItem item = itemSlots[index];
        if (item == null) return false;
        if (item.Name != newItem.Name) return false;

        if (item.Quantity >= maxCapacity)
            return false;

        item.Quantity++;
        return true;
    }


    //만약아이템의Quantity가 고정값이1개가아닐경우 사용할 아이템획득시 추가및 정렬함수
    public bool AddItem(IItem newItem)
    {
        //  기존 스택에 병합
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null) continue;
            if (itemSlots[i].Name != newItem.Name) continue;

            int canTake = maxCapacity - itemSlots[i].Quantity;
            if (canTake <= 0) continue;

            int take = Mathf.Min(canTake, newItem.Quantity);
            itemSlots[i].Quantity += take;
            newItem.Quantity -= take;

            if (newItem.Quantity <= 0)
                return true;
        }

        //  빈 슬롯에 분배
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null) continue;

            int take = Mathf.Min(maxCapacity, newItem.Quantity);
            var instance = Instantiate(newItem as ScriptableObject) as IItem;

            instance.Quantity = take;

            itemSlots[i] = instance;
            newItem.Quantity -= take;

            if (newItem.Quantity <= 0)
                return true;
        }

        return false; // 인벤토리 가득 참
    }



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
