using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigid;
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Camera camera;

    // ĳ���� ���� ����
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float jumpHeight = 10.0f;
    [SerializeField] private float rotateSpeed = 10.0f;

    // ���� ����
    private bool isGround = true;

    // ���� ���� ����
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

    private void Move() // ī�޶� �þ� �������� ĳ���� �̵�
    {
        Debug.DrawRay(cameraArm.position, new Vector3(transform.position.x - camera.transform.position.x, 0f, transform.position.z - camera.transform.position.z).normalized, Color.red);   // ī�޶��� ���� ���� ���ؼ� ���� ǥ��
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // ĳ���� �̵����� �Է�
        Vector3 lookFoward = new Vector3(transform.position.x - camera.transform.position.x, 0f, transform.position.z - camera.transform.position.z).normalized;    // ĳ������ ���麤��
        Vector3 lookRight = new Vector3(camera.transform.right.x, 0f, camera.transform.right.z).normalized; // ĳ������ ��, �� ����
        Vector3 moveDir = lookFoward * moveInput.y + lookRight * moveInput.x;   // ĳ������ �̵����� ���� ����

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // �̵� ��, �ٽ� ������ �ٶ󺸴� ������ ��ġ�� ����
        {
            if (Input.GetKey(KeyCode.LeftShift))    // �޸���
            {
                myRigid.velocity = moveDir * runSpeed; 
                myRigid.rotation = Quaternion.Lerp(myRigid.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
            else   // �ȱ�
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
