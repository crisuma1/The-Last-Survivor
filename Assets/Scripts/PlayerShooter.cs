using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총이연사인지단발인지 설정
public enum FireState
{
    Single,
    Automatic
}


// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : PlayerHandState
{
    public override HandStateType StateType => HandStateType.Shooter;
    public Gun OriginalGun;// 사용할 기본총
    [HideInInspector] public Gun CurrentGun; // 현재사용중인총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점
    private List<Gun> Guns = new List<Gun>(); //여러종류의 총종류를담을 리스트
    private int currentGunIndex = 0; //현재활성화된총의인덱스
    private bool crosshairInitialized = false; //총의조준선활성화여부



    public float CurrentDefaultFOV;
    public float CurrentAdsFOV;
    public float CurrentScopeFOV;

    //총발사할때 같이발동될필요가있는 이벤트 등록하는곳
    public static event Action<GunData, AimState> OnFire;


    [SerializeField] private float autoFireStartDelay = 0.15f; //연사모션나오는시간

    private bool isFiring; //총을계속쏘고있는지
    private float firePressedTime; //마지막으로총을누른시간
    private Coroutine autoFireCoroutine; //총연사로직 



    public void Awake()
    {

        base.Awake();
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



    public override void Enter()
    {
        base.Enter();
        CurrentGun.gameObject.SetActive(true);
        Debug.Log("OMG");

    }



    public override void HandleInput()
    {
        base.HandleInput();
        // 총 상태일 때만 무기 변경 허용
        if (input.gunSlot >= 0)
        {
            ChangeGun(input.gunSlot);
        }



        // 입력을 감지하고 총 발사하거나 재장전

        // 눌렀을 때
        if (input.fireDown)
        {
            firePressedTime = Time.time;
            isFiring = true;

            FireOnce(); // 첫 발은 무조건 즉시
            TryStartAutoFire();
        }


        // 뗐을 때
        if (input.fireUp)
        {
            isFiring = false;
            StopAutoFire();
        }

        if (input.reload)
        {
            // 재장전 입력 감지시 재장전
            if (CurrentGun.Reload())
            {
                // 재장전 성공시에만 재장전 애니메이션 재생
                animator.SetTrigger("Reload");
            }
        }
        // 남은 탄약 UI를 갱신
        UpdateUI();
    }

    public override void Exit()
    {
        base.Exit();
        CurrentGun.gameObject.SetActive(false);
        input.InitGunSlot();
    }

    void FireOnce()
    {
        if (!CurrentGun.Fire()) return;

        animator.SetTrigger(CurrentGun.gunData.recoilTriggerName);
        OnFire?.Invoke(CurrentGun.gunData, input.currentAimState);
    }

    void TryStartAutoFire()
    {
        if (CurrentGun.gunData.GunState != FireState.Automatic)
            return;

        autoFireCoroutine = StartCoroutine(AutoFireRoutine());
    }

    IEnumerator AutoFireRoutine()
    {
        // 단발/연사 구분용 딜레이
        yield return new WaitForSeconds(autoFireStartDelay);

        while (isFiring)
        {
            if (CurrentGun.Fire())
            {
                //Debug.Log("currentfire");
                animator.SetBool("Automatic", true);
                OnFire?.Invoke(CurrentGun.gunData, input.currentAimState);
            }

            //기본발사간격대기
            yield return new WaitForSeconds(CurrentGun.gunData.timeBetFire);
        }

        animator.SetBool("Automatic", false);
    }

    //연사 코루틴도중에만약에무기를바꾸면 isFiring이 false처리가안되기때문에 여기서해줌
    void StopAutoFire()
    {
        if (autoFireCoroutine != null)
        {
            StopCoroutine(autoFireCoroutine);
            autoFireCoroutine = null;
        }

        animator.SetBool("Automatic", false);
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


    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동

        // 현재 재생 중인 애니메이션 상태
        // AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(1);

        //bool isSniperRecoil = stateInfo.IsName("GunplaySniper");

        // Shooter 상태가 아니면 총기설정의 IK적용안함 하지 않음
        if (statecontroller.CurrentState != this)
        {
            return;
        }



        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);
        //Debug.Log(playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow));


        // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다

        // float leftHandIKWeight = isSniperRecoil ? 0f : 1f;



        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);


        animator.SetIKPosition(
            AvatarIKGoal.LeftHand,
            leftHandMount.position
        );
        animator.SetIKRotation(
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