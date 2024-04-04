using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMove playerMove;
    [HideInInspector] public PlayerData playerData;

    private void Awake()
    {
        TryGetComponent(out playerData);
        TryGetComponent(out playerMove);
    }
}
