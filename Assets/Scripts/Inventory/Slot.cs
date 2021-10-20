using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;       // 획득한 아이템
    public int itemCount;   // 해당 아이템의 갯수
    public Image itemImage; // 인벤토리에서 보여질 아이템의 이미지

    [SerializeField] public Text text_Count;

    private void SetColor(float _alpha) // 이미지 투명도 조절
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1) // 슬롯에 아이템 추가
    {
        item = _item;    // 아이템의 정보
        itemCount = _count; // 아이템 갯수
        itemImage.sprite = item.itemImage; // 아이템 이미지

        if (item.itemType != Item.ItemType.Equipment)   // 들어온 아이템이 장비가 아니라면
        {
            text_Count.text = itemCount.ToString();     // 아이템 갯수 표시 활성화
        }

        SetColor(1);    // 이미지 투명도 1로 변경하여 이미지가 보이도록
    }

    public void setSlotCount(int _count)    // 아이템 슬롯 갯수 업데이트
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString(); // 증가한 아이템 갯수 표시

        if (itemCount <= 0) { ClearSlot(); }    // 아이템이 하나도 없으면 해당 슬롯을 비움
    }

    private void ClearSlot()    // 슬롯 비우기
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);                // 아이템 정보, 갯수, 이미지, 투명도 초기화

        text_Count.text = "0";
    }
}
