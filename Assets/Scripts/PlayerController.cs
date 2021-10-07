using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private float h = 0f;
    private float v = 0f;
    private float r = 0f;

    private Transform transform;
    public float moveSpeed = 10.0f;

    public float rotSpeed = 80.0f;

    void Start()
    {
        transform = GetComponent<Transform>();        
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        transform.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime, Space.Self);

        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * h);
    }
}
