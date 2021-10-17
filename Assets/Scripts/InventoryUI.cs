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
        if (Input.GetKeyDown(KeyCode.I))    // i 버튼을 누르면
        {
            activeInventory = !activeInventory; // 인벤토리 활성화
            inventoryPanel.SetActive(activeInventory);  // SetActive 로 보여줌
        }
    }
}
