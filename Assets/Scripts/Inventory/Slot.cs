using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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

    public void SetSlotCount(int _count)    // 아이템 슬롯 갯수 업데이트
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

        text_Count.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)  // 마우스 클릭/터치에 대한 이벤트
    {
        if (eventData.button == PointerEventData.InputButton.Right) // PointerEventData 는 마우스 클릭/터치에 대한 정보들이 있다. 이벤트된 버튼, 클릭 수, 마우스 위치, 마우스의 움직임 여부 등
        {                                                           // 마우스 우클릭을 눌렀을때
            if (item != null)       // 선택된 아이템이 null 이 아니라면
            {
                if (item.itemType == Item.ItemType.Equipment)   // 선택한 item 의 itemType 이 Equipment 라면
                {
                    // 장착 하기 (새로 만들어야함)
                }
            }
            else if (item.itemType == Item.ItemType.Consumables) // 선택한 item의 itemType 이 Consumable 이면
            {
                Debug.Log(item.itemName + " 사용");
                SetSlotCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) // 마우스 드래그 시작 시 호출
    {
        if (item != null)   // 아이템이 빈게 아니라면
        {
            DragSlot.instance.dragSlot = this;  // 드래그 슬롯에 자기 자신을 할당
            DragSlot.instance.DragSetImage(itemImage);  // 드래그 슬롯의 이미지를 자기자신의 이미지로 변경
            DragSlot.instance.transform.position = eventData.position;  // 드래그슬롯의 위치를 이벤트가 발생한 마우스 위치로 변경
        }
    }

    public void OnDrag(PointerEventData eventData)  // 마우스 드래그 중에 계속 호출
    {
        if (item != null)   // 아이템이 빈게 아니라면
        {
            DragSlot.instance.transform.position = eventData.position;  // 드래그되고 있는 아이템을 계속 마우스를 따라 움직임
        }
    }

    public void OnEndDrag(PointerEventData eventData)   // 마우스 드래그가 끝났을 때 호출
    {
            DragSlot.instance.dragSlot = null;  // 드래그 슬롯 비움
            DragSlot.instance.SetColor(0);  // 드래그 슬롯을 다시 투명화
    }

    public void OnDrop(PointerEventData eventData)  // 슬롯에 무언가 드랍됬을때 호출
    {
        if (DragSlot.instance.dragSlot != null) // 드래그된 슬롯에 무언가 있다면
        {
            ChangeSlot(); // 두개의 슬롯을 바꿈
        }
    }

    // OnEndDrag 는 드래그 되고있던 대상이 끝났을때 호출되는것, 드래그 되고있던 대상에게서 호출됨
    // OnDrop 은 무언가 나한테 드랍되었을때 호출된다. 드래그를 멈춘 위치에 있는 대상에게서 호출됨

    private void ChangeSlot()   // 슬롯 서로 바꾸기
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount; // 바꿀 아이템의 정보를 담고있을 임시 변수

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount); // 옮기려고 했던 아이템을 위치에 추가

        if (_tempItem != null)  // 드래그한 슬롯이 빈슬롯이 아니라면
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);  // 서로 위치를 바꿈
        }
        else                    // 빈슬롯이라면
        {
            DragSlot.instance.dragSlot.ClearSlot(); // 옮기려고 했던 아이템이 있던 위치를 비움 
        }
    }
}
