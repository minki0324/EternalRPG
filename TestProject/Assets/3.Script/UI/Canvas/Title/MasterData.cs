using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MasterData : MonoBehaviour
{
    [SerializeField] private TMP_Text MasterInfomationText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private MasterStatusPanel[] panels;
    [SerializeField] private Sprite[] badgeSprites;

    private void OnEnable()
    {
        InitData();
    }

    public void InitData()
    {
        MasterInfomationText.text = $": {GameManager.Instance.MasterLevel}\n" +
                                                             $": {GameManager.Instance.MasterCurrentEXP:N0} / {GameManager.Instance.MasterRequireEXP:N0}\n" +
                                                             $": {GameManager.Instance.MasterCurrentAP:N0}";
    }

    public void PointReset()
    {
        // 초기 AP 값 설정
        int initialAP = GameManager.Instance.MasterCurrentAP;

        // MasterRunePoint, MasterBonusAPPoint, MasterDropPoint, MasterMovePoint, MasterEnergyPoint, MasterGemPoint의 포인트를 추가
        initialAP += GameManager.Instance.MasterRunePoint * 200;
        initialAP += GameManager.Instance.MasterBonusAPPoint * 100;
        initialAP += GameManager.Instance.MasterDropPoint;
        initialAP += GameManager.Instance.MasterMovePoint * 10;
        initialAP += GameManager.Instance.MasterEnergyPoint * 30;
        initialAP += GameManager.Instance.MasterGemPoint * 10;

        // 현재 AP 값을 초기화하고 새로 계산된 값 설정
        GameManager.Instance.MasterCurrentAP = initialAP;

        // 모든 포인트를 0으로 초기화
        GameManager.Instance.MasterRunePoint = 0;
        GameManager.Instance.MasterBonusAPPoint = 0;
        GameManager.Instance.MasterDropPoint = 0;
        GameManager.Instance.MasterMovePoint = 0;
        GameManager.Instance.BonusEnergy -= GameManager.Instance.MasterEnergyPoint; // 에너지는 별도로 감소
        GameManager.Instance.MasterEnergyPoint = 0;
        GameManager.Instance.MasterGemPoint = 0;

        // 각 패널의 상태 갱신
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].RenewMasterStat();
        }

        // 초기화 후 데이터 갱신
        InitData();

        // 경고 패널 비활성화
        WarningPanel.SetActive(false);
    }
}
