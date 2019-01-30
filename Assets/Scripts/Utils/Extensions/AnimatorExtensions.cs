using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimatorExtensions {

    public static bool HasTrigger(this Animator animator, string name)
    {
        return animator.parameters.Any(param => param.type == AnimatorControllerParameterType.Trigger && param.name == name);
    }
}
