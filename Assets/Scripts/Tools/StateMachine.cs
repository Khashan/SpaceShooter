using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnterState();
public delegate void UpdateState();
public delegate void ExitState();

public class StateMachine
{
    private State[] m_AllStates;
    private State m_PreviousState;
    private State m_CurrentState;
    private State m_IncomingState;

    private int m_StateMachineSize;
    private bool m_IsLocked;

    public StateMachine(int aSizeState)
    {
        m_AllStates = new State[aSizeState];
        m_StateMachineSize = aSizeState;
    }

    public void AddState(int aStateId, EnterState aEnter, UpdateState aUpdate, ExitState aExit)
    {
        m_AllStates[aStateId] = new State(aStateId, aEnter, aUpdate, aExit);
    }

    public void ChangeState(int aStateId)
    {
        if (!m_IsLocked && aStateId < m_StateMachineSize)
        {
            if (m_CurrentState != m_AllStates[aStateId])
            {
                m_IncomingState = m_AllStates[aStateId];

                if (m_CurrentState != null)
                {
                    m_CurrentState.Exit();
                }

                m_PreviousState = m_CurrentState;
                m_CurrentState = m_AllStates[aStateId];
                m_CurrentState.Enter();
                m_IncomingState = null;
            }
        }
    }

    public void ChangeStateWithoutExitSignal(int aStateId)
    {
        if (!m_IsLocked && aStateId < m_StateMachineSize)
        {
            if (m_CurrentState != m_AllStates[aStateId])
            {
                m_PreviousState = m_CurrentState;
                m_CurrentState = m_AllStates[aStateId];
                m_CurrentState.Enter();
            }
        }
    }

    public void ReturnToPreviousState()
    {
        if (!m_IsLocked && m_PreviousState != null)
        {
            ChangeState(m_PreviousState.GetStateId());
        }
    }

    public int GetCurrentState()
    {
        if (m_CurrentState == null)
        {
            return -1;
        }
        else
        {
            return m_CurrentState.GetStateId();
        }
    }

    public int GetPreviousState()
    {
        if (m_PreviousState == null)
        {
            return -1;
        }
        else
        {
            return m_PreviousState.GetStateId();
        }
    }

    public int GetIncomingState()
    {
        if (m_IncomingState == null)
        {
            return -1;
        }
        else
        {
            return m_IncomingState.GetStateId();
        }
    }

    public void Enter()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.Enter();
        }
    }

    public void Update()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.Update();
        }
    }

    public void Exit()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.Exit();
        }
    }

    public bool Locked
    {
        get
        {
            return m_IsLocked;
        }
        set
        {
            m_IsLocked = value;
        }
    }
}