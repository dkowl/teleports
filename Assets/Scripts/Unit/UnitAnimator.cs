using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimator : MonoBehaviour {

    Unit unit;
    Animator animator;

    int moveSpeedHash;
    int isMovingHash;
    int attackHash, attackStartHash, attackResetHash;

    void Awake()
    {
        unit = GetComponentInParent<Unit>();
        animator = GetComponent<Animator>();

        moveSpeedHash = Animator.StringToHash("moveSpeed");
        isMovingHash = Animator.StringToHash("isMoving");
        attackHash = Animator.StringToHash("cast");
        attackStartHash = Animator.StringToHash("castStart");
        attackResetHash = Animator.StringToHash("castReset");
    }

	// Use this for initialization
	void Start () {
        unit.castEvent += HandleCastEvent;
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetFloat(moveSpeedHash, unit.MoveSpeed);
        animator.SetBool(isMovingHash, unit.IsMoving);
	}

    void HandleCastEvent(object sender, CastEventArgs e)
    {
        if(e.Skill is Attack) animator.SetTrigger(attackHash);
    }
}
