using System.Collections;
using UnityEngine;

public class MotionSensorGun : MonoBehaviour
{
    [Header("센서(감지) 위치")]
    public Transform sensorPosition1;
    public Transform sensorPosition2;

    [Header("총알 발사 위치")]
    public Transform firePosition1;
    public Transform firePosition2;

    [Header("총알 설정")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float raycastDistance = 20f;
    public LayerMask playerLayer;

    [Header("이펙트 & 사운드")]
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    private AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    private LineRenderer bulletLineRenderer;


    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;
    }


    private void Update()
    {
        CheckAndFire(sensorPosition1, firePosition1);
        CheckAndFire(sensorPosition2, firePosition2);
    }

    private void CheckAndFire(Transform sensor, Transform firePoint)
    {
        if (sensor == null || firePoint == null || bulletPrefab == null)
            return;

        if (Physics.Raycast(sensor.position, sensor.forward, out RaycastHit hit, raycastDistance, playerLayer))
        {
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
 
                FireBullet(firePoint, hit.point);
            }
        }
    }

    private void FireBullet(Transform firePoint, Vector3 targetPoint)
    {
        Vector3 direction = (targetPoint - firePoint.position).normalized;
        Vector3 spawnPos = firePoint.position + direction * 0.5f;

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.LookRotation(direction));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        //  총구 화염
        if (muzzleFlashEffect != null)
            muzzleFlashEffect.Play();

        //  탄피 배출
        if (shellEjectEffect != null)
            shellEjectEffect.Play();

        //  총격 사운드
        if (gunAudioPlayer != null && shotClip != null)
            gunAudioPlayer.PlayOneShot(shotClip);

        //  궤적 라인
        if (bulletLineRenderer != null)
            StartCoroutine(ShotTrail(firePoint.position, targetPoint));


        Destroy(bullet, 10f);
    }



    private IEnumerator ShotTrail(Vector3 start, Vector3 end)
    {
        bulletLineRenderer.SetPosition(0, start);
        bulletLineRenderer.SetPosition(1, end);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }
}