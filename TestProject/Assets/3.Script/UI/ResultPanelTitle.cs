using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanelTitle : MonoBehaviour
{
    public bool Result = false;
    public TMP_Text TitleText = null;
    private RectTransform myRect = null;
    
    private void Awake()
    {
        myRect = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        myRect.sizeDelta = new Vector2(myRect.sizeDelta.x, 0);
        StartCoroutine(Scrolling(Result));
    }

    private IEnumerator Scrolling(bool _result)
    {
        // ���̸� 0���� �����ؼ� 300���� ��ũ���մϴ�.
        float scrollSpeed = 750f; // ��ũ�� �ӵ� ����
        while (myRect.sizeDelta.y < 400f)
        {
            // ���̸� �������� ��ũ�� ȿ���� �����մϴ�.
            myRect.sizeDelta += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }

        // �¸� or ���� �־������
        TitleText.gameObject.SetActive(true);
        if (Result)
        {
            TitleText.color = new Color(1f, 1f, 0f); // �����
            TitleText.text = "�¸� !";
        }
        else
        {
            TitleText.color = new Color(1f, 0f, 0f); // ������
            TitleText.text = "�й�..";
        }
    }

}
