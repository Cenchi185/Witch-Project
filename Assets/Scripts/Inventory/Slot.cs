using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;       // ȹ���� ������
    public int itemCount;   // �ش� �������� ����
    public Image itemImage; // �κ��丮���� ������ �������� �̹���

    [SerializeField] public Text text_Count;

    private void SetColor(float _alpha) // �̹��� ���� ����
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1) // ���Կ� ������ �߰�
    {
        item = _item;    // �������� ����
        itemCount = _count; // ������ ����
        itemImage.sprite = item.itemImage; // ������ �̹���

        if (item.itemType != Item.ItemType.Equipment)   // ���� �������� ��� �ƴ϶��
        {
            text_Count.text = itemCount.ToString();     // ������ ���� ǥ�� Ȱ��ȭ
        }

        SetColor(1);    // �̹��� ���� 1�� �����Ͽ� �̹����� ���̵���
    }

    public void SetSlotCount(int _count)    // ������ ���� ���� ������Ʈ
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString(); // ������ ������ ���� ǥ��

        if (itemCount <= 0) { ClearSlot(); }    // �������� �ϳ��� ������ �ش� ������ ���
    }

    private void ClearSlot()    // ���� ����
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);                // ������ ����, ����, �̹���, ���� �ʱ�ȭ

        text_Count.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)  // ���콺 Ŭ��/��ġ�� ���� �̺�Ʈ
    {
        if (eventData.button == PointerEventData.InputButton.Right) // PointerEventData �� ���콺 Ŭ��/��ġ�� ���� �������� �ִ�. �̺�Ʈ�� ��ư, Ŭ�� ��, ���콺 ��ġ, ���콺�� ������ ���� ��
        {                                                           // ���콺 ��Ŭ���� ��������
            if (item != null)       // ���õ� �������� null �� �ƴ϶��
            {
                if (item.itemType == Item.ItemType.Equipment)   // ������ item �� itemType �� Equipment ���
                {
                    // ���� �ϱ� (���� ��������)
                }
            }
            else if (item.itemType == Item.ItemType.Consumables) // ������ item�� itemType �� Consumable �̸�
            {
                Debug.Log(item.itemName + " ���");
                SetSlotCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) // ���콺 �巡�� ���� �� ȣ��
    {
        if (item != null)   // �������� ��� �ƴ϶��
        {
            DragSlot.instance.dragSlot = this;  // �巡�� ���Կ� �ڱ� �ڽ��� �Ҵ�
            DragSlot.instance.DragSetImage(itemImage);  // �巡�� ������ �̹����� �ڱ��ڽ��� �̹����� ����
            DragSlot.instance.transform.position = eventData.position;  // �巡�׽����� ��ġ�� �̺�Ʈ�� �߻��� ���콺 ��ġ�� ����
        }
    }

    public void OnDrag(PointerEventData eventData)  // ���콺 �巡�� �߿� ��� ȣ��
    {
        if (item != null)   // �������� ��� �ƴ϶��
        {
            DragSlot.instance.transform.position = eventData.position;  // �巡�׵ǰ� �ִ� �������� ��� ���콺�� ���� ������
        }
    }

    public void OnEndDrag(PointerEventData eventData)   // ���콺 �巡�װ� ������ �� ȣ��
    {
            DragSlot.instance.dragSlot = null;  // �巡�� ���� ���
            DragSlot.instance.SetColor(0);  // �巡�� ������ �ٽ� ����ȭ
    }

    public void OnDrop(PointerEventData eventData)  // ���Կ� ���� ��������� ȣ��
    {
        if (DragSlot.instance.dragSlot != null) // �巡�׵� ���Կ� ���� �ִٸ�
        {
            ChangeSlot(); // �ΰ��� ������ �ٲ�
        }
    }

    // OnEndDrag �� �巡�� �ǰ��ִ� ����� �������� ȣ��Ǵ°�, �巡�� �ǰ��ִ� ��󿡰Լ� ȣ���
    // OnDrop �� ���� ������ ����Ǿ����� ȣ��ȴ�. �巡�׸� ���� ��ġ�� �ִ� ��󿡰Լ� ȣ���

    private void ChangeSlot()   // ���� ���� �ٲٱ�
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount; // �ٲ� �������� ������ ������� �ӽ� ����

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount); // �ű���� �ߴ� �������� ��ġ�� �߰�

        if (_tempItem != null)  // �巡���� ������ �󽽷��� �ƴ϶��
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);  // ���� ��ġ�� �ٲ�
        }
        else                    // �󽽷��̶��
        {
            DragSlot.instance.dragSlot.ClearSlot(); // �ű���� �ߴ� �������� �ִ� ��ġ�� ��� 
        }
    }
}
