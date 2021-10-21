using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatusController : MonoBehaviour
{
    // 스태미나 상태 체크를 위한 PlayerMovement
    private PlayerMovement state;

    // HP 상태 변수들
    [SerializeField] Image hp_Gauge;        // HP 게이지
    [SerializeField] TextMeshProUGUI countCurrentHP;    // 현재 HP
    [SerializeField] private int maxHP;     // 최대 HP
    [SerializeField] private int currentHP; // 현재 HP

    // ST 상태 변수들
    [SerializeField] GameObject staminaBar; // 스테미나박스
    [SerializeField] Image status_Gauge;    // ST 게이지
    [SerializeField] private float maxST;     // 최대 ST
    [SerializeField] private float currentST; // 현재 ST
    private bool stUsing = false;           // 스태미나 사용중인지 체크
    private float stRechargeTime = 100f;       // 스태미나 자동 회복까지 걸리는 시간
    private float currentST_RechargeTime = 0; // 현재 자동회복까지 남은 시간

    #region 게터
    public float GetcurrnetST()
    {
        return currentST;
    }
    #endregion

    private void Start()
    {
        state = FindObjectOfType<PlayerMovement>();
        countCurrentHP.text = maxHP + " / " + currentHP;    // HP바의 텍스트를 maxHP / currentHP 로 설정
    }

    private void Update()
    {
        ShowStaminaBar();
        if (state.Getrun() || state.Getbroom()) { stUsing = true; DecreaseST(0.02f); }  //  달리거나 빗자루를 타고 있으면 스태미나 감소
        else { stUsing = false; }   // 뛰거나 빗자루를 타고 있는 상태가 아니면 스태미나 사용하지 않음 상태로 변경
        STRechargeTime();
        IncreaseST();

        GaugeUpdate();
    }

    private void ShowStaminaBar()   // 스태미나 바 보여주기
    {
        if (stUsing || currentST < maxST ) { staminaBar.SetActive(true); }    // 스태미나 사용중이거나, 스태미나 회복중일때는 스태미나 바를 보여줌
        else { staminaBar.SetActive(false); }   // 그렇지 않으면 숨김
    }

    private void GaugeUpdate()  // HP,ST 게이지 업데이트
    {
        hp_Gauge.fillAmount = (float)currentHP / maxHP;
        status_Gauge.fillAmount = (float)currentST / maxST;
    }

    public void IncreaseHP(int _count) // HP 회복시
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

    public void DecreaseHP(int _count) // HP 감소시
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
            Debug.Log("사망했습니다.");
        }
    }

    private void STRechargeTime()   // ST 자동 회복 시간 계산
    {
        if (!stUsing)
        {
            if (currentST_RechargeTime <= stRechargeTime)
                currentST_RechargeTime += 0.03f;
            else if (currentST_RechargeTime > stRechargeTime)
                currentST_RechargeTime = stRechargeTime;
        }
    }

    private void IncreaseST()   // ST 회복
    {
        if (!stUsing && currentST < maxST && currentST_RechargeTime == stRechargeTime)
        {
            currentST += 0.05f;
        }
    }

    public void DecreaseST(float _count) // 스태미나 사용시
    {
        stUsing = true;
        currentST_RechargeTime = 0;

        if ((currentST - _count) > 0) { currentST -= _count; }
        else
        { currentST = 0; }
    }
}
