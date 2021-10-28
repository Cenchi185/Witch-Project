using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private float range; // ������ ���� �ִ� �Ÿ�
    [SerializeField] private InventoryUI inventory; // �÷��̾��� �κ��丮

    private bool pickUpActivated = false; // ������ ���� ���� ���� 
    private bool talkNPC = false;         // NPC �� ��ȭ ���� ����
    NPCData targetNPC;
    private int talkIndex;

    // �浹ü ���� ����
    private Collider[] pickItem;          
    private Collider[] npc; 

    // Ư�� ���̾ ���� ������Ʈ�� ����
    [SerializeField] private LayerMask ItemlayerMask; 
    [SerializeField] private LayerMask NPC_layerMask;

    [SerializeField] private GameObject itemInfoBar;    // actionText �� ǥ�õ� ���
    [SerializeField] private TextMeshProUGUI actionText;     // �÷��̾ ���� �ൿ�� ǥ���� �ؽ�Ʈ

    public TalkManager talkManager;

    void Update()
    {
        CheckAround();
        if (Input.GetKeyDown(KeyCode.F)) // F��ư�� ������ �������� ����
        {
            if (pickUpActivated) { PickUp(); }
            else if (talkNPC) { ChatNPC(targetNPC.id, targetNPC.isNPC); }
        }
    }

    private void CheckAround()    // �ֿ� �� �ִ� �������� �ֺ��� �ִ��� üũ
    {
        pickItem = Physics.OverlapSphere(transform.position, range, ItemlayerMask); // �÷��̾� �ֺ� range ��ŭ�� item ���̾ ������ �� ������ pickItem �� ����
                                                                                    // �迭 ���� ������ ���� ������ ������ > ����� ������ ����
        npc = Physics.OverlapSphere(transform.position, range, NPC_layerMask);
        if (pickItem.Length > 0 || npc.Length > 0)   // pickItem �� ������ ������
        {
            #region ����� ��
            /* 
            for (int i = 0; i < pickItem.Length; i++)
            {
                Debug.Log(i + "�� ������ " + pickItem[i]);
            } 
            */
            #endregion
            Showinfo(); // �ش� ������ ���
        }
        else 
        {
            Cancel_info();  // ������ ������Ʈ�� �����Ƿ� ���� ��� ���
        }
    }

    private void Showinfo() // ������ ��ü�� ���� ǥ��
    {
        actionText.gameObject.SetActive(true);
        itemInfoBar.SetActive(true);

        if (pickItem.Length > 0)    // ������ ������
        {
            pickUpActivated = true;
            actionText.text = pickItem[0].GetComponent<ItemPickup>().item.name + " �ݱ�";  // 0�� �ε����� ����� ������ ���� ǥ��
        }
        else if (npc.Length > 0)    // npc ���� ��
        { 
            talkNPC = true;
            targetNPC = npc[0].GetComponent<NPCData>();
            actionText.text = npc[0].name + " �� ��ȭ�ϱ�";
        }
    }

    private void Cancel_info()  // �������� ����� ���� ǥ�� ����
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false); // ǥ�õ� pickItem UI �Ⱥ��̰� ����
        itemInfoBar.SetActive(false);
    }

    private void PickUp()   // ������ �ݱ�
    {
        if (pickUpActivated)    // �������� �ֿ� �� �ִ� ���¸�
        {
            Debug.Log(pickItem[0].name + " ȹ��");
            inventory.PickUpItem_Add(pickItem[0].GetComponent<ItemPickup>().item);
            Destroy(pickItem[0].gameObject);    // �ֿ� �������� ���忡�� ���������� ��
        }
    }

    private void ChatNPC(int id, bool isNPC)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null) { talkNPC = false;  return; }
        if (isNPC)
        {
            talkManager.chatNPC_Panel.SetActive(true);
            talkManager.talkPanel.text = talkData;
        }
    }
}
