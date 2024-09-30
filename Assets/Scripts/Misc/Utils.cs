using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void SetAllBoolFalse(Animator animator)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter != null)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }
}
