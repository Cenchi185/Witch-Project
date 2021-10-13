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

    // ĳ���� ���� ����, �������� �����Ǿ� ���� ������ �ν����Ϳ��� ������ ��
    [SerializeField] private float walkSpeed = 5f;  // �ȱ� �ӵ�
    [SerializeField] private float runSpeed = 10f;  // �ٱ� �ӵ�
    [SerializeField] private float finalSpeed;      // �ȱ�, �ٱ��� �ϳ��� �ӵ�
    [SerializeField] private float jumpHeight;      // ���� ����
    [SerializeField] private float gravity;         // �߷�
    // private bool toggleCameraRotation;     // ī�޶� ���� ��ȯ

    private bool run;   // �ٱ���� üũ
    [SerializeField] private bool isGround = true; // �÷��̾� ��ġ üũ
    [SerializeField] private bool broom;   // ���ڷ縦 ź �������� üũ
    private float yVelocity;    // ĳ���� ���� ���� ����

    private float smoothness = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // _animator = this.GetComponent<Animator>();
        _camera = Camera.main;  // ���� ī�޶� ���
        _controller = this.GetComponent<CharacterController>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftAlt)) { toggleCameraRotation = true; } // ī�޶� ���� ����
        // else { toggleCameraRotation = false; }
        CheckGround();  // ��ġ üũ
        BroomRiding();

        if (Input.GetKey(KeyCode.LeftShift)) { run = true; }
        else { run = false; }
        InputMovement();    // �̵�

        #region �߷�, ����
        if (isGround)  // ���� �������� ����
        {
            yVelocity = 0f;     // ���� ������ yVelocity ���� �ʱ�ȭ
            if (Input.GetKeyDown(KeyCode.Space))
            {  
                yVelocity = jumpHeight; // yVelocity ���� ������ ����
                Jump();     // ���� �Լ� ȣ��
            }
        }
        else if(!isGround && broom)   // ���� �ִ� ���°� �ƴϰ�, ���ڷ縦 ź ���¶��
        {
            yVelocity -= gravity * Time.deltaTime;  
            yVelocity = yVelocity * 0.98f; // �޴� �߷��� ���� ��Ŵ
        }
        else // ���� �ִ� ���µ� �ƴϰ�, ���ڷ縦 ź���µ� �ƴ϶��
        {
            yVelocity -= gravity * Time.deltaTime;  // gravity�� ����� ���������Ƿ� -�� 
        }
        #endregion �߷�, ����
        Debug.Log(broom);
    }

    private void InputMovement()    // ī�޶� ���� ĳ���� �̵�
    {
        finalSpeed = (run) ? runSpeed : walkSpeed;

        Debug.DrawRay(followCam.position, new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized, Color.red);   // ī�޶��� ���� ���� ���ؼ� ���� ǥ��

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // ĳ���� �̵����� �Է�
        Vector3 lookFoward = new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized;    // ĳ���Ͱ� ������ ���麤��
        Vector3 lookRight = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized; // ĳ���Ͱ� ������ ��, �� ����
        Vector3 moveDir = (lookFoward * moveInput.y + lookRight * moveInput.x) * finalSpeed;   // ĳ������ �̵����� ����

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // �̵� ��, �ٽ� ������ �ٶ󺸴� ������ ��ġ�� ����
        {
            transform.forward = moveDir;    // ���� ������ moveDir �� ����
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * smoothness);    // �̵��ϴ� ���� �ٶ�
        }

        moveDir.y = yVelocity; // ĳ���Ͱ� ���� �߷� ���
        _controller.Move(moveDir * Time.deltaTime); // Move �� ����Ͽ� ĳ���� ������
    }

    private void CheckGround()  // ���� �ִ��� üũ
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, capsuleCollider.bounds.extents.y + 0.1f);    // ���� �������� ���ϴ� Ray ǥ��
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);  // capsulecolider �� ���� + 0.1f ��ŭ �Ʒ��� ���� Ray �� �浹�ߴ��� ���ߴ��� Ȯ��
        Debug.Log(isGround);
    }

    private void Jump() // ����
    {
        Vector3 jumpDir = new Vector3(0, jumpHeight, 0);    // ���� ����
        _controller.Move(jumpDir * Time.deltaTime);         // ���� ĳ���� ����
    }

    private void BroomRiding()
    {
        if (isGround)   // ���� �ִ� ���¿��� ���ڷ� ž�� �Ұ�
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
}
