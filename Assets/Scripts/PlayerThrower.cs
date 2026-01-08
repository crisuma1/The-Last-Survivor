using UnityEngine;

public class PlayerThrower : MonoBehaviour
{
    [SerializeField] private Transform throwPosition;
    GameObject equippedBomb;
    private Animator playAnimator;

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트


    // Start is called before the first frame update
    void Start()
    {
        playAnimator = GetComponent<Animator>();
    }

    public void Equip(FireBomb fireBombData)
    {
        PlayerWeaponState.SetMode(WeaponMode.Throw);


        //슈터 비활성화
        GetComponent<PlayerShooter>().enabled = false;

        //총비활성화
        GetComponent<PlayerShooter>().CurrentGun.gameObject.SetActive(false);

        equippedBomb = Instantiate(fireBombData.FireBombPrefab, throwPosition);
        equippedBomb.transform.localPosition = Vector3.zero;
        equippedBomb.transform.localRotation = Quaternion.identity;
        equippedBomb.GetComponent<ItemEffect>().EffectOff();

        playAnimator.SetBool("IsThrowing", true);
    }

    public void Throw()
    {

        UpdateUI();
    }

    public void UnEquip()
    {
        PlayerWeaponState.SetMode(WeaponMode.Gun);

        // 2. 슈터 다시 활성화
        var shooter = GetComponent<PlayerShooter>();
        shooter.enabled = true;

        // 3. 총 다시 활성화
        shooter.CurrentGun.gameObject.SetActive(true);

        playAnimator.SetBool("IsThrowing", false);
    }

    private void UpdateUI()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            UnEquip();

    }
}
