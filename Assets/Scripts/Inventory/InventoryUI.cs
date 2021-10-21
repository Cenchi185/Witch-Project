using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel; // 인벤토리 UI 가져옴
    bool activeInventory = false;   // 활성화 상태 변수

    [SerializeField] private GameObject slotsParent;    // 슬롯들의 부모인 Grid Setting

    [SerializeField] Slot[] slots;  // 슬롯의 배열

    private void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
        inventoryPanel.SetActive(activeInventory);  // 시작시 인벤토리 비 활성화
    }

    private void Update()
    {
        OpenInven();    
    }

    private void OpenInven()
    {
        if (Input.GetKeyDown(KeyCode.I))    // i 버튼을 누르면
        {
            activeInventory = !activeInventory; // 인벤토리 활성화 & 비활성화
            inventoryPanel.SetActive(activeInventory);  // SetActive 로 보여줌
        }
    }

    public void PickUpItem_Add(Item _item, int _count = 1) // 주운 아이템 인벤토리에 추가. 주운 아이템의 정보 _item, 추가할 갯수
    {
        if (Item.ItemType.Equipment != _item.itemType)  // 아이템 타입이 장비인지 확인
        {
            for (int i = 0; i < slots.Length; i++)  // 인벤토리 크기만큼 반복
            {
                if (slots[i].item != null)  // i번째 슬롯의 칸이 비어있지 않다면
                {
                    if (slots[i].item.itemName == _item.itemName)   // i번째 슬롯의 이름이 같다면
                    {
                        slots[i].SetSlotCount(_count);  // 갯수 증가
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)  // 비어있는 슬롯 찾음
            {
                slots[i].AddItem(_item, _count);    // 주운 아이템 추가.
                return;
            }
        }
    }
}
