using UnityEngine;

public class ZombieLeftHandHitbox : MonoBehaviour
{
    public bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isHit == false)
        {
            isHit = true;
            Debug.Log("hit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isHit == true)
        {
            isHit = false;
            Debug.Log("out");
        }
    }
}
