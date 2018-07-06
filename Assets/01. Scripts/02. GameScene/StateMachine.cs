using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State<T>
{
    void Enter(T t);
    void Excute(T t);
    void Exit(T t);
}

public class Singleton<T> where T : class, new()
{
    private static T Instance;
    public static T GetInstance()
    {
        if(Instance == null)
        {
            Instance = new T();
        }
        return Instance;
    }
}

public class StateMachine<entity_type>
{
    private entity_type m_Owner;
    private State<entity_type> m_CurrentState;
    private State<entity_type> m_PrevState;
    private State<entity_type> m_GlobalState;

    public StateMachine(entity_type owner)
    {
        m_Owner = owner;
        m_CurrentState = null;
        m_PrevState = null;
        m_GlobalState = null;
    }

    public void SetCurrentState(State<entity_type> s)
    {
        m_CurrentState = s;
    }

    public void SetGlobalState(State<entity_type> s)
    {
        m_GlobalState = s;
    }

    public void SetPrevState(State<entity_type> s)
    {
        m_PrevState = s;
    }

    public void Update() 
    {
        if(m_GlobalState != null)
        {
            m_GlobalState.Excute(m_Owner);
        }
        if(m_CurrentState != null)
        {
            m_CurrentState.Excute(m_Owner);
        }
    }

    public void ChangeState(State<entity_type> NewState)
    {
        if (NewState == null)
            return;

        m_PrevState = m_CurrentState;
        m_CurrentState.Exit(m_Owner);
        m_CurrentState = NewState;
        m_CurrentState.Enter(m_Owner);
    }

    public void RevertToPrevState()
    {
        ChangeState(m_PrevState);
    }

    State<entity_type> CurrentState()
    {
        return m_CurrentState;
    }

    State<entity_type> GlobatState()
    {
        return m_GlobalState;
    }

    State<entity_type> PrevState()
    {
        return m_PrevState;
    }
} 


class StandState : Singleton<StandState>, State<CharacterMove>
{
    public void Enter(CharacterMove t)
    {
    }

    public void Excute(CharacterMove t)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            t.animator.SetBool("IsCrouch", true);
            t.stateMachine.ChangeState(Crouching.GetInstance());
        }

        Vector3 _moveDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        float zForce = _moveDir.z;
        t.InputDir = _moveDir;
        _moveDir.Normalize();
        _moveDir = t.tr.TransformDirection(_moveDir);

        t.MoveDir = Vector3.Lerp(t.MoveDir, _moveDir, 4.5f * Time.deltaTime);

        if(_moveDir.magnitude > 0.0f)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                t.speed = Mathf.Lerp(t.speed, t.runSpeed, 3.0f * Time.deltaTime);
            }
            else
            {
                t.speed = Mathf.Lerp(t.speed, t.walkSpeed, 3.0f * Time.deltaTime);
            }
        }
        else
        {
            if(t.speed < 10.0f)
            {
                t.speed = 0.0f;
            }
            t.speed = Mathf.Lerp(t.speed, 0.0f, 3.0f * Time.deltaTime);
        }
    }

    public void Exit(CharacterMove t)
    {
    }
}

class Crouching : Singleton<Crouching>, State<CharacterMove>
{
    public void Enter(CharacterMove t)
    {
        t.isCrouchingEnd = false;
        t.animator.SetBool("CrouchingEnd", false);
        
    }

    public void Excute(CharacterMove t)
    {
        t.MoveDir = Vector3.zero;
        if (t.isCrouchingEnd)
        {
            t.stateMachine.ChangeState(CrouchState.GetInstance());
        }
    }

    public void Exit(CharacterMove t)
    {
        
    }
}

class CrouchState : Singleton<CrouchState>, State<CharacterMove>
{
    public void Enter(CharacterMove t)
    {
    }

    public void Excute(CharacterMove t)
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            t.animator.SetBool("IsCrouch", false);
            t.stateMachine.ChangeState(StandingState.GetInstance());
        }
    }

    public void Exit(CharacterMove t)
    {
    }
}

class StandingState : Singleton<StandingState>, State<CharacterMove>
{
    public void Enter(CharacterMove t)
    {
        t.isStandingEnd = false;
        t.animator.SetBool("StandingEnd", false);
    }
    public void Excute(CharacterMove t)
    {
        if(t.isStandingEnd)
        {
            t.stateMachine.ChangeState(StandState.GetInstance());
        }
    }
    public void Exit(CharacterMove t)
    {
        
    }
}


class CharacterGlobalState : Singleton<CharacterGlobalState>, State<CharacterMove>
{
    public void Enter(CharacterMove t)
    {
    }

    public void Excute(CharacterMove t)
    {
        Vector3 direction = t.MoveDir;
        direction *= t.speed * t.translateSpeed;
        t.WalkAnimatorSet(t.InputDir);
        if (!t.controller.isGrounded)
        {
            t.stackGravity += t.gravity * Time.deltaTime;
        }
        else
        {
            t.stackGravity = 0.0f;
        }
        direction.y -= t.stackGravity;
        t.controller.Move(direction * Time.deltaTime);
        t.RotateHorizontal();
    }

    public void Exit(CharacterMove t)
    {
    }
}