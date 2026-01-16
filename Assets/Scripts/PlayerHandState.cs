using UnityEngine;

public abstract class PlayerHandState : MonoBehaviour
{
    protected Animator animator;
    protected PlayerInput input;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void HandleInput() { }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
