using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandStateController : MonoBehaviour
{
    Dictionary<HandStateType, PlayerHandState> states;

    public PlayerHandState CurrentState { get; private set; }

    public PlayerShooter shooterState;
    public PlayerThrower throwState;
    public Animator Animator { get; private set; }
    public PlayerInput Input { get; private set; }

    public event Action<PlayerHandState> OnStateChanged;

    // Start is called before the first frame update

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Input = GetComponent<PlayerInput>();

        states = new Dictionary<HandStateType, PlayerHandState>();


        foreach (var state in GetComponentsInChildren<PlayerHandState>())
        {
            state.Init(this);
            states.Add(state.StateType, state);
        }

    }

    void Start()
    {
        ChangeState(HandStateType.Shooter);

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(CurrentState.ToString());
        CurrentState?.HandleInput();
    }

    public void RequestState(HandStateType type)
    {
        if (!states.ContainsKey(type)) return;
        ChangeState(type);

    }

    public void ChangeState(HandStateType type)
    {
        var next = states[type];
        if (CurrentState == next) return;


        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();

        OnStateChanged?.Invoke(CurrentState);
        Debug.Log("changeState");
    }




}
