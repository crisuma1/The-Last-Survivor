using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Instance;

    [SerializeField] private GameObject crosshair;

    [SerializeField] private Transform crosshairTransform;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetTransform(Transform fireTransform)
    {
        crosshairTransform.SetPositionAndRotation(
        fireTransform.TransformPoint(Vector3.forward * 10f),
        fireTransform.rotation);

        crosshairTransform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        crosshair.SetActive(false);
    }
}
