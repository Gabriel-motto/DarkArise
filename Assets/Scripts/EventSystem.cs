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

    public event Action OnPlayerLand;
    public void PlayerLand()
    {
        OnPlayerLand?.Invoke();
    }

    public event Action OnAnimationEnd;

    public void AnimationEndEvent()
    {
        OnAnimationEnd?.Invoke();
    }
}
