using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {

    [HideInInspector]  public bool isCrouch;
    [HideInInspector] public bool isJump;
    [HideInInspector] public bool isCrouchingEnd;
    [HideInInspector] public bool isStandingEnd;
    [HideInInspector] public float speed;
    [HideInInspector] public float walkSpeed = 50.0f;
    [HideInInspector] public float runSpeed = 120.0f;
    [HideInInspector] public float translateSpeed = 0.05f;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 MoveDir;
    [HideInInspector] public Vector3 InputDir;
    [HideInInspector] public float gravity = 20.0f;
    public float stackGravity = 0.0f;
    [HideInInspector] public bool prevGrounded;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public StateMachine<CharacterMove> stateMachine;
    [HideInInspector] public Transform tr;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        isCrouch = false;

        speed = 0.0f;
        stackGravity = 0.0f;

        MoveDir = Vector3.zero;


        stateMachine = new StateMachine<CharacterMove>(this);
        stateMachine.SetCurrentState(StandState.GetInstance());
        stateMachine.SetGlobalState(CharacterGlobalState.GetInstance());
	}
	
	// Update is called once per frame
	void Update () {
        stateMachine.Update();
    }

    public void WalkAnimatorSet(Vector3 _direction)
    {
        animator.SetFloat("Speed", speed);
        animator.SetFloat("dirX", _direction.x);
        animator.SetFloat("dirZ", _direction.z);
    }

    public void RotateHorizontal()
    {
        float mouseX = Input.GetAxis("Mouse X");
        tr.Rotate(new Vector3(0.0f, mouseX));
    }

    void StandingEnd()
    {
        isStandingEnd = true;
        animator.SetBool("StandingEnd", true);
    }

    void CrouchingEnd()
    {
        isCrouchingEnd = true;
        animator.SetBool("CrouchingEnd", true);
    }
}