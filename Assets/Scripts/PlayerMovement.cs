using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animator _animator;   
    Camera _camera;         
    CharacterController _controller;    
    [SerializeField] Transform followCam;   

    // ĳ���� ���� ����
    [SerializeField] private float walkSpeed = 5f;  // �ȱ� �ӵ�
    [SerializeField] private float runSpeed = 10f;  // �ٱ� �ӵ�
    [SerializeField] private float finalSpeed;      // �ȱ�, �ٱ��� �ϳ��� �ӵ�
    // private bool toggleCameraRotation;     // ī�޶� ���� ��ȯ
    private bool run;   // �ٱ���� üũ

    private float smoothness = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // _animator = this.GetComponent<Animator>();
        _camera = Camera.main;  // ���� ī�޶� ���
        _controller = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.LeftAlt)) { toggleCameraRotation = true; } // ī�޶� ���� ����
        // else { toggleCameraRotation = false; }

        if(Input.GetKey(KeyCode.LeftShift)) { run = true; }
        else { run = false; }
        InputMovement();

    }

    private void InputMovement()    // ī�޶� ���� ĳ���� �̵�
    {
        finalSpeed = (run) ? runSpeed : walkSpeed;

        Debug.DrawRay(followCam.position, new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized, Color.red);   // ī�޶��� ���� ���� ���ؼ� ���� ǥ��

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));    // ĳ���� �̵����� �Է�
        Vector3 lookFoward = new Vector3(transform.position.x - _camera.transform.position.x, 0f, transform.position.z - _camera.transform.position.z).normalized;    // ĳ���Ͱ� ������ ���麤��
        Vector3 lookRight = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized; // ĳ���Ͱ� ������ ��, �� ����
        Vector3 moveDir = lookFoward * moveInput.y + lookRight * moveInput.x;   // ĳ������ �̵����� ���� ����

        if (!(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))    // �̵� ��, �ٽ� ������ �ٶ󺸴� ������ ��ġ�� ����
        {
                transform.forward = moveDir;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * smoothness);
        }
        _controller.Move(moveDir.normalized * finalSpeed * Time.deltaTime); // Move �� ����Ͽ� ĳ���� ������
    }

}
