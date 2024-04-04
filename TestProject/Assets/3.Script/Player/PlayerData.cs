using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        TryGetComponent(out player);
    }

    
}
