#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
// 총을 구현한다
public class Gun : MonoBehaviour
{
    // 총의 상태를 표현하는데 사용할 타입을 선언한다
    public enum State
    {
        Ready, // 발사 준비됨
        Empty, // 탄창이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 총알이 발사될 위치

    public GameObject bullet; //실제로 날릴 총알 프리팹

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 0; // 남은 전체 탄약
    public int magAmmo; // 현재 탄창에 남아있는 탄약

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private PlayerShooter shooter; //총을획득시 playershooter리스트에 추가

    public Transform LeftHandlePosition;
    public Transform RightHandlePosition;


    //장착시 프리팹의 값을그대로적용하기위해서 사용
    [Header("Prefab Transform")]
    public Vector3 equipLocalPosition;
    public Vector3 equipLocalRotation;
    public Vector3 equipLocalScale = Vector3.one;






    //프리팹의Transform정보를 총에저장해둠
#if UNITY_EDITOR
private void OnValidate()
{
    // 프리팹 에셋 상태에서만 실행
    if (!gameObject.scene.IsValid())
    {
        equipLocalPosition = transform.localPosition;
        equipLocalRotation = transform.localEulerAngles;
        equipLocalScale    = transform.localScale;
        // 변경 사항 저장
        EditorUtility.SetDirty(this);
    }
}
#endif

    private void Awake()
    {

        // 사용할 컴포넌트들의 참조를 가져오기



        if (gunAudioPlayer == null)
        {
            gunAudioPlayer = gameObject.AddComponent<AudioSource>();
        }
        gunAudioPlayer = GetComponent<AudioSource>();

        if (bulletLineRenderer == null)
        {
            bulletLineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        bulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;



        // 전체 예비 탄약 양을 초기화
        ammoRemain = gunData.startAmmoRemain;
        // 현재 탄창을 가득채우기
        magAmmo = gunData.magCapacity;

        // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        state = State.Ready;
        // 마지막으로 총을 쏜 시점을 초기화
        lastFireTime = 0;
    }

    //드랍되있는총을플레이어가획득시리스트에 추가
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerShooter shooter = other.GetComponent<PlayerShooter>();
        if (shooter == null) return;

        if (shooter.CurrentGun == this) return;

        shooter.AddGun(this);
        gameObject.SetActive(false); // Destroy 말고
    }




    private void OnEnable()
    {




    }




    // 발사 시도
    public bool Fire()
    {

        //  UI 위에 마우스가 올라가 있으면 발사 금지
        if (EventSystem.current != null &&
        EventSystem.current.IsPointerOverGameObject())
            return false;


        if (state != State.Ready) return false;
        if (Time.time < lastFireTime + gunData.timeBetFire) return false;


        lastFireTime = Time.time;
        Shot();
        return true;
    }

    // 실제 발사 처리
    private void Shot()
    {


        //실제피격판정ray는 카메라중앙에서나감
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));


        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;
        Debug.DrawRay(ray.origin, ray.direction * fireDistance, Color.green, 10.0f);

        LayerMask l = LayerMask.GetMask("Player");

        // 레이캐스트(시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if (Physics.Raycast(ray, out hit, fireDistance, ~l, QueryTriggerInteraction.Ignore)) //트리거박스는 레이어에걸리지않도록
        {
            // 레이가 어떤 물체와 충돌한 경우  
            // Debug.Log("레이캐스트 충돌 대상: " + hit.collider.gameObject.name, hit.transform);

            // 충돌한 상대방으로부터 IDamageable 오브젝트를 가져오기 시도
            IDamageable target =
                hit.collider.GetComponent<IDamageable>();

            // 상대방으로 부터 IDamageable 오브젝트를 가져오는데 성공했고자기자신이아닌경우
            if (target != null && !hit.collider.CompareTag("Player"))
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 총알이 최대 사정거리까지 날아갔을때의 위치를 충돌 위치로 사용
            Debug.Log($"충돌 안됌: {hitPosition}");
            hitPosition = fireTransform.position +
                          fireTransform.forward * fireDistance;
        }


        // 총알 생성 시 발사 방향에 맞게 회전 설정
        // 총알이 발사될 위치를 계산(반동때문에앞으로)
        Vector3 spawnPos = fireTransform.position + fireTransform.forward * 0.1f;
        Vector3 shootDir = (hitPosition - spawnPos).normalized;
        Instantiate(bullet, spawnPos, Quaternion.LookRotation(shootDir));


        // 발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPosition));


        // 남은 탄환의 수를 -1
        magAmmo--;
        if (magAmmo <= 0)
        {
            // 탄창에 남은 탄약이 없다면, 총의 현재 상태를 Empty으로 갱신
            state = State.Empty;
        }
    }

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {

        // 총구 화염 효과 재생
        muzzleFlashEffect.Play();
        // 탄피 배출 효과 재생
        shellEjectEffect.Play();

        // 총격 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload()
    {
        if (state == State.Reloading ||
            ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            // 이미 재장전 중이거나, 남은 총알이 없거나
            // 탄창에 총알이 이미 가득한 경우 재장전 할수 없다
            return false;
        }

        // 재장전 처리 시작
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
        // 재장전 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        // 재장전 소요 시간 만큼 처리를 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 탄창에 채울 탄약을 계산한다
        int ammoToFill = gunData.magCapacity - magAmmo;

        // 탄창에 채워야할 탄약이 남은 탄약보다 많다면,
        // 채워야할 탄약 수를 남은 탄약 수에 맞춰 줄인다
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        // 탄창을 채운다
        magAmmo += ammoToFill;
        // 남은 탄약에서, 탄창에 채운만큼 탄약을 뺸다
        ammoRemain -= ammoToFill;

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}
