using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animator _animator;   
    Camera _camera;         
    CharacterController _controller;
    Collider capsuleCollider;
    StatusController state;
    [SerializeField] Transform followCam;   // 카메라 초점

    // 캐릭터 내부 변수
    // private bool toggleCameraRotation;     // 카메라 상태 전환
    private float walkSpeed = 5f;  // 걷기 속도
    private float runSpeed = 10f;  // 뛰기 속도
    private float broomSpeed = 8f;  // 빗자루 탑승시 속도
    private float jumpHeight = 18f;      // 점프 높이
    private float gravity = 20f;         // 중력
    [SerializeField] private bool isGround = true; // 플레이어 위치 체크
    [SerializeField] private bool broom;   // 빗자루를 탄 상태인지 체크

    private bool broomBlock = true;   // 낮은 높이에서는 빗자루를 못타도록 설정
    private bool run;   // 뛰기상태 체크

    private float finalSpeed;   // 걷기, 뛰기 속도 결정
    private float yVelocity;    // 캐릭터 점프 벡터 저장
    private float smoothness = 10f;

    Vector3 moveDir;    // 캐릭터 이동 방향

    // Start is called before the first frame update
    void Start()
    {
        // _animator = this.GetComponent<Animator>();
        _camera = Camera.main;  // 메인 카메라 사용
        _controller = this.GetComponent<CharacterController>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        moveDir = Vector3.zero; // 초기화
        state = FindObjectOfType<StatusController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftAlt)) { toggleCameraRotation = true; } // 카메라 상태 변경
        // else { toggleCameraRotation = false; }
        CheckGround();
        Gravity();

        BroomRiding();
        if (Input.GetKey(KeyCode.LeftShift) && !broom && state.GetcurrnetST() > 0) // 스태미나가 다 닳으면 달리기를 할 수 없음
        { run = true; }
        else { run = false; }

        InputMovement();    // 이동
    }

    #region Getter
    public bool Getbroom() { return broom; }

    public bool Getrun() { return run; }
    #endregion

    private void InputMovement()    // 카메라 기준 캐릭터 이동
    {
        if (run) { finalSpeed = runSpeed; }
        else if (broom) { finalSpeed = broomSpeed; }
        else { finalSpeed = walkSpeed; }

        Debug.DrawRay(followCam.position, new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized, Color.red);   // 카메라의 정면 벡터 구해서 씬에 표시

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // 캐릭터 이동방향 입력
        Vector3 lookFoward = new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized;    // 캐릭터가 움직일 정면벡터
        Vector3 lookRight = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized; // 캐릭터가 움직일 좌, 우 벡터
        moveDir = (lookFoward * moveInput.y + lookRight * moveInput.x).normalized * finalSpeed;   // 캐릭터의 이동방향 결정, normalized 해주지 않으면 대각선 이동이 비정상적으로 빠름

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // 이동 후, 다시 정면을 바라보는 현상을 고침
        {
            transform.forward = moveDir;    // 정면 방향을 moveDir 로 설정
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * smoothness);    // 이동하는 방향 바라봄
        }

        moveDir.y = yVelocity;
        _controller.Move(moveDir * Time.deltaTime); // Move 를 사용하여 캐릭터 움직임
    }

    private void CheckGround()  // 땅에 있는지 체크
    {
        // Debug.DrawRay(transform.position, Vector3.down, Color.red, capsuleCollider.bounds.extents.y + 0.5f);    // 지상 방향으로 향하는 Ray 표시
        if (Physics.Raycast(transform.position, Vector3.down ,capsuleCollider.bounds.extents.y + 0.1f) || _controller.isGrounded) // capsulecolider 의 절반 + 0.1f 만큼 아래로 향한 Ray 가 충돌했는지 안했는지 확인
        {
            isGround = true;
        }
        else 
        {
            isGround = false;
        }
    }

    private void Jump() // 점프
    {
        Vector3 jumpDir = new Vector3(0, jumpHeight, 0);    // 점프 높이
        _controller.Move(jumpDir * Time.deltaTime);         // 실제 캐릭터 점프
    }

    private void BroomRiding()  // 빗자루 탑승 판단 함수
    {
        // Debug.DrawRay(transform.position, Vector3.down * 2f, Color.blue, 0.1f);    // 지상 방향으로 향하는 Ray 표시
        broomBlock = Physics.Raycast(transform.position, Vector3.down, 3f); // 너무 낮은 높이에서 빗자루를 탑승 할 수 없도록하는 Raycast
        // Debug.Log(broomBlock);

        if (isGround || broomBlock || state.GetcurrnetST() <= 0)   // 땅에 있는 상태, 스태미나가 없는 상태에서는 탑승 불가
        {
            broom = false;
        }
        else if (!isGround && !broom && Input.GetKeyDown(KeyCode.Space))    // 땅에 있지 않고, 빗자루 탑승상태도 아닐때, Space 입력
        {
            broom = true;
        }
        else if (!isGround && broom && Input.GetKeyDown(KeyCode.Space))     // 땅에 있지 않지만 빗자루 탑승상태 일때, Space 입력
        {
            broom = false;
        }
    }

    private void Gravity()  // 캐릭터 중력 함수
    {
        if (isGround)  // 땅에 있을때만 점프
        {
            yVelocity = -10f;   // 항상 일정 이상의 중력을 받도록 설정(오르막길, 내리막길 오르내릴때 통통 튀는 현상 방지)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity += jumpHeight; // yVelocity 값을 높여서 점프
                Jump();     // 점프 함수 호출
            }
        }
        else if (!isGround && broom)   // 땅에 있는 상태가 아니고, 빗자루를 탄 상태라면
        {
            yVelocity -= gravity * Time.deltaTime;
            yVelocity = yVelocity * 0.98f; // 받는 중력을 감소 시킴
        }
        else // 땅에 있는 상태도 아니고, 빗자루를 탄상태도 아니라면
        {
            yVelocity -= gravity * Time.deltaTime;  // gravity를 양수로 설정했으므로 -함 
        }
    }
}
