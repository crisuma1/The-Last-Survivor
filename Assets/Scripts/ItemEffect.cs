using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    [SerializeField] GameObject itemEffectPrefab;
    private GameObject effectInstance;


    private void Awake()
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
    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EffectOff();
        }
    }

    public void EffectOff()
    {
        Destroy(effectInstance);
        Debug.Log("des");
    }

}
