using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool activeInventory = false;

    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
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
