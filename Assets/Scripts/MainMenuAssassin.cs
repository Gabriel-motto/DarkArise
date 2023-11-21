using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAssassin : MonoBehaviour
{
    private Animator player;

    void Start()
    {
        player = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        player.SetTrigger("TrRun");
    }
}
