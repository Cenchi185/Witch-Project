using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animator _animator;   
    Camera _camera;         
    CharacterController _controller;    
    [SerializeField] Transform followCam;   

    // 캐릭터 내부 변수
    [SerializeField] private float walkSpeed = 5f;  // 걷기 속도
    [SerializeField] private float runSpeed = 10f;  // 뛰기 속도
    [SerializeField] private float finalSpeed;      // 걷기, 뛰기중 하나의 속도
    // private bool toggleCameraRotation;     // 카메라 상태 전환
    private bool run;   // 뛰기상태 체크

    private float smoothness = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // _animator = this.GetComponent<Animator>();
        _camera = Camera.main;  // 메인 카메라 사용
        _controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftAlt)) { toggleCameraRotation = true; } // 카메라 상태 변경
        // else { toggleCameraRotation = false; }

        if(Input.GetKey(KeyCode.LeftShift)) { run = true; }
        else { run = false; }
        InputMovement();

    }

    private void InputMovement()    // 카메라 기준 캐릭터 이동
    {
        finalSpeed = (run) ? runSpeed : walkSpeed;

        Debug.DrawRay(followCam.position, new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized, Color.red);   // 카메라의 정면 벡터 구해서 씬에 표시

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // 캐릭터 이동방향 입력
        Vector3 lookFoward = new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized;    // 캐릭터가 움직일 정면벡터
        Vector3 lookRight = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized; // 캐릭터가 움직일 좌, 우 벡터
        Vector3 moveDir = lookFoward * moveInput.y + lookRight * moveInput.x;   // 캐릭터의 이동방향 벡터 생성

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // 이동 후, 다시 정면을 바라보는 현상을 고치기 위함
        {
                transform.forward = moveDir;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * smoothness);
        }
        _controller.Move(moveDir.normalized * finalSpeed * Time.deltaTime); // Move 를 사용하여 캐릭터 움직임
    }

}
