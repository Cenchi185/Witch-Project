using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData; // string �� �迭�� ������ NPC�� ���� ������ ���� ���� �����Ƿ� 

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
    void Generate() // ��ȭ�� ����
    {
        talkData.Add(1, new string[] { "NPC�� ��ȭ�ϱ� �׽�Ʈ�Դϴ�.", "�ι�°����", "����° ����" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length) { return null; }
        else { return talkData[id][talkIndex]; }
    }
}
