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

    public Transform player;
    private bool isClicked = false;



    //카메라충돌때사용할변수
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float smoothSpeed = 10f;
    private Vector3 direction;
    private float desiredDistance;
    private Vector3 desiredPosition;
    private PlayerHealth playerHealth;

    //마우스우클릭시줌하기위해playerinput할당
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] float fovLerpSpeed = 10f;


    float targetFOV;


    [SerializeField] LayerMask normalMask; // 기본
    [SerializeField] LayerMask scopeMask;  // Player 제외

    //FOV값을받기위해참조
    [SerializeField] PlayerShooter shooter;


    // Start is called before the first frame update
    void Start()
    {
      
        playerHealth = FindObjectOfType<PlayerHealth>();
      

        desiredDistance = Vector3.Distance(Camera.main.transform.position, player.position);

        // 카메라 초기 회전을 마우스 값으로 세팅
        Vector3 angles = transform.eulerAngles;
        mouseX = angles.y;
        mouseY = -angles.x;     
        


        // 커서 숨기기
        if (!clickToMoveCamera)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }



        targetFOV = shooter.CurrentDefaultFOV;
        Camera.main.fieldOfView = shooter.CurrentDefaultFOV;

    }

    void CalculateCameraRotation()
    {
        //새로운 위치 계산
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        //카메라 리밋 적용
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        //플레이어 카메라에맞게 회전(180도 회전)
        player.parent.rotation = Quaternion.Euler(0, mouseX+180, 0);
    }

    private void SetFOV(float fov)
    {
        Camera.main.fieldOfView = Mathf.Lerp(
           Camera.main.fieldOfView,
            fov,
            Time.deltaTime * fovLerpSpeed
        );
    }

    // Update is called once per frame
    void Update()
    {
        bool isScope = playerInput.currentAimState == AimState.SCope;
        Camera.main.cullingMask = isScope ? scopeMask : normalMask;


        if (!playerHealth.dead) //죽었을때카메라회전안하게
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClicked = true;
            }

            //카메라 줌인 줌아웃
            if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
                Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;

            if (clickToMoveCamera && Input.GetAxisRaw("Fire2") != 0)
            {
                CalculateCameraRotation();
            }

            if (clickToMoveCamera == false && isClicked)
            {
                CalculateCameraRotation();
            }

            direction = transform.forward;
            //direction = (Camera.main.transform.position - player.position).normalized;

            /*
            Debug.Log(transform.forward);
            Debug.Log(direction);
            Debug.DrawRay(player.position, transform.forward, Color.red);
            Debug.DrawRay(player.position, direction, Color.green);
            */

            //카메라와 플레이어사이에 물체가있어서 가려질시 앞당김
            if (Physics.SphereCast(player.position, sphereRadius, direction, out RaycastHit hit, desiredDistance))
            {
                //Debug.Log("T");
                float newDist = Mathf.Clamp(hit.distance - 0.1f, minDistance, desiredDistance);
                Vector3 newPos = player.position + (direction * newDist);
                Camera.main.transform.position = newPos;
            }
            else
            {
                //Debug.Log("F");
                desiredPosition = player.position + (direction * desiredDistance);

                Camera.main.transform.position = desiredPosition; //Vector3.Slerp(Camera.main.transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            }
        }
        else
        {
            //마우스 커서 보이기
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }


        switch (playerInput.currentAimState)
        {
            case AimState.None:
                {
                    targetFOV = shooter.CurrentDefaultFOV;
                    // Debug.Log("defaultFOV");
                    break;
                }
            case AimState.ADS:
                {
                    targetFOV = shooter.CurrentAdsFOV;
                    //Debug.Log("adsFOV");
                    break;
                }
            case AimState.SCope:
                {
                    targetFOV = shooter.CurrentScopeFOV;
                    //Debug.Log("scopeFOV");
                    break;
                }
                
        }

        SetFOV(targetFOV);




    }
}
