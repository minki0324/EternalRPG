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
        // �ʱ� AP �� ����
        int initialAP = GameManager.Instance.MasterCurrentAP;

        // MasterRunePoint, MasterBonusAPPoint, MasterDropPoint, MasterMovePoint, MasterEnergyPoint, MasterGemPoint�� ����Ʈ�� �߰�
        initialAP += GameManager.Instance.MasterRunePoint * 200;
        initialAP += GameManager.Instance.MasterBonusAPPoint * 100;
        initialAP += GameManager.Instance.MasterDropPoint;
        initialAP += GameManager.Instance.MasterMovePoint * 10;
        initialAP += GameManager.Instance.MasterEnergyPoint * 30;
        initialAP += GameManager.Instance.MasterGemPoint * 10;

        // ���� AP ���� �ʱ�ȭ�ϰ� ���� ���� �� ����
        GameManager.Instance.MasterCurrentAP = initialAP;

        // ��� ����Ʈ�� 0���� �ʱ�ȭ
        GameManager.Instance.MasterRunePoint = 0;
        GameManager.Instance.MasterBonusAPPoint = 0;
        GameManager.Instance.MasterDropPoint = 0;
        GameManager.Instance.MasterMovePoint = 0;
        GameManager.Instance.BonusEnergy -= GameManager.Instance.MasterEnergyPoint; // �������� ������ ����
        GameManager.Instance.MasterEnergyPoint = 0;
        GameManager.Instance.MasterGemPoint = 0;

        // �� �г��� ���� ����
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].RenewMasterStat();
        }

        // �ʱ�ȭ �� ������ ����
        InitData();

        // ��� �г� ��Ȱ��ȭ
        WarningPanel.SetActive(false);
    }
}
