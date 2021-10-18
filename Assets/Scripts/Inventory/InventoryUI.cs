using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel; // �κ��丮 UI ������
    bool activeInventory = false;   // Ȱ��ȭ ���� ����

    [SerializeField] Slot[] slots;
    [SerializeField] Transform slotHolder;


    private void Start()
    {
        slots = slotHolder.GetComponentsInChildren<Slot>(); // slots �� ������ ��� �ڽ� ������Ʈ�� ������
        inventoryPanel.SetActive(activeInventory);  // ���۽� �κ��丮 �� Ȱ��ȭ
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))    // i ��ư�� ������
        {
            activeInventory = !activeInventory; // �κ��丮 Ȱ��ȭ
            inventoryPanel.SetActive(activeInventory);  // SetActive �� ������
        }
    }
}
