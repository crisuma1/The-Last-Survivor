using UnityEngine;

public class PlayerThrower : MonoBehaviour
{
    [SerializeField] private Transform throwPosition;
    GameObject equippedBomb;
    private Animator playAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playAnimator = GetComponent<Animator>();
    }

    public void Equip(FireBomb fireBombData)
    {


        equippedBomb = Instantiate(fireBombData.FireBombPrefab, throwPosition);
        equippedBomb.transform.localPosition = Vector3.zero;
        equippedBomb.transform.localRotation = Quaternion.identity;
        equippedBomb.GetComponent<ItemEffect>().EffectOff();

        playAnimator.SetTrigger("ThrowingIdle");
    }

    public void Throw()
    {

    }

    public void UnEquip()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
