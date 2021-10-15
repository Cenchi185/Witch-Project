using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform startPosition;   // ����ü ���� ����
    Vector3 endPosition;                        // ���� ����
    Vector3 currentPosition;                    // ���� ��ġ

    private float speedX;   // X���� �ӵ�
    private float speedY;   // Y���� �ӵ�
    private float speedZ;   // Z���� �ӵ�

    private float fGravity;     // Y������ �������� �߷°��ӵ�
    private float fEndTime;     // ������������ �ɸ��� �ð�
    private float fMaxHeigt = 70f;    // �������� �ִ� ����
    private float fHeight;      // �ִ���� - ���������� ����(Y)
    private float fEndHeight;   // �������� ���� - ���������� ���� (Y)
    private float fTime = 0f;   // �帣�� �ð�(???)
    private float fMaxTime = 1.6f;  // �ִ� ���̱��� ���µ� �ɸ��� �ð�

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
