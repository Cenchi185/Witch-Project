using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // string 이 배열인 이유는 NPC가 여러 문장을 말할 수도 있으므로 

    public GameObject chatNPC_Panel;
    public TextMeshProUGUI talkPanel;

    // Start is called before the first frame update
    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        Generate();
    }

    private void Start()
    {
        chatNPC_Panel.SetActive(false);
    }

    // Update is called once per frame
    void Generate() // 대화문 생성
    {
        talkData.Add(1, new string[] { "NPC와 대화하기 테스트입니다.", "두번째문장", "세번째 문장" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length) { return null; }
        else { return talkData[id][talkIndex]; }
    }
}
