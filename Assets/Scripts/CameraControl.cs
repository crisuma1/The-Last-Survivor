using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public bool clickToMoveCamera = true;
    public bool canZoom = true;
    public float sensitivity = 5f;
    public Vector2 cameraLimit = new Vector2(-45, 40);
    

    float mouseX;
    float mouseY;
    float offsetDistanceY;
    float offsetDistanceZ;
    public Transform player;
    private bool isClicked = false;


    // Start is called before the first frame update
    void Start()
    {
        
        // 카메라 초기 회전을 마우스 값으로 세팅
        Vector3 angles = transform.eulerAngles;
        mouseX = angles.y;
        mouseY = -angles.x;
        


        offsetDistanceY = this.transform.position.y- player.position.y;
        offsetDistanceZ= this.transform.position.z - player.position.z;
        // 커서 숨기기
        if (!clickToMoveCamera)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
      


    }

    void CalculateCameraPosition()
    {
        //새로운 위치 계산
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        //카메라 리밋 적용
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
        //플레이어 카메라에맞게 회전(180도 회전)
        player.rotation = Quaternion.Euler(0, mouseX+180, 0);
       
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isClicked = true;
            
        }


        // 카메라 위치를 플레이어 위치에 맞게 갱신
        transform.position = player.position + new Vector3(0, offsetDistanceY, offsetDistanceZ);

        //카메라 줌인 줌아웃
        if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;
      



        if(clickToMoveCamera&& Input.GetAxisRaw("Fire2") != 0)
        {

            CalculateCameraPosition();
        }

        if(clickToMoveCamera==false && isClicked)
        {
            CalculateCameraPosition();
        }



    }
}
