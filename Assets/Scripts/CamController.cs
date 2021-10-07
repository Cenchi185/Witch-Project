using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] public GameObject player; // 바라볼 플레이어 오브젝트
    [SerializeField] private float moveSensivity = 1f;  // 카메라 민감도 조절
    [SerializeField] private float zoomSensivity = 1f;  // 카메라 줌 민감도

    private float xmove = 0;  // X축 누적 이동량
    private float ymove = 0;  // Y축 누적 이동량
    private float distance = 3; // 카메라와 캐릭터 거리

    private float zommSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        xmove += Input.GetAxis("Mouse X"); // 마우스의 좌우 이동
        ymove -= Input.GetAxis("Mouse Y"); // 마우스의 상하 이동
        ymove = Mathf.Clamp(ymove, -90f, 90f);  // 카메라가 캐릭터를 넘어가 반전되지 않도록 회전각 조절

        // 줌 부드럽게 하는방법 나중에 알아보기
        transform.rotation = Quaternion.Euler(ymove * moveSensivity, xmove * moveSensivity, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정
        distance -= Input.GetAxis("Mouse ScrollWheel") * zommSpeed * zoomSensivity;
        distance = Mathf.Clamp(distance, 3f, 10f);

        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // 이동량에 따른 Z 축방향의 벡터
        transform.position = player.transform.position - transform.rotation * reverseDistance; // 플레이어의 위치에서 카메라가 바라보는 방향에 벡터값을 적용한 상대 좌표를 차감
    }
}
