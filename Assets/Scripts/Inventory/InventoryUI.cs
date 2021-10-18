using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel; // 인벤토리 UI 가져옴
    bool activeInventory = false;   // 활성화 상태 변수

    [SerializeField] Slot[] slots;
    [SerializeField] Transform slotHolder;


    private void Start()
    {
        slots = slotHolder.GetComponentsInChildren<Slot>(); // slots 에 지정된 모든 자식 컴포넌트를 가져옴
        inventoryPanel.SetActive(activeInventory);  // 시작시 인벤토리 비 활성화
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
