using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel; // �κ��丮 UI ������
    bool activeInventory = false;   // Ȱ��ȭ ���� ����

    [SerializeField] private GameObject slotsParent;    // ���Ե��� �θ��� Grid Setting

    [SerializeField] Slot[] slots;  // ������ �迭

    private void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
        inventoryPanel.SetActive(activeInventory);  // ���۽� �κ��丮 �� Ȱ��ȭ
    }

    private void Update()
    {
        OpenInven();    
    }

    private void OpenInven()
    {
        if (Input.GetKeyDown(KeyCode.I))    // i ��ư�� ������
        {
            activeInventory = !activeInventory; // �κ��丮 Ȱ��ȭ & ��Ȱ��ȭ
            inventoryPanel.SetActive(activeInventory);  // SetActive �� ������
        }
    }

    public void PickUpItem_Add(Item _item, int _count = 1) // �ֿ� ������ �κ��丮�� �߰�. �ֿ� �������� ���� _item, �߰��� ����
    {
        if (Item.ItemType.Equipment != _item.itemType)  // ������ Ÿ���� ������� Ȯ��
        {
            for (int i = 0; i < slots.Length; i++)  // �κ��丮 ũ�⸸ŭ �ݺ�
            {
                if (slots[i].item != null)  // i��° ������ ĭ�� ������� �ʴٸ�
                {
                    if (slots[i].item.itemName == _item.itemName)   // i��° ������ �̸��� ���ٸ�
                    {
                        slots[i].SetSlotCount(_count);  // ���� ����
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)  // ����ִ� ���� ã��
            {
                slots[i].AddItem(_item, _count);    // �ֿ� ������ �߰�.
                return;
            }
        }
    }
}
