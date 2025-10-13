using System.Runtime.CompilerServices;
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotationSpeed = 3f;
    public float jumpForce = 7f;
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

      
        // 움직임 실행
        MoveVertical();
        MoveHorizontal();
        Rotate();
        Jump();

        Vector2 moveInput = new Vector2(playerInput.horizontalmove, playerInput.verticalmove);
        float moveSpeed = moveInput.magnitude; // 이동 벡터의 크기 (0~1)
        playerAnimator.SetFloat("Move", moveSpeed);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void MoveVertical()
    {
        // 상대적으로 이동할 거리 계산
        Vector3 moveDistance =
            playerInput.verticalmove * transform.forward * moveSpeed * Time.deltaTime;
        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        playerAnimator.SetFloat("Move", playerInput.verticalmove);
    }

    private void MoveHorizontal()
    {
        // 상대적으로 이동할 거리 계산
        Vector3 moveDistance =
            playerInput.horizontalmove * transform.right * moveSpeed * Time.deltaTime;
        // 리지드바디를 통해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        playerAnimator.SetFloat("Move", playerInput.horizontalmove);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전

private void Rotate()
{

        /*
        Vector3 moveDir = new Vector3(playerInput.horizontalmove, 0, playerInput.verticalmove);

        // 1️ 입력이 거의 없으면 회전 안 함
        if (moveDir.sqrMagnitude < 0.001f)
            return;

        // 2️정규화 (길이를 1로)
        moveDir.Normalize();

        // 3️ 현재 바라보는 방향과 이동 방향의 각도 차이 계산
        float angleDiff = Vector3.Angle(transform.forward, moveDir);

        // 4️ 각도 차이가 매우 작으면 회전 생략 (정면 유지)
        if (angleDiff < 1f)
            return;

        // 5️ 목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);

        // 6️ 회전을 부드럽게 보간
        float rotationSpeed = 5f;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
        */


    }



    private void Jump()
    {
        if (playerInput.jumpPressed && isGrounded)
        {
            Vector3 velocity = playerRigidbody.velocity;
            velocity.y = jumpForce;
            playerRigidbody.velocity = velocity;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}