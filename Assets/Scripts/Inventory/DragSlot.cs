using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public DragSlot instance;    // static 으로 선언하면 게임 내내 instance 에 담긴 내용이 메모리에 유지됨
    public Slot dragSlot;   // 드래그 된 슬롯의 정보

    private Image dragImage;
    [SerializeField] Image imageItem;

    // Start is called before the first frame update
    void Start()
    {
        dragImage = this.GetComponent<Image>();
        instance = this;
    }

    public void OnBeginDrag(PointerEventData eventData) // 이 스크립트가 있는 오브젝트 드래그 시작 시 호출
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)  // 이 스크립트가 있는 오브젝트의 드래그중 호출
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)   // 이 스크랩트가 있는 오브젝트 드래그가 끝났을때 호출
    {
        throw new System.NotImplementedException();
    }

    public void DragSetImage(Image _itemImage)  // 드래그될 슬롯의 이미지 가져옴
    {
        imageItem.sprite = _itemImage.sprite;   // 드래그될 슬롯의 이미지를 드래그하는 슬롯으로 복사
        SetColor(1);
    }

    public void SetColor(float _alpha)  // 드래그 슬롯의 투명도 조절
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
        dragImage.color = color;
    }
}
