using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private float range; // ������ ���� �ִ� �Ÿ�

    private bool pickUpActivated = false; // ������ ���� ���� ���� 
    private Collider[] pickItem;          // �浹ü ���� ����

    [SerializeField] private LayerMask layerMask; // Ư�� ���̾ ���� ������Ʈ�� �����ϵ���
    [SerializeField] private TextMeshProUGUI actionText;     // �÷��̾ ���� �ൿ�� ǥ���� �ؽ�Ʈ

    void Update()
    {
        CheckItem();
        if (Input.GetKeyDown(KeyCode.F)) // F��ư�� ������ �������� ����
        {
            PickUp();
        }
    }

    private void CheckItem()    // �ֿ� �� �ִ� �������� �ֺ��� �ִ��� üũ
    {
        pickItem = Physics.OverlapSphere(transform.position, range, layerMask); // �÷��̾� �ֺ� range ��ŭ�� ������ layerMask �� ������ �� ������ pickItem �� ����
                                                                                // �迭 ���� ������ �÷��̾�� ����� ������ ����
        if (pickItem != null && pickItem.Length > 0)   // pickItem �� ������ ������
        {
            #region ����� ��
            /* 
            for (int i = 0; i < pickItem.Length; i++)
            {
                Debug.Log(i + "�� ������ " + pickItem[i]);
            } 
            */
            #endregion
            ShowIteminfo(); // �ش� ������ ���
        }
        else 
        {
            Cancel_Iteminfo();  // ������ ������Ʈ�� �����Ƿ� ���� ��� ���
        }
    }

    private void ShowIteminfo() // �ֿ� �� �ִ� ������ ���� ǥ��
    {
        pickUpActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = pickItem[0].name + " �ݱ�";  // 0�� �ε����� ����� ������ ���� ǥ��(�ֿ�)
    }

    private void Cancel_Iteminfo()  // �������� ����� ������ ���� ǥ�� ����
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false); // ǥ�õ� pickItem UI �Ⱥ��̰� ����
    }

    private void PickUp()   // ������ �ݱ�
    {
        if (pickUpActivated)    // �������� �ֿ� �� �ִ� ���¸�
        {
            Debug.Log(pickItem[0].name + " ȹ��");
            Destroy(pickItem[0].gameObject);
        }
    }
}
