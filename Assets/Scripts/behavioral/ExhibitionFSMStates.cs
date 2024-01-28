using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     This state implements the just-standing-and-doing-absolutely-nothing behaviour
/// </summary>
public class Idle : State
{
    protected ExhibitionFSM exhibitionFSM;
    public Idle(ExhibitionFSM _exhibitionFSM, GameObject _npc, Animator _npcAnimator, NavMeshAgent _npcAgent, NPCHelper _helper) :
        base(_exhibitionFSM, _npc, _npcAnimator, _npcAgent, _helper)
    {
        exhibitionFSM = _exhibitionFSM;
    }

}

/// <summary>
///     This state implements the walking-around-without-a-particular-goal behaviour.
///     Agent randomly chooses a set of waypoints (without replacement) and goes to each one of them.
///     When the last waypoint is reached, the FSM is informed to determine the next state for the agent.
/// </summary>
public class Wander : State
{
    ExhibitionFSM exhibitionFSM;

    int nbr_waypoints;                                                                                                  // number of waypoints to check
    List<Vector3> chosen_waypoints = new List<Vector3>();                                                               // which waypoints to check

    int currentWaypointIndex = 0;                                                                                       // to keep track of the index of the current target waypoint
    public Wander(ExhibitionFSM _exhibitionFSM, GameObject _npc, Animator _npcAnimator, NavMeshAgent _npcAgent,
        NPCHelper _helper)
        : base(_exhibitionFSM, _npc, _npcAnimator, _npcAgent, _helper)
    {
        exhibitionFSM = _exhibitionFSM;
    }

    public override void Enter()
    {
        // clear chosen-waypoints list
        chosen_waypoints.Clear();

        // reset current waypoint index
        currentWaypointIndex = 0;

        // create a copy of waypoints list (yeah there is probably a much more efficient way to do this, but it really doesn't matter that much)
        List<Vector3> choose_from_waypoints = new List<Vector3>(exhibitionFSM.aiRelatedData.spawnPositions);

        // randomly pick the number of waypoints to go to ([|2, min(8, total_nbr_waypoints)|]).
        nbr_waypoints = Random.Range(2, Mathf.Min(2, exhibitionFSM.aiRelatedData.spawnPositions.Count));

        // randomly choose which waypoints to go to.
        for (int i = 0; i < nbr_waypoints; i++)
        {
            int randomly_chosen_waypoint_index = Random.Range(0, choose_from_waypoints.Count - 1);                      // randomly chose an index
            chosen_waypoints.Add(choose_from_waypoints[randomly_chosen_waypoint_index]);

            choose_from_waypoints.RemoveAt(randomly_chosen_waypoint_index);                                             // remove the previously chosen element
        }

        // set destination to the first waypoint
        npcAgent.SetDestination(chosen_waypoints[currentWaypointIndex]);

        // Animation
        npcAnimator.SetBool(exhibitionFSM.IS_WALKING, true);                                                            // activate locomotion animation (blend tree)
    }

    public override void Update()
    {
        if ( !npcAgent.pathPending && npcAgent.remainingDistance < 2.0f)
        {
            if (++currentWaypointIndex >= nbr_waypoints) exhibitionFSM.NotifyActionCompleted();
            else npcAgent.SetDestination(chosen_waypoints[currentWaypointIndex]);
        }

        npcAnimator.SetFloat(exhibitionFSM.VELOCITY, npcAgent.velocity.magnitude, 0.2f, Time.deltaTime);
        npcAnimator.SetFloat(exhibitionFSM.ANGULAR_VELOCITY, npcHelper.angularVelocityY, 0.2f, Time.deltaTime);
    }

    public override void Exit()
    {
        npcAgent.ResetPath();
        npcAnimator.SetBool(exhibitionFSM.IS_WALKING, false);                                                           // disable walking animation
    }
}

/// <summary>
///     This state implements the very realistic, I-am-pretending-to-be-on-phone behaviour.
/// </summary>
public class IdleTexting : State
{
    protected ExhibitionFSM exhibitionFSM;
    public IdleTexting(ExhibitionFSM _exhibitionFSM, GameObject _npc, Animator _npcAnimator, NavMeshAgent _npcAgent, NPCHelper _helper) : 
        base(_exhibitionFSM, _npc, _npcAnimator, _npcAgent, _helper)
    {
        exhibitionFSM = _exhibitionFSM;
    }
    public override void Enter()
    {
        npcAnimator.SetBool(exhibitionFSM.IS_TEXTING, true);   
        exhibitionFSM.AnimationEndEvent += OnEndAnimation;

        exhibitionFSM.AnimationPhoneOutEvent += OnPhoneOut;
        exhibitionFSM.AnimationPhoneInEvent += OnPhoneIn;
    }

    public override void Exit()
    {
        exhibitionFSM.AnimationEndEvent -= OnEndAnimation;

        exhibitionFSM.AnimationPhoneOutEvent -= OnPhoneOut;
        exhibitionFSM.AnimationPhoneInEvent -= OnPhoneIn;
        npcAnimator.SetBool(exhibitionFSM.IS_TEXTING, false);
    }

    void OnEndAnimation()
    {
        // inform the FSM that this state has finished execution and that it shouldn't execute it again.
        exhibitionFSM.NotifyActionCompleted(isPreviousStateRepeatable: false);
    }

        // call a helper function to destroy the previously instantiated phone object
    void OnPhoneOut() => npcHelper.InstantiatePhone();

    // call a helper function to instantiate a phone object in npc's hands
    void OnPhoneIn() => npcHelper.DestroyPhone();
}

/// <summary>
///     This state implements the extremely realitic, look-I-am-so-busy-talking-to-no-one-on-the-phone behaviour.
/// </summary>
public class IdlePhoneTalking : State
{
    protected ExhibitionFSM exhibitionFSM;
    public IdlePhoneTalking(ExhibitionFSM _exhibitionFSM, GameObject _npc, Animator _npcAnimator, NavMeshAgent _npcAgent, NPCHelper _helper) :
        base(_exhibitionFSM, _npc, _npcAnimator, _npcAgent, _helper)
    {
        exhibitionFSM = _exhibitionFSM;
    }
    public override void Enter()
    {
        npcAnimator.SetBool(exhibitionFSM.IS_PHONE_TALKING, true);
        exhibitionFSM.AnimationEndEvent += OnAnimationEnd;

        exhibitionFSM.AnimationPhoneOutEvent += OnPhoneOut;
        exhibitionFSM.AnimationPhoneInEvent += OnPhoneIn;
    }

    public override void Exit()
    {
        exhibitionFSM.AnimationEndEvent -= OnAnimationEnd;

        exhibitionFSM.AnimationPhoneOutEvent -= OnPhoneOut;
        exhibitionFSM.AnimationPhoneInEvent -= OnPhoneIn;
        npcAnimator.SetBool(exhibitionFSM.IS_PHONE_TALKING, false);
    }

    void OnAnimationEnd()
    {
        // inform the FSM that this state has finished execution and that it shouldn't execute it again.
        exhibitionFSM.NotifyActionCompleted(isPreviousStateRepeatable: false);
    }

    // call a helper function to destroy the previously instantiated phone object
    void OnPhoneOut() => npcHelper.InstantiatePhone();

    // call a helper function to instantiate a phone object in npc's hands
    void OnPhoneIn() => npcHelper.DestroyPhone();
}

/// <summary>
///     NPC agent decides to go to a spot to observe an object. The object can be anything,
///     most importantly a stand or an object showcasing a sponsor.
///     The set of steps the agent must make are:
///     - reach observable waypoint
///     - rotate towards the waypoints target (NO NEED FOR THIS IF THE WAYPOINT IS THE TARGET ITSELF!)
///     - play "observeTarget" animation
/// </summary>
public class ObserveObject : State
{                                                                                                                                                                     // that limits how close an npc can geet to an observabl

    protected ExhibitionFSM exhibitionFSM;

    int observableObjectIndex;
    bool waypointReached = false;

    int previousIndex = -1;

    public ObserveObject(ExhibitionFSM _exhibitionFSM, GameObject _npc, Animator _npcAnimator, 
        NavMeshAgent _npcAgent, NPCHelper _helper)
        : base(_exhibitionFSM, _npc, _npcAnimator, _npcAgent, _helper)
    {
        exhibitionFSM = _exhibitionFSM;
    }
    public override void Enter()
    {
        // randomly pick an oservable waypoint and set it as destination
        if (previousIndex < 0)
            observableObjectIndex = Random.Range(0, exhibitionFSM.aiRelatedData.observable_waypoints.Count);
        else
        {
            observableObjectIndex = Random.Range(0, exhibitionFSM.aiRelatedData.observable_waypoints.Count - 1);
            if (observableObjectIndex == previousIndex) observableObjectIndex = exhibitionFSM.aiRelatedData.observable_waypoints.Count - 1;
        }    

        npcAgent.SetDestination(exhibitionFSM.aiRelatedData.observable_waypoints[observableObjectIndex].position);
        npcAnimator.SetBool(exhibitionFSM.IS_WALKING, true);                                                            // activate walking animation
        npcAgent.stoppingDistance = 0;                                                                                  // important to avoid edge case where stopping distance
                                                                                                                        // is larger than observing distance.
                                                                                                                        // It should also be noted that observing distance SHOULD
                                                                                                                        // the navmesh, otherwise agent will get stuck!
        previousIndex = observableObjectIndex;                                                                                                         
    }
    public override void Update()
    {
        if (    !waypointReached && !npcAgent.pathPending && npcAgent.remainingDistance <= 
                exhibitionFSM.aiRelatedData.observable_waypoints[observableObjectIndex].observingDistance   )
        {
            waypointReached = true;                                                                                     // waypoint is reached!
            npcAgent.ResetPath();
            npcAnimator.SetBool(exhibitionFSM.IS_WALKING, false);                                                       // disable walking animation
            npcAnimator.SetBool(exhibitionFSM.IS_OBSERVING_OBJECT, true);                                               // enable observing object animation

            exhibitionFSM.AnimationEndEvent += OnAnimationEnd;                                                          // subscribe to animation-end event to exit this state
                                                                                                                        // when animation ends
        }

        npcAnimator.SetFloat(exhibitionFSM.VELOCITY, npcAgent.velocity.magnitude, 0.1f, Time.deltaTime);
        npcAnimator.SetFloat(exhibitionFSM.ANGULAR_VELOCITY, npcHelper.angularVelocityY, 0.3f, Time.deltaTime);
    }
    public override void Exit()
    {
        waypointReached = false;
        exhibitionFSM.AnimationEndEvent -= OnAnimationEnd;
        npcAnimator.SetBool(exhibitionFSM.IS_OBSERVING_OBJECT, false);
    }

    void OnAnimationEnd()
    {
        exhibitionFSM.NotifyActionCompleted();
    }
}

