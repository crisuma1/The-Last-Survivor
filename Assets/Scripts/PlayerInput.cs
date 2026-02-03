using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공


public class PlayerInput : MonoBehaviour
{
    public string moveVerticalName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string moveHorizontalName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름


    // 값 할당은 내부에서만 가능
    public float horizontalmove { get; private set; } // 감지된 움직임 입력값
    public float verticalmove { get; private set; } // 감지된 회전 입력값

    public bool fireDown { get; private set; } // 누른 순간
    public bool fireUp { get; private set; }   // 뗀 순간


    public bool reload { get; private set; } // 감지된 재장전 입력값

    public bool jumpPressed { get; private set; }

    public PlayerShooter shooter { get; private set; }



    public int gunSlot { get; private set; } = -1;

    public int itemSlot { get; private set; } = -1;

    private PlayerHandStateController stateController;

    private void Awake()
    {
        shooter = GetComponent<PlayerShooter>();
        stateController = GetComponent<PlayerHandStateController>();
    }

    public void InitGunSlot()
    {
        gunSlot = -1;
    }

    public void InitItemSlot()
    {
        itemSlot = -1;
    }
    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        InitGunSlot();

        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            horizontalmove = 0;
            verticalmove = 0;
            fireDown = false;
            fireUp = false;
            reload = false;
            jumpPressed = false;
            return;
        }

        // move에 관한 입력 감지
        horizontalmove = Input.GetAxis(moveHorizontalName);
        // rotate에 관한 입력 감지
        verticalmove = Input.GetAxis(moveVerticalName);
        // fire에 관한 입력 감지
        fireDown = Input.GetButtonDown(fireButtonName);
        fireUp = Input.GetButtonUp(fireButtonName);



        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);

        jumpPressed = Input.GetButtonDown("Jump");

        //총종류변경
        if (Input.GetKeyDown(KeyCode.Z) && stateController.InputLock == InputLockType.None)
        {
            gunSlot = 0;

        }
        if (Input.GetKeyDown(KeyCode.X) && stateController.InputLock == InputLockType.None)
        {
            gunSlot = 1;
        }
        if (Input.GetKeyDown(KeyCode.C) && stateController.InputLock == InputLockType.None)
        {
            gunSlot = 2;
        }
        if (Input.GetKeyDown(KeyCode.V) && stateController.InputLock == InputLockType.None)
        {
            gunSlot = 3;
        }

        //마우스우클릭한번시중간줌->두번클릭시확대줌

        //우클릭누른순간
        if (Input.GetMouseButtonDown(1))
        {
            stateController.OnAimPressed();
        }

        // 우클릭 땐순간
        if (Input.GetMouseButtonUp(1))
        {
            stateController.OnAimReleased();
        }




        //아이템사용키 
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((i == 9) ? KeyCode.Alpha0 : (KeyCode)((int)KeyCode.Alpha1 + i)) && stateController.InputLock == InputLockType.None)
            {
                itemSlot = i;
                break;
            }
        }




    }
}