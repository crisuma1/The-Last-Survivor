using UnityEngine;

// 플레이어 캐릭터를 조작하기 위한 사용자 입력을 감지
// 감지된 입력값을 다른 컴포넌트들이 사용할 수 있도록 제공


public enum AimState
{
    None,
    ADS,
    SCope
}

public enum FireState
{
    Single,
    Automatic
}


public class PlayerInput : MonoBehaviour
{
    public string moveVerticalName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string moveHorizontalName = "Horizontal"; // 좌우 움직임을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼 이름

    // 값 할당은 내부에서만 가능
    public float horizontalmove { get; private set; } // 감지된 움직임 입력값
    public float verticalmove { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값


    public bool reload { get; private set; } // 감지된 재장전 입력값

    public bool jumpPressed { get; private set; }

    public PlayerShooter shooter { get; private set; }

    public AimState currentAimState = AimState.None;

    //-1값은 마우스우클릭을처음눌럿을때 ADS로설정하기위한 임시값
    float lastRightClickTime = -1f;
    //우클릭한번누른상태에서 다시누를떄scope상태로가기위한최소시간
    float doubleClickThreshold = 1f;
    //우클릭누르는중인지
    bool isRightHeld = false;

    //단발연발구분해서 애니따로나오게
    public FireState currentFireState = FireState.Single;
    private float SingleToAutomaticGap = 0.1f; //단발에서연발로넘어가는데누르는시간갭
    private float fireButtonDownTime = 1f; //총입력처음누른시간

    public int gunSlot { get; private set; } = -1;

    private void Awake()
    {
        shooter = GetComponent<PlayerShooter>();
    }

    public void InitGunSlot()
    {
        gunSlot = -1;
    }
    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        if (GameManager.instance != null
            && GameManager.instance.isGameover)
        {
            horizontalmove = 0;
            verticalmove = 0;
            fire = false;
            reload = false;
            jumpPressed = false;
            return;
        }

        // move에 관한 입력 감지
        horizontalmove = Input.GetAxis(moveHorizontalName);
        // rotate에 관한 입력 감지
        verticalmove = Input.GetAxis(moveVerticalName);
        // fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);


        //단발연발구분용로직
        // 버튼 처음 눌렀을 때
        if (Input.GetButtonDown(fireButtonName))
        {
            fireButtonDownTime = Time.time;
            currentFireState = FireState.Single;
        }

        // 누르고 있는 동안
        if (fire)
        {
            if (currentFireState == FireState.Single &&
                Time.time - fireButtonDownTime >= SingleToAutomaticGap)
            {
                currentFireState = FireState.Automatic;
            }
        }

        // 버튼을 떼면 리셋
        if (Input.GetButtonUp(fireButtonName))
        {
            currentFireState = FireState.Single;
        }




        // reload에 관한 입력 감지
        reload = Input.GetButtonDown(reloadButtonName);

        jumpPressed = Input.GetButtonDown("Jump");

        //총종류변경
        if (Input.GetKeyDown(KeyCode.Z))
        {
            gunSlot = 0;

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            gunSlot = 1;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            gunSlot = 2;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            gunSlot = 3;
        }

        //마우스우클릭한번시중간줌->두번클릭시확대줌
        if (Input.GetMouseButtonDown(1))
        {
            float now = Time.time;

            if (now - lastRightClickTime <= doubleClickThreshold)
            {
                currentAimState = AimState.SCope;
            }
            else
            {
                currentAimState = AimState.ADS;
            }
            lastRightClickTime = now;
            isRightHeld = true;
        }

        // 우클릭 유지 중
        if (isRightHeld)
        {
            if (!Input.GetMouseButton(1))
            {
                // 버튼을 떼는 순간
                currentAimState = AimState.None;
                isRightHeld = false;
            }
        }




    }
}