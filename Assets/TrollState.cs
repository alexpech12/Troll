using UnityEngine;
using System.Collections.Generic;

public class TrollState : MonoBehaviour {
    
    public enum State
    {
        DORMANT,
        UNAWARE,
        INVESTIGATE,
        AWARE,
        LOST_SIGHT,
        SEARCHING
    }

    Dictionary<State, AwarenessState> stateTable = new Dictionary<State, AwarenessState>();

    public static int DORMANT = (int)State.DORMANT;
    public static int UNAWARE = (int)State.UNAWARE;
    public static int INVESTIGATE = (int)State.INVESTIGATE;
    public static int AWARE = (int)State.AWARE;
    public static int LOST_SIGHT = (int)State.LOST_SIGHT;
    public static int SEARCHING = (int)State.SEARCHING;

    State m_state = State.UNAWARE;
    State m_prev_state = State.UNAWARE;
    bool m_changed;

    void Start()
    {
        // Build state table
        AwarenessState[] awarenessStates = GetComponents<AwarenessState>();
        foreach(AwarenessState state in awarenessStates)
        {
            stateTable.Add(state.type, state);
        }
    }

    void Update()
    {
        m_changed = m_state != m_prev_state;
        m_prev_state = m_state;
    }

    public int ToInt()
    {
        return (int)m_state;
    }

    public void SetState(AwarenessState newState)
    {
        if(stateTable.ContainsValue(newState))
        {
            SetState(newState.type);
        } else
        {
            Debug.LogError("state " + newState + " is not part of the TrollState state table!");
        }
    }

    public void SetState(State newState)
    {
        Debug.Log("Setting state to " + newState);
        m_state = newState;
    }

    public State GetState()
    {
        return m_state;
    }

    public AwarenessState GetStateComponent()
    {
        return stateTable[m_state];
    }

    public State LastState()
    {
        return m_prev_state;
    }

    public bool Changed()
    {
        return m_changed;
    }
}
