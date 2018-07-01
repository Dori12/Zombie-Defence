using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour {

    public enum CState { Nothing, Stand, Standing, Crouch, Crouching, Jump }
    private CState state;
    private bool isCrouch;
    private bool isJump;
    private float speed;
    private float walkSpeed = 50.0f;
    private float runSpeed = 120.0f;
    private float translateSpeed = 0.05f;
    private Animator animator;
    private Vector3 direction;
    private Vector3 MoveDir;
    private float gravity = 3.0f;
    private float stackGravity;
    private bool prevGrounded;

    private Transform tr;
	// Use this for initialization
	void Start () {
        isCrouch = false;
        animator = GetComponent<Animator>();
        speed = 0.0f;
        tr = GetComponent<Transform>();
        state = CState.Stand;
        stackGravity = 0.0f;
        MoveDir = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        RotateHorizontal();
        Crouch();
        Run();
        MoveDir *= (speed * translateSpeed);
        tr.Translate(MoveDir * Time.deltaTime);
    }

    void Jump()
    {
    }

    void Run()
    {
        if(state != CState.Stand)
        {
            return;
        }

        bool _isWalk = false;

        MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        Vector2 XZpower = new Vector2(MoveDir.x, MoveDir.z);
        if(XZpower.magnitude > 0.0f)
        {
            _isWalk = true;
        }
        else
        {
            _isWalk = false;
        }

        if(_isWalk)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = Mathf.Lerp(speed, runSpeed, 3.0f * Time.deltaTime);
                WalkAnimatorSet(XZpower);
            }
            else
            {
                speed = Mathf.Lerp(speed, walkSpeed, 3.0f * Time.deltaTime);
                WalkAnimatorSet(XZpower);
            }
        }
        else
        {
            speed = Mathf.Lerp(speed, 0.0f, 3.0f * Time.deltaTime);
            if(speed < 10.0f)
            {
                speed = 0.0f;
            }
            WalkAnimatorSet(XZpower);
        }
    }

    void WalkAnimatorSet(Vector2 _direction)
    {
        animator.SetFloat("Speed", speed);
        animator.SetFloat("dirX", _direction.x);
        animator.SetFloat("dirZ", _direction.y);
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            speed = 0.0f;
            isCrouch = !isCrouch;
            animator.SetBool("IsCrouch", isCrouch);
            if (isCrouch)
            {
                state = CState.Crouching;
            }
            else
            {
                state = CState.Standing;
            }
        }
    }

    void RotateHorizontal()
    {
        float mouseX = Input.GetAxis("Mouse X");
        tr.Rotate(new Vector3(0.0f, mouseX));
    }

    void StandingEnd()
    {
        animator.SetBool("StandingEnd", true);
        animator.SetBool("CrouchingEnd", false);
        state = CState.Stand;
    }

    void CrouchingEnd()
    {
        animator.SetBool("CrouchingEnd", true);
        animator.SetBool("StandingEnd", false);
        state = CState.Crouch;
    }
}