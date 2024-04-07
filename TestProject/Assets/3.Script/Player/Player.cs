using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerMove playerMove;

    private void Awake()
    {
        TryGetComponent(out playerMove);
    }
}
