using UnityEngine;

public class CinemachineCollisionFix : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1.5f, 0);

    private Vector3 originalLocalPosition;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 pivotPos = player.position + cameraOffset;
        Vector3 desiredCameraPos = pivotPos + transform.forward * -maxDistance;

        // Ray �� ī�޶�� ĳ���� ���̿� ���� �ִ��� Ȯ��
        if (Physics.Linecast(pivotPos, desiredCameraPos, out RaycastHit hit, wallMask))
        {
            float dist = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            transform.position = pivotPos + transform.forward * -dist;
        }
        else
        {
            transform.position = desiredCameraPos;
        }
    }
}
