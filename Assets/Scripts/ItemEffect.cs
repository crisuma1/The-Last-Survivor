using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    [SerializeField] GameObject itemEffectPrefab;
    private GameObject effectInstance;

    void Start()
    {
        effectInstance = Instantiate(
        itemEffectPrefab,
        transform.position,
        Quaternion.identity,
        transform
    );

        Vector3 parentScale = transform.lossyScale;
        effectInstance.transform.localScale = new Vector3(
            1f / parentScale.x,
            1f / parentScale.y,
            1f / parentScale.z
        );
    }

 
}
