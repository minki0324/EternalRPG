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
        // 높이를 0부터 시작해서 300까지 스크롤합니다.
        float scrollSpeed = 750f; // 스크롤 속도 조절
        while (myRect.sizeDelta.y < 400f)
        {
            // 높이를 증가시켜 스크롤 효과를 구현합니다.
            myRect.sizeDelta += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }

        // 승리 or 실패 넣어줘야함
        TitleText.gameObject.SetActive(true);
        if (Result)
        {
            TitleText.color = new Color(1f, 1f, 0f); // 노란색
            TitleText.text = "승리 !";
        }
        else
        {
            TitleText.color = new Color(1f, 0f, 0f); // 빨간색
            TitleText.text = "패배..";
        }
    }

}
