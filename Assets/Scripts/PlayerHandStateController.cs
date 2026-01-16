using System;
using UnityEngine;

public class PlayerHandStateController : MonoBehaviour
{
    public PlayerHandState CurrentState { get; private set; }

    public PlayerShooter shooterState;
    public PlayerThrower throwState;


    public event Action<PlayerHandState> OnStateChanged;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(shooterState);

    }

    // Update is called once per frame
    void Update()
    {
        CurrentState?.HandleInput();
    }

    public void ChangeState(PlayerHandState newState)
    {
        if (CurrentState == newState) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();

        OnStateChanged?.Invoke(CurrentState);
    }

}
