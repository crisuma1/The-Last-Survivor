using UnityEngine;

public enum HandStateType { Shooter, Thrower, Melee }

public abstract class PlayerHandState : MonoBehaviour
{
    public abstract HandStateType StateType { get; }
    protected Animator animator;
    protected PlayerInput input;
    protected PlayerHandStateController statecontroller;
    protected Inventory inventory;
    protected InventoryUI inventoryUI;

    protected virtual void Awake()
    {

    }


    public virtual void Init(PlayerHandStateController c)
    {
        statecontroller = c;
        animator = c.Animator;
        input = c.Input;
        inventory = c.Inventory;
        inventoryUI = c.InventoryUI;

    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput()
    {
        // Debug.Log(statecontroller.InputLock);
        if (input.itemSlot >= 0)
        {
            // 어떤입력락이라도걸려있으면 
            if (statecontroller.IsAnyLocked())
            {
                input.InitItemSlot();
                return;
            }


            //입력잠그기
            statecontroller.Lock(InputLockType.UseItem);

            inventory.UseItemAtSlot(input.itemSlot, statecontroller.gameObject);
            inventoryUI.RefreshSlot(input.itemSlot);

            //입력잠그기해재
            statecontroller.Unlock(InputLockType.UseItem);

            input.InitItemSlot();
        }


    }



}
