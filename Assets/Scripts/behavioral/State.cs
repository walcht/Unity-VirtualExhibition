using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    // name of the state is automatically generated from the name of its class
    public string name;
    protected StateMachine stateMachine;

    protected GameObject    npc;
    protected Animator      npcAnimator;
    protected NavMeshAgent  npcAgent;
    protected NPCHelper     npcHelper;

    public State(StateMachine _stateMachine, GameObject _npc, Animator _npcAnimator, NavMeshAgent _npcAgent,
        NPCHelper _helper)
    {
        stateMachine = _stateMachine;
        npc = _npc;
        npcAnimator = _npcAnimator;
        npcAgent = _npcAgent;
        npcHelper = _helper;

        // verify this!
        name = GetType().Name;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void Exit() { }
}