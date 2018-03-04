using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionRedirector : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private Animator animator;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        targetTransform = GetComponentInParent<Unit>().transform;
    }

    private void OnAnimatorMove()
    {
        if(animator.deltaPosition.magnitude > 0) Debug.LogFormat("OnAnimator move {0:F5} {1:F5} {2:F5}", animator.deltaPosition.x, animator.deltaPosition.y, animator.deltaPosition.z);
        targetTransform.position += animator.deltaPosition;
    }
}
