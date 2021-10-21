using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatusController : MonoBehaviour
{
    // ���¹̳� ���� üũ�� ���� PlayerMovement
    private PlayerMovement state;

    // HP ���� ������
    [SerializeField] Image hp_Gauge;        // HP ������
    [SerializeField] TextMeshProUGUI countCurrentHP;    // ���� HP
    [SerializeField] private int maxHP;     // �ִ� HP
    [SerializeField] private int currentHP; // ���� HP

    // ST ���� ������
    [SerializeField] GameObject staminaBar; // ���׹̳��ڽ�
    [SerializeField] Image status_Gauge;    // ST ������
    [SerializeField] private float maxST;     // �ִ� ST
    [SerializeField] private float currentST; // ���� ST
    private bool stUsing = false;           // ���¹̳� ��������� üũ
    private float stRechargeTime = 100f;       // ���¹̳� �ڵ� ȸ������ �ɸ��� �ð�
    private float currentST_RechargeTime = 0; // ���� �ڵ�ȸ������ ���� �ð�

    #region ����
    public float GetcurrnetST()
    {
        return currentST;
    }
    #endregion

    private void Start()
    {
        state = FindObjectOfType<PlayerMovement>();
        countCurrentHP.text = maxHP + " / " + currentHP;    // HP���� �ؽ�Ʈ�� maxHP / currentHP �� ����
    }

    private void Update()
    {
        ShowStaminaBar();
        if (state.Getrun() || state.Getbroom()) { stUsing = true; DecreaseST(0.02f); }  //  �޸��ų� ���ڷ縦 Ÿ�� ������ ���¹̳� ����
        else { stUsing = false; }   // �ٰų� ���ڷ縦 Ÿ�� �ִ� ���°� �ƴϸ� ���¹̳� ������� ���� ���·� ����
        STRechargeTime();
        IncreaseST();

        GaugeUpdate();
    }

    private void ShowStaminaBar()   // ���¹̳� �� �����ֱ�
    {
        if (stUsing || currentST < maxST ) { staminaBar.SetActive(true); }    // ���¹̳� ������̰ų�, ���¹̳� ȸ�����϶��� ���¹̳� �ٸ� ������
        else { staminaBar.SetActive(false); }   // �׷��� ������ ����
    }

    private void GaugeUpdate()  // HP,ST ������ ������Ʈ
    {
        hp_Gauge.fillAmount = (float)currentHP / maxHP;
        status_Gauge.fillAmount = (float)currentST / maxST;
    }

    public void IncreaseHP(int _count) // HP ȸ����
    {
        if ((currentHP + _count) < maxHP)
        {
            currentHP += _count;
            countCurrentHP.text = maxHP + " / " + currentHP;
        }
        else
        {
            currentHP = maxHP;
            countCurrentHP.text = maxHP + " / " + currentHP;
        }
    }

    public void DecreaseHP(int _count) // HP ���ҽ�
    {
        if ((currentHP - _count) > 0)
        {
            currentHP -= _count;
            countCurrentHP.text = maxHP + " / " + currentHP;
        }
        else
        {
            currentHP = 0;
            countCurrentHP.text = maxHP + " / " + currentHP;
            Debug.Log("����߽��ϴ�.");
        }
    }

    private void STRechargeTime()   // ST �ڵ� ȸ�� �ð� ���
    {
        if (!stUsing)
        {
            if (currentST_RechargeTime <= stRechargeTime)
                currentST_RechargeTime += 0.03f;
            else if (currentST_RechargeTime > stRechargeTime)
                currentST_RechargeTime = stRechargeTime;
        }
    }

    private void IncreaseST()   // ST ȸ��
    {
        if (!stUsing && currentST < maxST && currentST_RechargeTime == stRechargeTime)
        {
            currentST += 0.05f;
        }
    }

    public void DecreaseST(float _count) // ���¹̳� ����
    {
        stUsing = true;
        currentST_RechargeTime = 0;

        if ((currentST - _count) > 0) { currentST -= _count; }
        else
        { currentST = 0; }
    }
}
