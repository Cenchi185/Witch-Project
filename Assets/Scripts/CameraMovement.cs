using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform objectFollow;    // ī�޶� ���� ������Ʈ
    [SerializeField] private float followSpeed = 10f;   // ī�޶� ������Ʈ�� ���󰡴� �ӵ�
    [SerializeField] private float sensitivity = 500f;  // ī�޶� ���� �ΰ���
    [SerializeField] private float clampAngle = 80f;    // ī�޶� �ޱ� ������

    // ���콺�� ���� ��
    private float rotX;
    private float rotY;

    [SerializeField] private Transform realCamera;  // ���� ī�޶��� ��ġ
    [SerializeField] private float minDistance;     // ī�޶�� ĳ���ͻ��̿� ���ع��� ���� ��� ĳ���Ϳ� ī�޶��� �ּҰŸ�
    [SerializeField] private float maxDistance;     // ī�޶�� ĳ���ͻ��̿� ���ع��� ���� ��� ĳ���Ϳ� ī�޶��� �ִ�Ÿ�
    [SerializeField] private float zoomSensitivity = 10f; // ī�޶� �� �ΰ���

    private Vector3 dirNormalized; // ���� ���͸� �����ϴ� ����
    private Vector3 finalDir;      // ���������� ������ ����
    private float currentDistance = 10f; // ���� �Ÿ�
    private float finalDistance;   // ���������� ������ �Ÿ�
    private float smoothness = 2f;

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized; // normalized�� ����ϸ� ������ ���⸸ ���� �� ���� 
        finalDistance = realCamera.localPosition.magnitude;  // magnitude �� ����ϸ� ������ ũ�⸸ ���� �� ���� 

        Cursor.lockState = CursorLockMode.Locked;   // Ŀ���� ȭ�� �߾ӿ� ������Ŵ
        Cursor.visible = false; // Ŀ���� ������ �ʵ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime; // X���� ȸ���Ϸ��� ��/�Ϸ� �������� �ϹǷ� Mouse�� Y ��
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Y���� ȸ���Ϸ��� ��/��� �������� �ϹǷ� Mouse�� X��

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle); // ī�޶��� ���� ����
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate() // Upodate �Լ� ȣ���� ���� �� ȣ���
    {
        transform.position = Vector3.MoveTowards(transform.position, objectFollow.position, followSpeed * Time.deltaTime); // objectFollow �� ��ǥ�� ���󰡵��� ����

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);   // ī�޶�κ��� ĳ���ͱ����� ���� ����
        float wheelInput = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;  // ���콺 �� �Է� �ø��� 0.1 ������ -0.1 * zoomSensitivity ��ŭ

        currentDistance -= wheelInput;  // �ٷ� �Է��� ����ŭ + or -
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);   // �ּҰŸ��� �ִ� �Ÿ��� ����� �ʵ��� ��
        
        RaycastHit hit;
        if (Physics.Linecast(transform.position, finalDir, out hit))    // ī�޶󿡼����� finalDir ������ Ray�� ���ع��� hit �Ǹ�
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, currentDistance);    // ī�޶�� ĳ���Ͱ��� �Ÿ��� ���ع��� �Ÿ��� ���� ����, �ٸ� �ִ�, �ּҰ��� �Ѿ�� ����
        }
        else
        {
            finalDistance = currentDistance;    // �΋H���� ��ü�� ������ ���� ������ �Ÿ�
        }

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness); // realCamera�� localPosition ���κ��� dirNormalized * finalDistance ��ŭ ������ ��ġ��, Time.deltaTime * smoothness �� �ð� �ȿ� �̵�
    }
}
