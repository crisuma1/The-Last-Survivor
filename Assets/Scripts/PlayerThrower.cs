using UnityEngine;

public class PlayerThrower : PlayerHandState
{
    public override HandStateType StateType => HandStateType.Thrower;
    [SerializeField] private Transform throwPosition;
    GameObject equippedBomb;
    private FireBomb currentBombData;
    private int equippedSlotIndex = -1; //슬롯의인덱스정보
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetData(FireBomb fireBombData, int slotIndex)
    {
        bool isActive = (statecontroller.CurrentState == this);

        // 같은 슬롯을 다시 누른 경우 -> 아무 것도 안 함
        if (equippedSlotIndex == slotIndex && equippedBomb != null)
            return;

        equippedSlotIndex = slotIndex;
        currentBombData = fireBombData;

        // 기존 폭탄 제거
        if (equippedBomb != null)
        {
            Destroy(equippedBomb);
            equippedBomb = null;
        }

        // 새 폭탄 생성
        equippedBomb = Instantiate(currentBombData.FireBombPrefab, throwPosition);
        equippedBomb.transform.localPosition = Vector3.zero;
        equippedBomb.transform.localRotation = Quaternion.identity;
        equippedBomb.GetComponent<ItemEffect>()?.EffectOff();
        equippedBomb.SetActive(false);

        // 이미 Thrower 상태면 즉시 보여주기
        if (isActive)
            Equip();
    }



    public void UnEquip()
    {
        //animator.SetBool("IsThrowing", false);
        equippedBomb.SetActive(false);
    }

    public void Equip()
    {
        animator.SetTrigger("IsThrowing"); ;
        equippedBomb.SetActive(true);
        Debug.Log("Equipped");
    }

    private void UpdateUI()
    {

    }


    public override void Enter()
    {


        base.Enter();
        Debug.Log("bombGet");
        if (equippedBomb != null)
        {
            Equip();
        }
        else
        {
            Debug.Log("nobomb");
        }

        
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (input.fire)
        {
           // animator.SetBool("IsThrowing", false);
            
            animator.SetTrigger("Throw");
            Debug.Log("firehall");
        }





        if (input.gunSlot >= 0)
        {
           
            statecontroller.RequestState(HandStateType.Shooter);
            
        }



    }

    public void Throw()
    {

        UpdateUI();
    }

    public override void Exit()
    {
        base.Exit();
        UnEquip();


    }

}
