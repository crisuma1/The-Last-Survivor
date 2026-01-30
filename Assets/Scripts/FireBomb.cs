using UnityEngine;
[CreateAssetMenu(menuName = "Items/FireBomb", fileName = "New FireBomb")]
public class FireBomb : ScriptableObject, IItem
{
    [SerializeField] private string itemName = "FireBomb"; // 아이템 이름
    [SerializeField] private Sprite icon; // 아이템 아이콘
    [SerializeField] private int quantity = 1; // 아이템 수량
    [SerializeField] public GameObject FireBombPrefab;//실체가필요한아이템이라 프리팹가져옴

    public string Name => itemName; // 아이템 이름 프로퍼티
    public Sprite Icon => icon; // 아이템 아이콘 프로퍼티
    public int Quantity { get => quantity; set => quantity = value; } // 아이템 수량 프로퍼티



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Use(GameObject target, int slotIndex)
    {

        var controller = target.GetComponent<PlayerHandStateController>();
        var thrower = controller.throwState;

        thrower.SetData(this, slotIndex);
        controller.RequestState(HandStateType.Thrower);

        //Quantity--;
    }

    public void UseAfterClick(GameObject target, int slotIndex)
    {
        // 전달 받은 게임 오브젝트로부터 Playerthrower 컴포넌트를 가져오기 시도
        PlayerThrower playerThrower = target.GetComponent<PlayerThrower>();

        // Playerthrower 컴포넌트가 있으며, 폭탄 오브젝트가 존재하면
        if (playerThrower != null && playerThrower.equippedBomb != null)
        {
            // 폭탄의개수를줄입니다
            Quantity--;
        }


    }
}
