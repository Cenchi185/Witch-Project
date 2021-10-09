using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] public GameObject player; // �ٶ� �÷��̾� ������Ʈ
    [SerializeField] private float moveSensivity = 1f;  // ī�޶� �ΰ��� ����
    [SerializeField] private float zoomSensivity = 1f;  // ī�޶� �� �ΰ���

    private float xmove = 0;  // X�� ���� �̵���
    private float ymove = 0;  // Y�� ���� �̵���
    private float distance = 10; // ī�޶�� ĳ���� �Ÿ�

    private float zommSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        xmove += Input.GetAxis("Mouse X"); // ���콺�� �¿� �̵�
        ymove -= Input.GetAxis("Mouse Y"); // ���콺�� ���� �̵�
        ymove = Mathf.Clamp(ymove, -89f, 89f);  // ī�޶� ĳ���͸� �Ѿ �������� �ʵ��� ȸ���� ����, ���� 90���� �Ǹ� Move��ɿ� ������ ���ܼ� ���� 1�� �� ��

        // �� �ε巴�� �ϴ¹�� ���߿� �˾ƺ���
        transform.rotation = Quaternion.Euler(ymove * moveSensivity, xmove * moveSensivity, 0); // �̵����� ���� ī�޶��� �ٶ󺸴� ������ ����
        distance -= Input.GetAxis("Mouse ScrollWheel") * zommSpeed * zoomSensivity;     // ī�޶�� ĳ���� ������ �Ÿ� ����
        distance = Mathf.Clamp(distance, 3f, 10f);

        Vector3 reverseDistance = new Vector3(0f, 0f, distance); // �̵����� ���� Z ������� ����
        transform.position = player.transform.position - transform.rotation * reverseDistance; // �÷��̾��� ��ġ���� ī�޶� �ٶ󺸴� ���⿡ ���Ͱ��� ������ ��� ��ǥ�� ����
    }
}