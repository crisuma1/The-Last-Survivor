using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterTrigger : MonoBehaviour
{
    public GameObject ZombiePrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        
        if (other.gameObject.tag == "Player")
        {
            Invoke("SpawnZombie", 3f);

        }
    }
    void SpawnZombie()
    {
        Instantiate(ZombiePrefab, transform.position, Quaternion.identity);
    }
}
