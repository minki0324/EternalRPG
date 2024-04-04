using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PrintLog : MonoBehaviour
{
    public static PrintLog Instance = null;

    [SerializeField] private GameObject[] battleLogObj;
    [SerializeField] private TMP_Text[] battleLogs;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public void BattleLog(string _logText)
    {
        // �α� ����ִ� �κ��� �ִٸ� �ش� ��ġ�� �α� ���

        for (int i = 0; i < battleLogs.Length; i++)
        {
            if (battleLogs[i].text == string.Empty)
            {
                battleLogObj[i].SetActive(true);
                battleLogs[i].text = _logText;
                return;
            }
        }

        // �α� ����ִ� �κ��� ���ٸ� �о��
        // ��� �αװ� �� ���ִ� ���
        // ���� ������ �α׸� �����ϰ�, ���ο� �α׸� �迭�� ���� �ڿ� �߰�.
        for (int i = 0; i < battleLogs.Length - 1; i++)
        {
            battleLogs[i].text = battleLogs[i + 1].text;
        }

        // ���� ������ �α� ��ġ�� ���ο� �α� �߰�
        battleLogs[battleLogs.Length - 1].text = _logText;
    }

    public void BattleLogClear()
    {
        for(int i = 0; i < battleLogs.Length; i++)
        {
            battleLogs[i].text = string.Empty;
            battleLogObj[i].SetActive(false);
        }
    }
}
