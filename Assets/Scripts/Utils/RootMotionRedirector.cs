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
    }

    private void OnAnimatorMove()
    {
        targetTransform.position += animator.deltaPosition;
    }
}
