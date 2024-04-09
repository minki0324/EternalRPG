using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEffect : MonoBehaviour
{
    [SerializeField] private Image effectPanel = null;
    private float delay = 0.2f;

    private void Awake()
    {
        if(effectPanel == null)
        {
            TryGetComponent(out effectPanel);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        effectPanel.fillAmount = 1f;
        yield return delay;
        while (effectPanel.fillAmount >= 0)
        {
            effectPanel.fillAmount -= 0.02f;
            yield return null;
        }
    }
}
