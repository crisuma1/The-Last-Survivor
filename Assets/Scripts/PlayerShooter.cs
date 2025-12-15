using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour {
    public Gun OriginalGun;// 사용할 기본총
    [HideInInspector] public Gun CurrentGun; // 현재사용중인총
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트
    
    private List<Gun> Guns = new List<Gun>(); //여러종류의 총종류를담을 리스트
    private int currentGunIndex = 0;

    public void Awake()
    {
        if (OriginalGun == null)
        {
            Debug.LogError("OriginalGun이 할당되지 않았습니다.");
            return;
        }
        CurrentGun =OriginalGun;
        Guns.Add(CurrentGun);
    }

    public void AddGun(Gun gun)
    {

        if (Guns.Contains(gun))
        {
            return;
        }
        Guns.Add(gun);    
    }

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

    private void InitGun(Gun gun)
    {
        gun.transform.SetParent(gunPivot, false);

        //기본총이아닐경우 프리팹의 Trasnform값 적용
        if(currentGunIndex !=0)
        {
            gun.transform.localPosition = gun.equipLocalPosition;
            gun.transform.localRotation = Quaternion.Euler(gun.equipLocalRotation);
            gun.transform.localScale = gun.equipLocalScale;
        }
       

        this.leftHandMount=CurrentGun.LeftHandlePosition;
        this.rightHandMount=CurrentGun.RightHandlePosition;
    }



    private void Start() {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

        
    }
   
  

    private void OnEnable() {
        // 슈터가 활성화될 때 총도 함께 활성화
        CurrentGun.gameObject.SetActive(true);
    }

    private void OnDisable() {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        CurrentGun.gameObject.SetActive(false);
    }

    private void Update() {
        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.fire)
        {
            // 발사 입력 감지시 총 발사
            CurrentGun.Fire();
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

        // 남은 탄약 UI를 갱신
        UpdateUI();
    }

    // 탄약 UI 갱신
    private void UpdateUI() {
        if (CurrentGun != null && GlobalUIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            GlobalUIManager.instance.UpdateAmmoText(CurrentGun.magAmmo, CurrentGun.ammoRemain);
        }
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex) {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
        gunPivot.position =
            playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand,
            leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand,
            leftHandMount.rotation);

        // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,
            rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand,
            rightHandMount.rotation);
    }
}