using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField] private float range; // 아이템 습득 최대 거리
    [SerializeField] private InventoryUI inventory; // 플레이어의 인벤토리

    private bool pickUpActivated = false; // 아이템 습득 가능 여부 
    private bool talkNPC = false;         // NPC 와 대화 가능 여부
    NPCData targetNPC;
    private int talkIndex;

    // 충돌체 정보 저장
    private Collider[] pickItem;          
    private Collider[] npc; 

    // 특정 레이어를 가진 오브젝트만 인지
    [SerializeField] private LayerMask ItemlayerMask; 
    [SerializeField] private LayerMask NPC_layerMask;

    [SerializeField] private GameObject itemInfoBar;    // actionText 가 표시될 배경
    [SerializeField] private TextMeshProUGUI actionText;     // 플레이어가 취할 행동을 표시할 텍스트

    public TalkManager talkManager;

    void Update()
    {
        CheckAround();
        if (Input.GetKeyDown(KeyCode.F)) // F버튼을 누르면 아이템을 줏음
        {
            if (pickUpActivated) { PickUp(); }
            else if (talkNPC) { ChatNPC(targetNPC.id, targetNPC.isNPC); }
        }
    }

    private void CheckAround()    // 주울 수 있는 아이템이 주변에 있는지 체크
    {
        pickItem = Physics.OverlapSphere(transform.position, range, ItemlayerMask); // 플레이어 주변 range 만큼에 item 레이어가 있으면 그 정보를 pickItem 에 저장
                                                                                    // 배열 저장 순서는 먼저 감지된 아이템 > 가까운 아이템 순서
        npc = Physics.OverlapSphere(transform.position, range, NPC_layerMask);
        if (pickItem.Length > 0 || npc.Length > 0)   // pickItem 의 정보가 있으면
        {
            #region 디버그 용
            /* 
            for (int i = 0; i < pickItem.Length; i++)
            {
                Debug.Log(i + "번 아이템 " + pickItem[i]);
            } 
            */
            #endregion
            Showinfo(); // 해당 정보를 출력
        }
        else 
        {
            Cancel_info();  // 감지된 오브젝트가 없으므로 정보 출력 취소
        }
    }

    private void Showinfo() // 감지된 물체들 정보 표시
    {
        actionText.gameObject.SetActive(true);
        itemInfoBar.SetActive(true);

        if (pickItem.Length > 0)    // 아이템 감지시
        {
            pickUpActivated = true;
            actionText.text = pickItem[0].GetComponent<ItemPickup>().item.name + " 줍기";  // 0번 인덱스에 저장된 아이템 부터 표시
        }
        else if (npc.Length > 0)    // npc 감지 시
        { 
            talkNPC = true;
            targetNPC = npc[0].GetComponent<NPCData>();
            actionText.text = npc[0].name + " 와 대화하기";
        }
    }

    private void Cancel_info()  // 감지에서 벗어나면 정보 표시 삭제
    {
        pickUpActivated = false;
        actionText.gameObject.SetActive(false); // 표시된 pickItem UI 안보이게 변경
        itemInfoBar.SetActive(false);
    }

    private void PickUp()   // 아이템 줍기
    {
        if (pickUpActivated)    // 아이템을 주울 수 있는 상태면
        {
            Debug.Log(pickItem[0].name + " 획득");
            inventory.PickUpItem_Add(pickItem[0].GetComponent<ItemPickup>().item);
            Destroy(pickItem[0].gameObject);    // 주운 아이템이 월드에서 없어지도록 함
        }
    }

    private void ChatNPC(int id, bool isNPC)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null) { talkNPC = false;  return; }
        if (isNPC)
        {
            talkManager.chatNPC_Panel.SetActive(true);
            talkManager.talkPanel.text = talkData;
        }
    }
}
