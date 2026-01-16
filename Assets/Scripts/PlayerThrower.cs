using UnityEngine;

public class PlayerThrower : PlayerHandState
{
    [SerializeField] private Transform throwPosition;
    GameObject equippedBomb;
    private FireBomb currentBombData;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    public void Equip(FireBomb fireBombData)
    {
        currentBombData = fireBombData;

    }



    public void UnEquip()
    {
        animator.SetBool("IsThrowing", false);
    }

    private void UpdateUI()
    {

    }


    public override void Enter()
    {


        base.Enter();
        equippedBomb = Instantiate(currentBombData.FireBombPrefab, throwPosition);
        equippedBomb.transform.localPosition = Vector3.zero;
        equippedBomb.transform.localRotation = Quaternion.identity;
        equippedBomb.GetComponent<ItemEffect>().EffectOff();

        animator.SetBool("IsThrowing", true);

    }

    public override void HandleInput()
    {
        base.HandleInput();


        //gun slot 에 해당하는번호가눌렷을때 change state해서그번호에해당하는 gun slot 으로 이동

        //대신에 위에조건쓸려면 이제 gun slot의 번호를 한번쓰고 이제 폭탄상태로바꿀때 다시 -1로해주는로직이필요함 

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
