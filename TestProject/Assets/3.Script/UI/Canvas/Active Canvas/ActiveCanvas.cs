using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCanvas : MonoBehaviour
{
    [Header("Panel")]
    public GameObject versusPanel;
    public GameObject resultPanel;

    public void BattleSpeed(int _rate)
    {
        GameManager.Instance.BattleSpeed = _rate;
    }
}
