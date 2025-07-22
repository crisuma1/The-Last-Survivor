using System.Collections;
using UnityEngine;

public class MotionSensorGun : MonoBehaviour
{
    [Header("����(����) ��ġ")]
    public Transform sensorPosition1;
    public Transform sensorPosition2;

    [Header("�Ѿ� �߻� ��ġ")]
    public Transform firePosition1;
    public Transform firePosition2;

    [Header("�Ѿ� ����")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float raycastDistance = 20f;
    public LayerMask playerLayer;

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

        Destroy(bullet, 10f);
    }
}