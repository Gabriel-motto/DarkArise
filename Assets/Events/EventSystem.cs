using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action PlayerLand;
    public void onPlayerLand()
    {
        PlayerLand?.Invoke();
    }
}
