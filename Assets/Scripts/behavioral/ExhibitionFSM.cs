using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class ExhibitionFSM : StateMachine
{
    [HideInInspector] public AIRelatedData aiRelatedData;
    public event System.Action AnimationEndEvent;                                                   // invoked when currently playing animation ends
                                                                                                    // this is crucial for certain states to exit
    public event System.Action AnimationPhoneInEvent;
    public event System.Action AnimationPhoneOutEvent;

    [HideInInspector] public Idle idleState;
    [HideInInspector] public Wander wanderState;
    [HideInInspector] public IdleTexting idleTextingState;
    [HideInInspector] public IdlePhoneTalking idlePhoneTalking;
    [HideInInspector] public ObserveObject observeObject;

    [HideInInspector] public readonly string IS_WALKING             = "isWalking";
    [HideInInspector] public readonly string IS_TEXTING             = "isTexting";
    [HideInInspector] public readonly string IS_PHONE_TALKING       = "isPhoneTalking";
    [HideInInspector] public readonly string IS_OBSERVING_OBJECT    = "isObservingObject";
    [HideInInspector] public readonly string VELOCITY               = "velocity";
    [HideInInspector] public readonly string ANGULAR_VELOCITY       = "angularVelocity";

    // a list of states and the probability to transition to them
    public List<(State state, int probability)> stochastic_states;

    private void Awake()
    {
        Animator _anim = GetComponent<Animator>();
        NavMeshAgent _agent = GetComponent<NavMeshAgent>();
        NPCHelper _helper = GetComponent<NPCHelper>();

        idleState = new Idle(this, gameObject, _anim, _agent, _helper);
        wanderState = new Wander(this, gameObject, _anim, _agent, _helper);
        idleTextingState = new IdleTexting(this, gameObject, _anim, _agent, _helper);
        idlePhoneTalking = new IdlePhoneTalking(this, gameObject, _anim, _agent, _helper);
        observeObject = new ObserveObject(this, gameObject, _anim, _agent, _helper);

        // use integers instead of floats!
        stochastic_states = new List<(State state, int probability)>{
            (wanderState,           60),
            (idleTextingState,      13),
            (idlePhoneTalking,      7),
            (observeObject,         20),
        };

        stochastic_states.Sort(delegate ((State state, int probability) tuple1, (State state, int probability) tuple2)
        {
            return tuple1.probability.CompareTo(tuple2.probability);
        });

        long prob_sum = stochastic_states.Sum(x => x.probability);
        if (prob_sum != 100)
        {
            Debug.LogWarning("sum of probabilities is NOT 1!");
            throw new System.Exception();
        }

    }

    public override State DetermineNextState(bool isPreviousStateRepeatable = true)
    {
        // it turns out that for some reason even states with  probability can still be picked!
        // that's because randomly generated number started from 0!
        State NextStateUsingRandomness(List<(State state, int probability)> stochastic_list_parameter)
        {
            int current_range = 0;
            int rand_value = Random.Range(1, 101);

            for (int i = 0; i < stochastic_list_parameter.Count; i++)
            {
                var state_prob = stochastic_list_parameter[i];

                current_range += state_prob.probability;
                if ((rand_value <= current_range))
                {
                    return state_prob.state;
                }
            }

            throw new System.Exception();
        }

        if (!isPreviousStateRepeatable)
        {
            // find current state in stochastic_states array
            int currentStateIndex = stochastic_states.FindIndex(((State, int) obj) => obj.Item1 == currentState);

            // check if current state has a 100% probabiltiy of accuring, if so then raise an exception (and log a warning)
            if (stochastic_states[currentStateIndex].probability >= 100)
            {
                throw new System.ArgumentException("You have specified that previous state should NOT be repeated, yet ut has a 100% probability of occurence");
            }

            // create a new stochastic_states list and remove the current state
            List<(State state, int probability)> stochastic_states_copy = new List<(State state, int probability)>(stochastic_states);
            stochastic_states_copy.RemoveAt(currentStateIndex);

            // uniformly distribute the probability of the current state to the other states
            for (int remainingProbability = stochastic_states[currentStateIndex].probability, index = 0; remainingProbability > 0; --index)
            {
                if (index < 0) index = stochastic_states_copy.Count - 1;

                var (state, probability) = stochastic_states_copy[index];
                if (probability > 0)
                {
                    stochastic_states_copy[index] = (state, probability + 1);
                    --remainingProbability;
                }
            }

            return NextStateUsingRandomness(stochastic_states_copy);
        }

        return NextStateUsingRandomness(stochastic_states);
        
    }

    /// <summary>
    ///     Unity animation events should call this function to inform the current state that the currently
    ///     playing animation has ended.
    /// </summary>
    public void OnAnimationEnd() => AnimationEndEvent?.Invoke();

    /// <summary>
    ///     Unity animation events should call this function to inform the current state about the moment
    ///     to destroy the previously instantiated phone.
    /// </summary>
    public void OnPhoneIn() => AnimationPhoneInEvent?.Invoke();

    /// <summary>
    ///     Unity animation events should call this function to in form the current state about the moment
    ///     to destroy the previously instantiated phone.
    /// </summary>
    public void OnPhoneOut() => AnimationPhoneOutEvent?.Invoke();

    protected override State GetInitialState() => wanderState;
}
