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
    private CameraControl cameraControl;

    public bool isActivate = true; //비밀번호맞추면 다시지나가도안뜨게


    void Start()
    {
  
        cameraControl = Camera.main.GetComponentInParent<CameraControl>();

    }
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            passwordUI.SetActive(true);
            fKeyUI.SetActive(false);
            Time.timeScale = 0f;

            cameraControl.LockCamera();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(isActivate)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNear = true;
                fKeyUI.SetActive(true);
            }
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
