using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public DragSlot instance;    // static ���� �����ϸ� ���� ���� instance �� ��� ������ �޸𸮿� ������
    public Slot dragSlot;   // �巡�� �� ������ ����

    private Image dragImage;
    [SerializeField] Image imageItem;

    // Start is called before the first frame update
    void Start()
    {
        dragImage = this.GetComponent<Image>();
        instance = this;
    }

    public void OnBeginDrag(PointerEventData eventData) // �� ��ũ��Ʈ�� �ִ� ������Ʈ �巡�� ���� �� ȣ��
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)  // �� ��ũ��Ʈ�� �ִ� ������Ʈ�� �巡���� ȣ��
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)   // �� ��ũ��Ʈ�� �ִ� ������Ʈ �巡�װ� �������� ȣ��
    {
        throw new System.NotImplementedException();
    }

    public void DragSetImage(Image _itemImage)  // �巡�׵� ������ �̹��� ������
    {
        imageItem.sprite = _itemImage.sprite;   // �巡�׵� ������ �̹����� �巡���ϴ� �������� ����
        SetColor(1);
    }

    public void SetColor(float _alpha)  // �巡�� ������ ���� ����
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
        dragImage.color = color;
    }
}
