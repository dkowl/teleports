using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {

    public Unit unit_;
    public Animator animator_;

    int moveSpeedHash_ = Animator.StringToHash("moveSpeed");
    int isMovingHash_ = Animator.StringToHash("isMoving");
    int attackHash_ = Animator.StringToHash("attack");

	// Use this for initialization
	void Start () {
        unit_.castEvent += HandleCastEvent;
	}
	
	// Update is called once per frame
	void Update () {
        animator_.SetFloat(moveSpeedHash_, unit_.MoveSpeed);
        animator_.SetBool(isMovingHash_, unit_.IsMoving);
	}

    void HandleCastEvent(object sender, CastEventArgs e)
    {
        if(e.Skill is Attack) animator_.SetTrigger(attackHash_);
    }
}
