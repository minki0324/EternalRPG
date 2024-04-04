using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleResult : MonoBehaviour
{
    [SerializeField] private ActiveCanvas activeCanvas;
    [SerializeField] private Player player;
    [SerializeField] private ResultPanelTitle resultPanelTitle;
    public Monster mon;
    public bool isWin = false;

    [Header("UI")]
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text APText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text totalGoldText;
    public GameObject DataPanel = null;

    private void OnEnable()
    {
        InitData();
    }

    private void InitData()
    {
        if (isWin)
        {
            resultPanelTitle.Result = true;
            StartCoroutine(AccountBattle_Co());
        }
        else if (!isWin)
        {
            resultPanelTitle.Result = false;
            expText.text = "0";
            // todo ������ �ý��� �־�ߵ�
            levelText.text = "0";
            APText.text = "0";
            goldText.text = "0";
            totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
        }
    }

    private IEnumerator AccountBattle_Co()
    { // ���� ���
        // ����ġ
        GameManager.Instance.CurrentEXP += (int)mon.monsterData.RewordEXP;
        int levelup = 0;
        while (GameManager.Instance.RequireEXP < GameManager.Instance.CurrentEXP)
        {
            GameManager.Instance.CurrentEXP -= GameManager.Instance.RequireEXP;
            GameManager.Instance.RequireEXP = (int)(GameManager.Instance.RequireEXP * 1.25);
            GameManager.Instance.PlayerLevel++;
            GameManager.Instance.CurrentAP += 5;
            levelup++;
            yield return null;
        }
        // ���
        int gold = Mathf.RoundToInt(Random.Range(mon.monsterData.RewordGold * 0.95f, mon.monsterData.RewordGold * 1.05f));
        GameManager.Instance.Gold += gold;

        // todo ������

        expText.text = $"{mon.monsterData.RewordEXP:N0}";
        levelText.text = $"{levelup} UP";
        APText.text = $"+ {levelup * 5}";
        goldText.text = $"{gold:N0}";
        totalGoldText.text = $"{GameManager.Instance.Gold:N0}";
        DataPanel.SetActive(true);
    }

    public void BackButton()
    {
        activeCanvas.resultPanel.SetActive(false);
        activeCanvas.versusPanel.SetActive(false);
        activeCanvas.gameObject.SetActive(false);

        player.playerMove.isFight = false;
        // ���� �Ŀ��� ���Ϳ� ����� ��ġ��ŭ �������� ����
        player.gameObject.transform.position = new Vector2(mon.gameObject.transform.position.x + mon.monsterData.ReturnPos.x,
                                                                                                                        mon.gameObject.transform.position.y + mon.monsterData.ReturnPos.y);

        if(isWin)
        { // todo �������� �¸� ���� �� ���İ� �����ִ°� �����ؾ���
            mon.isDead = true;
            mon.animator.enabled = false;
        }
        else
        {
            mon.isDead = false;
        }
        resultPanelTitle.TitleText.gameObject.SetActive(false);
        DataPanel.SetActive(false);
        PrintLog.Instance.BattleLogClear();
    }
}
