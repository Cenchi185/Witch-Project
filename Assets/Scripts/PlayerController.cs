using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float walkSpeed = 5.0f;
    private float runSpeed = 10.0f;
    private float jumpHeight;
    private float rotateSpeed = 10.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3 (xMove, 0, zMove);

        if (!(xMove == 0 && zMove == 0))    // �̵� ��, �ٽ� ������ �ٶ󺸴� ������ ��ġ�� ����
        {
            if (Input.GetKey(KeyCode.LeftShift))    // �޸���
            {
                transform.position += moveDir * runSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
            else   // �ȱ�
            {
                transform.position += moveDir * walkSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
        }
    }
}
