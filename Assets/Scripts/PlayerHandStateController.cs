using System;
using System.Collections.Generic;
using UnityEngine;

public enum InputLockType
{
    None = 0,
    Fire = 1 << 0,
    Reload = 1 << 1,
    UseItem = 1 << 2,
    WeaponChange = 1 << 3,
    Throw = 1 << 4,
}


public class PlayerHandStateController : MonoBehaviour
{
    Dictionary<HandStateType, PlayerHandState> states;

    public PlayerHandState CurrentState { get; private set; }

    public PlayerShooter shooterState;
    public PlayerThrower throwState;
    public Animator Animator { get; private set; }
    public PlayerInput Input { get; private set; }


    public event Action<PlayerHandState> OnStateChanged;

    [Header("Inventory References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;

    public Inventory Inventory => inventory;
    public InventoryUI InventoryUI => inventoryUI;

    public InputLockType InputLock { get; private set; } = InputLockType.None;


    // Start is called before the first frame update

    //카메라가아래를보면캐릭터도숙이기위해서 사용
    [Header("Aim Spine Control")]
    [SerializeField] public Transform cameraPivot;   // 카메라 피벗
    [SerializeField] Transform spine;          // Spine 또는 Chest
    //스파인z값제외한나머지는기본값반영
    private Quaternion spineBaseLocalRotation;

    private void Awake()
    {
        // Spine 기본 회전 저장
        spineBaseLocalRotation = spine.localRotation;

        Animator = GetComponent<Animator>();
        Input = GetComponent<PlayerInput>();

        states = new Dictionary<HandStateType, PlayerHandState>();


        foreach (var state in GetComponentsInChildren<PlayerHandState>())
        {
            state.Init(this);
            states.Add(state.StateType, state);
        }

    }

    public void Lock(InputLockType type)
    {
        InputLock |= type;
    }

    public void Unlock(InputLockType type)
    {
        InputLock &= ~type;
    }

    // 특정 락이 걸려 있는지
    public bool IsLocked(InputLockType type)
    {
        return (InputLock & type) != 0;
    }

    // 하나라도 잠겨 있는지
    public bool IsAnyLocked()
    {
        return InputLock != InputLockType.None;
    }

    //현재걸린lock모두해재
    public void ClearAllLocks()
    {
        InputLock = InputLockType.None;
    }

    public void OnAimPressed()
    {
        if (InputLock == InputLockType.None)
            CurrentState?.OnAimPressed();
    }

    public void OnAimReleased()
    {
        CurrentState?.OnAimReleased();
    }


    private void ApplySpineRotationByCamera()
    {

        //  카메라 X 각도 가져오기 (local 기준)
        float camX = cameraPivot.localEulerAngles.x;
        if (camX > 180f) camX -= 360f; // -180 ~ 180 변환

        //  입력 범위 제한
        camX = Mathf.Clamp(camX, -45f, 40f);

        float spineZ;

        //  구간별 선형 매핑
        if (camX <= 0f)
        {
            // -45 → +30  /  0 → 0
            spineZ = Mathf.Lerp(30f, 0f, Mathf.InverseLerp(-45f, 0f, camX));
            //Debug.Log(spineZ);
        }
        else
        {
            // 0 → 0  /  40 → -40
            spineZ = Mathf.Lerp(0f, -40f, Mathf.InverseLerp(0f, 40f, camX));
            //Debug.Log(spineZ);
        }

        //  애니메이션 이후 덮어쓰기 (Additive)
        spine.localRotation =
            spineBaseLocalRotation * Quaternion.Euler(0, 0, spineZ);

    }

    private void ApplyLeanAnimationByCamera()
    {
        //  카메라 X 각도 가져오기 (local 기준)
        float camX = cameraPivot.localEulerAngles.x;
        if (camX > 180f) camX -= 360f; // -180 ~ 180 변환

        //  입력 범위 제한
        camX = Mathf.Clamp(camX, -45f, 40f);

        Animator.SetFloat("Lean", camX);
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
        if (CurrentState == next)
        {
            //Debug.Log("byebye");
            return;
        }


        CurrentState?.Exit();
        CurrentState = next;
        CurrentState.Enter();

        OnStateChanged?.Invoke(CurrentState);
        Debug.Log("changeState");
    }

    void LateUpdate()
    {
        ApplyLeanAnimationByCamera();
        //카메라x축회전에따른spine01의 z값조정
        //ApplySpineRotationByCamera();
    }


}
