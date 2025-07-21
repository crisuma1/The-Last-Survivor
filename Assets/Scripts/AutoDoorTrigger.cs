using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoDoorTrigger : MonoBehaviour
{
    public GameObject fKeyUI; // "F키 누르시오" UI
    public GameObject passwordUI; // 암호 입력창 UI
    public float interactionDistance = 3f;

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            passwordUI.SetActive(true);
            fKeyUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            fKeyUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            fKeyUI.SetActive(false);
            passwordUI.SetActive(false);
        }
    }
}
