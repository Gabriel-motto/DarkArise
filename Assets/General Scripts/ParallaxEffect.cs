using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform camTransform;
    private Vector3 lastCamPos;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPos = camTransform.position;
    }
}
