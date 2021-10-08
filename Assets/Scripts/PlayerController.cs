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

        if (!(xMove == 0 && zMove == 0))    // 이동 후, 다시 정면을 바라보는 현상을 고치기 위함
        {
            if (Input.GetKey(KeyCode.LeftShift))    // 달리기
            {
                transform.position += moveDir * runSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
            else   // 걷기
            {
                transform.position += moveDir * walkSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
            }
        }
    }
}
