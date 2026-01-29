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

        //Debug.Log(equippedSlotIndex == slotIndex);


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


        //Debug.Log(equippedBomb != null);
    }



    public void UnEquip()
    {
        animator.SetTrigger("ThrowingToIdle");
        if (equippedBomb != null)
            equippedBomb.SetActive(false);
    }

    public void Equip()
    {
        animator.SetTrigger("IsThrowing");
        equippedBomb.SetActive(true);
        Debug.Log(equippedBomb != null);
        //equippedBomb.AddComponent<Rigidbody>();

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

        //Debug.Log(equippedBomb != null);


        if (input.fireDown)
        {

            // 어떤입력락이라도걸려있으면 
            if (statecontroller.IsAnyLocked())
                return;

            //입력잠그기
            statecontroller.Lock(InputLockType.Throw);
            // animator.SetBool("IsThrowing", false);

            animator.SetTrigger("Throw");


            //Debug.Log("firehall");
        }





        if (input.gunSlot >= 0)
        {
            // 어떤입력락이라도걸려있으면 
            if (statecontroller.IsAnyLocked())
            {
                statecontroller.Input.InitGunSlot();
                return;
            }


            statecontroller.RequestState(HandStateType.Shooter);

        }



    }

    public void Throw()
    {

        UpdateUI();
    }

    public void ReleasedBomb()
    {
        equippedBomb.transform.SetParent(null);

        Rigidbody rb = equippedBomb.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        Vector3 forward = statecontroller.cameraPivot.transform.forward;

        // 위를 볼수록 upPower 감소
        float upPower = Mathf.Lerp(6f, 2f, Mathf.Clamp01(forward.y));

        Vector3 throwDir =
            forward * 14f +
            Vector3.up * upPower;

        rb.AddForce(throwDir, ForceMode.VelocityChange);


        equippedBomb = null;
    }

    public override void Exit()
    {
        base.Exit();
        UnEquip();


    }

}
