using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform objectFollow;    // 카메라가 따라갈 오브젝트
    [SerializeField] private float followSpeed = 10f;   // 카메라가 오브젝트를 따라가는 속도
    [SerializeField] private float sensitivity = 500f;  // 카메라 조작 민감도
    [SerializeField] private float clampAngle = 80f;    // 카메라 앵글 고정각

    // 마우스의 조작 값
    private float rotX;
    private float rotY;

    [SerializeField] private Transform realCamera;  // 실제 카메라의 위치
    [SerializeField] private float minDistance;     // 카메라와 캐릭터사이에 방해물이 있을 경우 캐릭터와 카메라간의 최소거리
    [SerializeField] private float maxDistance;     // 카메라와 캐릭터사이에 방해물이 있을 경우 캐릭터와 카메라간의 최대거리
    [SerializeField] private float zoomSensitivity = 10f; // 카메라 줌 민감도

    private Vector3 dirNormalized; // 방향 벡터를 저장하는 변수
    private Vector3 finalDir;      // 최종적으로 정해진 방향
    private float currentDistance = 10f; // 현재 거리
    private float finalDistance;   // 최종적으로 결정된 거리
    private float smoothness = 2f;

    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized; // normalized를 사용하면 벡터의 방향만 남길 수 있음 
        finalDistance = realCamera.localPosition.magnitude;  // magnitude 를 사용하면 벡터의 크기만 남길 수 있음 

        Cursor.lockState = CursorLockMode.Locked;   // 커서를 화면 중앙에 고정시킴
        Cursor.visible = false; // 커서가 보이지 않도록 가림
    }

    // Update is called once per frame
    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime; // X축을 회전하려면 상/하로 움직여야 하므로 Mouse의 Y 값
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Y축을 회전하려면 좌/우로 움직여야 하므로 Mouse의 X값

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle); // 카메라의 각도 제한
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate() // Upodate 함수 호출이 끝난 후 호출됨
    {
        transform.position = Vector3.MoveTowards(transform.position, objectFollow.position, followSpeed * Time.deltaTime); // objectFollow 의 좌표를 따라가도록 설정

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);   // 카메라로부터 캐릭터까지의 방향 벡터
        float wheelInput = Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;  // 마우스 휠 입력 올릴때 0.1 내릴때 -0.1 * zoomSensitivity 만큼

        currentDistance -= wheelInput;  // 휠로 입력한 값만큼 + or -
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);   // 최소거리와 최대 거리를 벗어나진 않도록 함
        
        RaycastHit hit;
        if (Physics.Linecast(transform.position, finalDir, out hit))    // 카메라에서부터 finalDir 방향의 Ray에 방해물이 hit 되면
        {
            finalDistance = Mathf.Clamp(hit.distance, minDistance, currentDistance);    // 카메라와 캐릭터간의 거리를 방해물의 거리에 맞춰 조절, 다만 최대, 최소값을 넘어가지 않음
        }
        else
        {
            finalDistance = currentDistance;    // 부딫히는 물체가 없으면 현재 설정된 거리
        }

        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness); // realCamera의 localPosition 으로부터 dirNormalized * finalDistance 만큼 떨어진 위치로, Time.deltaTime * smoothness 의 시간 안에 이동
    }
}
