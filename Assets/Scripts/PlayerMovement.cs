using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animator _animator;   
    Camera _camera;         
    CharacterController _controller;
    Collider capsuleCollider;
    [SerializeField] Transform followCam;

    // 캐릭터 내부 변수, 변수값이 설정되어 있지 않으면 인스펙터에서 조정한 것
    [SerializeField] private float walkSpeed = 5f;  // 걷기 속도
    [SerializeField] private float runSpeed = 10f;  // 뛰기 속도
    [SerializeField] private float finalSpeed;      // 걷기, 뛰기중 하나의 속도
    [SerializeField] private float jumpHeight;      // 점프 높이
    [SerializeField] private float gravity;         // 중력
    // private bool toggleCameraRotation;     // 카메라 상태 전환

    private bool run;   // 뛰기상태 체크
    [SerializeField] private bool isGround = true; // 플레이어 위치 체크
    [SerializeField] private bool broom;   // 빗자루를 탄 상태인지 체크
    private float yVelocity;    // 캐릭터 점프 벡터 저장

    private float smoothness = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // _animator = this.GetComponent<Animator>();
        _camera = Camera.main;  // 메인 카메라 사용
        _controller = this.GetComponent<CharacterController>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftAlt)) { toggleCameraRotation = true; } // 카메라 상태 변경
        // else { toggleCameraRotation = false; }
        CheckGround();  // 위치 체크
        BroomRiding();

        if (Input.GetKey(KeyCode.LeftShift)) { run = true; }
        else { run = false; }
        InputMovement();    // 이동

        #region 중력, 점프
        if (isGround)  // 땅에 있을때만 점프
        {
            yVelocity = 0f;     // 땅에 닿으면 yVelocity 값을 초기화
            if (Input.GetKeyDown(KeyCode.Space))
            {  
                yVelocity = jumpHeight; // yVelocity 값을 높여서 점프
                Jump();     // 점프 함수 호출
            }
        }
        else if(!isGround && broom)   // 땅에 있는 상태가 아니고, 빗자루를 탄 상태라면
        {
            yVelocity -= gravity * Time.deltaTime;  
            yVelocity = yVelocity * 0.98f; // 받는 중력을 감소 시킴
        }
        else // 땅에 있는 상태도 아니고, 빗자루를 탄상태도 아니라면
        {
            yVelocity -= gravity * Time.deltaTime;  // gravity를 양수로 설정했으므로 -함 
        }
        #endregion 중력, 점프
        Debug.Log(broom);
    }

    private void InputMovement()    // 카메라 기준 캐릭터 이동
    {
        finalSpeed = (run) ? runSpeed : walkSpeed;

        Debug.DrawRay(followCam.position, new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized, Color.red);   // 카메라의 정면 벡터 구해서 씬에 표시

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // 캐릭터 이동방향 입력
        Vector3 lookFoward = new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized;    // 캐릭터가 움직일 정면벡터
        Vector3 lookRight = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized; // 캐릭터가 움직일 좌, 우 벡터
        Vector3 moveDir = (lookFoward * moveInput.y + lookRight * moveInput.x) * finalSpeed;   // 캐릭터의 이동방향 결정

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // 이동 후, 다시 정면을 바라보는 현상을 고치기 위함
        {
            transform.forward = moveDir;    // 정면 방향을 moveDir 로 설정
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * smoothness);    // 이동하는 방향 바라봄
        }

        moveDir.y = yVelocity; // 캐릭터가 받을 중력 계산
        _controller.Move(moveDir * Time.deltaTime); // Move 를 사용하여 캐릭터 움직임
    }

    private void CheckGround()  // 땅에 있는지 체크
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, capsuleCollider.bounds.extents.y + 0.1f);    // 지상 방향으로 향하는 Ray 표시
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);  // capsulecolider 의 절반 + 0.1f 만큼 아래로 향한 Ray 가 충돌했는지 안했는지 확인
        Debug.Log(isGround);
    }

    private void Jump() // 점프
    {
        Vector3 jumpDir = new Vector3(0, jumpHeight, 0);    // 점프 높이
        _controller.Move(jumpDir * Time.deltaTime);         // 실제 캐릭터 점프
    }

    private void BroomRiding()
    {
        if (isGround)   // 땅에 있는 상태에선 빗자루 탑승 불가
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
}
