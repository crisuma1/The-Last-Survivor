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

    //카메라가아래를보면캐릭터도숙이기위해서 사용
    [Header("Aim Spine Control")]
    [SerializeField] Transform cameraPivot;   // 카메라 피벗
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
        if (CurrentState == next) return;


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
