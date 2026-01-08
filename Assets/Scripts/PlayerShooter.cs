using System;
using System.Collections.Generic;
using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour
{
    public Gun OriginalGun;// 사용할 기본총
    [HideInInspector] public Gun CurrentGun; // 현재사용중인총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    private List<Gun> Guns = new List<Gun>(); //여러종류의 총종류를담을 리스트
    private int currentGunIndex = 0; //현재활성화된총의인덱스
    private bool crosshairInitialized = false; //총의조준선활성화여부

    //카메라가아래를보면캐릭터도숙이기위해서 사용
    [Header("Aim Spine Control")]
    [SerializeField] Transform cameraPivot;   // 카메라 피벗
    [SerializeField] Transform spine;          // Spine 또는 Chest
    //스파인z값제외한나머지는기본값반영
    private Quaternion spineBaseLocalRotation;

    public float CurrentDefaultFOV;
    public float CurrentAdsFOV;
    public float CurrentScopeFOV;

    public static event Action<GunData, AimState> OnFire;

    public void Awake()
    {
        //기본총장착
        if (OriginalGun == null)
        {
            Debug.LogError("OriginalGun이 할당되지 않았습니다.");
            return;
        }
        CurrentGun = OriginalGun;
        Guns.Add(CurrentGun);

        //총의줌정도를데이타에서가져옴
        CurrentDefaultFOV = CurrentGun.gunData.defaultFOV;
        CurrentAdsFOV = CurrentGun.gunData.adsFOV;
        CurrentScopeFOV = CurrentGun.gunData.scopeFOV;
    }


    //드랍된총을먹을시 총리스트에총추가
    public void AddGun(Gun gun)
    {

        if (Guns.Contains(gun))
        {
            return;
        }
        Guns.Add(gun);
    }

    //플레이어input을받아서키입력시총교체
    public void ChangeGun(int index)
    {
        if (index < 0 || index >= Guns.Count) return;
        if (index == currentGunIndex) return;
        // 이전 총 비활성화
        if (CurrentGun != null)
            CurrentGun.gameObject.SetActive(false);

        currentGunIndex = index;
        CurrentGun = Guns[index];

        // 새 총 활성화
        CurrentGun.gameObject.SetActive(true);
        InitGun(CurrentGun);
    }

    //총변경시총의Transform값조정
    private void InitGun(Gun gun)
    {
        gun.transform.SetParent(gunPivot, false);

        //기본총이아닐경우 프리팹의 Trasnform값 적용
        if (currentGunIndex != 0)
        {
            gun.transform.localPosition = gun.equipLocalPosition;
            gun.transform.localRotation = Quaternion.Euler(gun.equipLocalRotation);
            gun.transform.localScale = gun.equipLocalScale;
        }


        this.leftHandMount = CurrentGun.LeftHandlePosition;
        this.rightHandMount = CurrentGun.RightHandlePosition;

        //총의줌정도를데이타에서가져옴
        CurrentDefaultFOV = CurrentGun.gunData.defaultFOV;
        CurrentAdsFOV = CurrentGun.gunData.adsFOV;
        CurrentScopeFOV = CurrentGun.gunData.scopeFOV;
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

        playerAnimator.SetFloat("Lean", camX);
    }


    private void Start()
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        // Spine 기본 회전 저장
        spineBaseLocalRotation = spine.localRotation;
    }



    private void OnEnable()
    {
        // 슈터가 활성화될 때 총도 함께 활성화
        CurrentGun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        CurrentGun.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.fire)
        {
            // 발사 입력 감지시 총 발사
            if (CurrentGun.Fire())
            {
                if (playerInput.currentFireState == FireState.Single || CurrentGun.gunData.GunState == FireState.Single)
                {
                    playerAnimator.SetTrigger(CurrentGun.gunData.recoilTriggerName);
                    // Debug.Log("shot");



                }
                if (playerInput.currentFireState == FireState.Automatic && CurrentGun.gunData.GunState == FireState.Automatic)
                {
                    playerAnimator.SetBool("Automatic", true);
                    // Debug.Log("Auto");
                }

                //스코프상태일때카메라반동

                OnFire?.Invoke(CurrentGun.gunData, playerInput.currentAimState);


            }

        }


        else if (playerInput.reload)
        {
            // 재장전 입력 감지시 재장전
            if (CurrentGun.Reload())
            {
                // 재장전 성공시에만 재장전 애니메이션 재생
                playerAnimator.SetTrigger("Reload");
            }
        }
        else
        {
            //총안쏠때연발해제
            playerAnimator.SetBool("Automatic", false);
        }


        // 남은 탄약 UI를 갱신
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI()
    {
        if (CurrentGun != null && GlobalUIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            GlobalUIManager.instance.UpdateAmmoText(CurrentGun.magAmmo, CurrentGun.ammoRemain);
        }
    }

    void LateUpdate()
    {
        //ApplyLeanAnimationByCamera();
        //카메라x축회전에따른spine01의 z값조정
        //ApplySpineRotationByCamera();
    }
    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동

        // 현재 재생 중인 애니메이션 상태
        // AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(1);

        //bool isSniperRecoil = stateInfo.IsName("GunplaySniper");

        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);
        //Debug.Log(playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow));


        // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다

        // float leftHandIKWeight = isSniperRecoil ? 0f : 1f;



        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);


        playerAnimator.SetIKPosition(
            AvatarIKGoal.LeftHand,
            leftHandMount.position
        );
        playerAnimator.SetIKRotation(
            AvatarIKGoal.LeftHand,
            leftHandMount.rotation
        );


        //////////////////////////////////



        /*

 //IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
 playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
 playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

 playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,
     rightHandMount.position);
 playerAnimator.SetIKRotation(AvatarIKGoal.RightHand,
     rightHandMount.rotation);

       */

        /*
        //crosshair의위치를총에따라다르게설정
        if (!crosshairInitialized)
        {

            CrosshairManager.Instance.SetTransform(CurrentGun.fireTransform);
            crosshairInitialized = true;
        }
        */
    }
}