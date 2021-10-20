using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
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

    public void setSlotCount(int _count)    // ������ ���� ���� ������Ʈ
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

        text_Count.text = "0";
    }
}
