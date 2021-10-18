using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private float range; // 아이템 습득 최대 거리

    private bool pickUpActivated = false; // 아이템 습득 가능 여부 
    private Collider[] pickItem;          // 충돌체 정보 저장

    [SerializeField] private LayerMask layerMask; // 특정 레이어를 가진 오브젝트만 인지하도록
    [SerializeField] private TextMeshProUGUI actionText;     // 플레이어가 취할 행동을 표시할 텍스트

    void Update()
    {
        CheckItem();
        if (Input.GetKeyDown(KeyCode.F)) // F버튼을 누르면 아이템을 줏음
        {
            PickUp();
        }
    }

    private void CheckItem()    // 주울 수 있는 아이템이 주변에 있는지 체크
    {
        pickItem = Physics.OverlapSphere(transform.position, range, layerMask); // 플레이어 주변 range 만큼에 지정한 layerMask 가 있으면 그 정보를 pickItem 에 저장
                                                                                // 배열 저장 순서는 플레이어에서 가까운 아이템 순서
        if (pickItem != null && pickItem.Length > 0)   // pickItem 의 정보가 있으면
        {
            #region 디버그 용
            /* 
            for (int i = 0; i < pickItem.Length; i++)
            {
                Debug.Log(i + "번 아이템 " + pickItem[i]);
            } 
            */
            #endregion
            ShowIteminfo(); // 해당 정보를 출력
        }
        else 
        {
            Cancel_Iteminfo();  // 감지된 오브젝트가 없으므로 정보 출력 취소
        }
    }

    private void ShowIteminfo() // 주울 수 있는 아이템 정보 표시
    {
        pickUpActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = pickItem[0].name + " 줍기";  // 0번 인덱스에 저장된 아이템 부터 표시(주움)
    }

    private void Cancel_Iteminfo()  // 감지에서 벗어나면 아이템 정보 표시 삭제
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false); // 표시된 pickItem UI 안보이게 변경
    }

    private void PickUp()   // 아이템 줍기
    {
        if (pickUpActivated)    // 아이템을 주울 수 있는 상태면
        {
            Debug.Log(pickItem[0].name + " 획득");
            Destroy(pickItem[0].gameObject);
        }
    }
}
