using System.Runtime.CompilerServices;
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotationSpeed = 3f;
    private float jumpForce = 5f;
    public Transform groundCheck; //  발밑 기준점
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;


    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    public bool isGrounded;

    private void Start()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {

     

        CheckGround();
      

        // 이동처리

        float directionX= playerInput.horizontalmove * moveSpeed* Time.deltaTime;
        float directionZ= playerInput.verticalmove * moveSpeed* Time.deltaTime;

        //회전처리
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;


        forward.y = 0;
        right.y = 0;


        forward.Normalize();
        right.Normalize();

        forward = forward * directionZ;
        right = right * directionX;


        /*
        //회전처리
        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }
        */
        

        //최종계산

      
        Vector3 horizontalDirection = forward + right;
        Vector3 moviment = horizontalDirection;

        playerRigidbody.MovePosition(playerRigidbody.position + moviment);
        


        Jump();

        Vector2 moveInput = new Vector2(playerInput.horizontalmove, playerInput.verticalmove);
        float movespeed = moveInput.magnitude; // 이동 벡터의 크기 (0~1)
        playerAnimator.SetFloat("Move", movespeed);
    }

  


    private void Jump()
    {
        if (playerInput.jumpPressed && isGrounded)
        {
            Vector3 velocity = playerRigidbody.velocity;
            velocity.y = jumpForce;
            playerRigidbody.velocity = velocity;

            playerAnimator.SetTrigger("Jump");
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}