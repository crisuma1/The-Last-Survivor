using UnityEngine;

public enum HandStateType { Shooter, Thrower, Melee }

public abstract class PlayerHandState : MonoBehaviour
{
    public abstract HandStateType StateType { get; }
    protected Animator animator;
    protected PlayerInput input;
    protected PlayerHandStateController statecontroller;

    protected virtual void Awake()
    {

    }


    public virtual void Init(PlayerHandStateController c)
    {
        statecontroller = c;
        animator = c.Animator;
        input = c.Input;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput()
    {

    }



}
