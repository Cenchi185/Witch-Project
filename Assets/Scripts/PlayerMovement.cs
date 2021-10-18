using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animator _animator;   
    Camera _camera;         
    CharacterController _controller;
    Collider capsuleCollider;
    [SerializeField] Transform followCam;   // ī�޶� ����
    [SerializeField] Transform secondRay;   // ��������, �������� �Ǵ��� ���� �ι�° ray�� ������

    // ĳ���� ���� ����
    // private bool toggleCameraRotation;     // ī�޶� ���� ��ȯ
    [SerializeField] private float walkSpeed = 5f;  // �ȱ� �ӵ�
    [SerializeField] private float runSpeed = 10f;  // �ٱ� �ӵ�
    [SerializeField] private float jumpHeight = 15f;      // ���� ����
    [SerializeField] private float gravity = 20f;         // �߷�
    [SerializeField] private bool isGround = true; // �÷��̾� ��ġ üũ
    [SerializeField] private bool broom;   // ���ڷ縦 ź �������� üũ

    private bool broomBlock = true;   // ���� ���̿����� ���ڷ縦 ��Ÿ���� ����
    private bool run;   // �ٱ���� üũ

    private float finalSpeed;   // �ȱ�, �ٱ� �ӵ� ����
    private float yVelocity;    // ĳ���� ���� ���� ����
    private float smoothness = 10f;

    Vector3 moveDir;    // ĳ���� �̵� ����

    // Start is called before the first frame update
    void Start()
    {
        // _animator = this.GetComponent<Animator>();
        _camera = Camera.main;  // ���� ī�޶� ���
        _controller = this.GetComponent<CharacterController>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        moveDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftAlt)) { toggleCameraRotation = true; } // ī�޶� ���� ����
        // else { toggleCameraRotation = false; }
        CheckGround();
        Gravity();

        BroomRiding();

        if (Input.GetKey(KeyCode.LeftShift) && !broom) { run = true; }  // ���ڷ縦 Ÿ�� �߿��� �޸��⸦ �� �� ����
        else { run = false; }
        InputMovement();    // �̵�
    }

    private void InputMovement()    // ī�޶� ���� ĳ���� �̵�
    {
        finalSpeed = (run) ? runSpeed : walkSpeed;

        Debug.DrawRay(followCam.position, new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized, Color.red);   // ī�޶��� ���� ���� ���ؼ� ���� ǥ��

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // ĳ���� �̵����� �Է�
        Vector3 lookFoward = new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized;    // ĳ���Ͱ� ������ ���麤��
        Vector3 lookRight = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized; // ĳ���Ͱ� ������ ��, �� ����
        moveDir = (lookFoward * moveInput.y + lookRight * moveInput.x).normalized * finalSpeed;   // ĳ������ �̵����� ����, normalized ������ ������ �밢�� �̵��� ������������ ����

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // �̵� ��, �ٽ� ������ �ٶ󺸴� ������ ��ħ
        {
            transform.forward = moveDir;    // ���� ������ moveDir �� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * smoothness);    // �̵��ϴ� ���� �ٶ�
        }

        moveDir.y = yVelocity;
        _controller.Move(moveDir * Time.deltaTime); // Move �� ����Ͽ� ĳ���� ������
    }

    private void CheckGround()  // ���� �ִ��� üũ
    {
        // Debug.DrawRay(transform.position, Vector3.down, Color.red, capsuleCollider.bounds.extents.y + 0.5f);    // ���� �������� ���ϴ� Ray ǥ��
        if (Physics.Raycast(transform.position, Vector3.down ,capsuleCollider.bounds.extents.y + 0.1f) || _controller.isGrounded) // capsulecolider �� ���� + 0.1f ��ŭ �Ʒ��� ���� Ray �� �浹�ߴ��� ���ߴ��� Ȯ��
        {
            isGround = true;
        }
        else 
        {
            isGround = false;
        }
    }

    private void Jump() // ����
    {
        Vector3 jumpDir = new Vector3(0, jumpHeight, 0);    // ���� ����
        _controller.Move(jumpDir * Time.deltaTime);         // ���� ĳ���� ����
    }

    private void BroomRiding()
    {
        // Debug.DrawRay(transform.position, Vector3.down * 2f, Color.blue, 0.1f);    // ���� �������� ���ϴ� Ray ǥ��
        broomBlock = Physics.Raycast(transform.position, Vector3.down, 2f); // �ʹ� ���� ���̿��� ���ڷ縦 ž�� �� �� �������ϴ� Raycast
        // Debug.Log(broomBlock);

        if (isGround || broomBlock)   // ���� �ִ� ���¿��� ���ڷ� ž�� �Ұ�
        {
            broom = false;
        }
        else if (!isGround && !broom && Input.GetKeyDown(KeyCode.Space))    // ���� ���� �ʰ�, ���ڷ� ž�»��µ� �ƴҶ�, Space �Է�
        {
            broom = true;
        }
        else if (!isGround && broom && Input.GetKeyDown(KeyCode.Space))     // ���� ���� ������ ���ڷ� ž�»��� �϶�, Space �Է�
        {
            broom = false;
        }
    }

    private void Gravity()
    {
        if (isGround)  // ���� �������� ����
        {
            yVelocity = -10f;   // �׻� ���� �̻��� �߷��� �޵��� ����(��������, �������� ���������� ���� Ƣ�� ���� ����)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity += jumpHeight; // yVelocity ���� ������ ����
                Jump();     // ���� �Լ� ȣ��
            }
        }
        else if (!isGround && broom)   // ���� �ִ� ���°� �ƴϰ�, ���ڷ縦 ź ���¶��
        {
            yVelocity -= gravity * Time.deltaTime;
            yVelocity = yVelocity * 0.98f; // �޴� �߷��� ���� ��Ŵ
        }
        else // ���� �ִ� ���µ� �ƴϰ�, ���ڷ縦 ź���µ� �ƴ϶��
        {
            yVelocity -= gravity * Time.deltaTime;  // gravity�� ����� ���������Ƿ� -�� 
        }
    }
}
