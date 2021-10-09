using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigid;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Camera camera;

    // 캐릭터 내부 변수
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private float rotateSpeed = 10.0f;

    // 상태 변수
    private bool isGround = true;

    // 땅에 착지 여부
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        checkGround();
        Move();
        Jump();
    }

    private void Move() // 카메라 시야 기준으로 캐릭터 이동
    {
        Debug.DrawRay(cameraArm.position, new Vector3(transform.position.x - camera.transform.position.x, 0f, transform.position.z - camera.transform.position.z).normalized, Color.red);   // 카메라의 정면 벡터 구해서 씬에 표시
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // 캐릭터 이동방향 입력
        Vector3 lookFoward = new Vector3(transform.position.x - camera.transform.position.x, 0f, transform.position.z - camera.transform.position.z).normalized;    // 캐릭터의 정면벡터
        Vector3 lookRight = new Vector3(camera.transform.right.x, 0f, camera.transform.right.z).normalized; // 캐릭터의 좌, 우 벡터
        Vector3 moveDir = lookFoward * moveInput.y + lookRight * moveInput.x;   // 캐릭터의 이동방향 벡터 생성

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // 이동 후, 다시 정면을 바라보는 현상을 고치기 위함
        {
            if (Input.GetKey(KeyCode.LeftShift))    // 달리기
            {
                myRigid.velocity = moveDir * runSpeed; 
                myRigid.rotation = Quaternion.Lerp(myRigid.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
            else   // 걷기
            {
                myRigid.velocity = moveDir * walkSpeed;
                myRigid.rotation = Quaternion.Lerp(myRigid.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            myRigid.velocity = myRigid.transform.up * jumpHeight;
        }
    }

    private void checkGround()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, capsuleCollider.bounds.extents.y + 0.1f);
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
}
