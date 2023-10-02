using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventSystem.current.onPlayerLand();
    }
}
