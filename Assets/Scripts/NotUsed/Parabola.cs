using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform startPosition;   // 투사체 시작 지점
    Vector3 endPosition;                        // 도착 지점
    Vector3 currentPosition;                    // 현재 위치

    private float speedX;   // X축의 속도
    private float speedY;   // Y축의 속도
    private float speedZ;   // Z축의 속도

    private float fGravity;     // Y축으로 가해지는 중력가속도
    private float fEndTime;     // 도착지점까지 걸리는 시간
    private float fMaxHeigt = 70f;    // 포물선의 최대 높이
    private float fHeight;      // 최대높이 - 시작지점의 높이(Y)
    private float fEndHeight;   // 도착지점 높이 - 시작지점의 높이 (Y)
    private float fTime = 0f;   // 흐르는 시간(???)
    private float fMaxTime = 1.6f;  // 최대 높이까지 가는데 걸리는 시간

    // Start is called before the first frame update
    void Start()
    {
        fEndHeight = endPosition.y - startPosition.position.y;
        fHeight = fMaxHeigt - startPosition.position.y;

        fGravity = 2 * fHeight / (fMaxHeigt - startPosition.position.y);

        speedY = Mathf.Sqrt(2 * fGravity * fHeight);

        float a = fGravity;
        float b = -2 * speedY;
        float c = 2 * fEndHeight;

        fEndTime = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        speedX = -(startPosition.position.x - endPosition.x) / fEndTime;
        speedZ = -(startPosition.position.x - endPosition.x) / fEndTime;
    }

    // Update is called once per frame
    void Update()
    {
        fTime += Time.deltaTime;

        currentPosition.x = startPosition.position.x + speedX * fTime;
        currentPosition.y = startPosition.position.y + (speedY * fTime) - (0.5f * fGravity * fTime * fTime);
        currentPosition.z = startPosition.position.z + speedZ * fTime;
    }
}
