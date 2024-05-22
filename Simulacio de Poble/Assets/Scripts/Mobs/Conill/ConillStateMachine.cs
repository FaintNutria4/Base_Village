using System.Collections;
using System.Collections.Generic;
using KevinCastejon.FiniteStateMachine;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class ConillStateMachine : AbstractFiniteStateMachine
{

    public RabitController controller;
    public float idleWait = 1.5f;
    public float pratrolLenght = 5f;
    public float howFarFromSpawn = 50f;
    public enum ConillState
    {
        IDLE,
        PATROL,
        FLEE
    }

    private void Awake()
    {
        controller = GetComponent<RabitController>();
        Init(ConillState.IDLE,
            AbstractState.Create<IdleState, ConillState>(ConillState.IDLE, this),
            AbstractState.Create<PatrolState, ConillState>(ConillState.PATROL, this),
            AbstractState.Create<FleeState, ConillState>(ConillState.FLEE, this)
        );
    }
    public class IdleState : AbstractState
    {
        public override void OnEnter()
        {
            IEnumerator coroutine = WaitTime(GetStateMachine<ConillStateMachine>().idleWait);
            GetStateMachine<ConillStateMachine>().StartCoroutine(coroutine);
        }
        public override void OnUpdate()
        {
            if (GetStateMachine<ConillStateMachine>().controller.villagers.Count > 0)
            {
                TransitionToState(ConillState.FLEE);
            }

        }
        public override void OnExit()
        {

        }

        IEnumerator WaitTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (GetStateMachine<ConillStateMachine>().GetCurrentStateEnumValue<ConillState>() == ConillState.IDLE) TransitionToState(ConillState.PATROL);
        }
    }
    public class PatrolState : AbstractState
    {
        public Vector3 target;
        ConillStateMachine sm;
        public override void OnEnter()
        {
            sm = GetStateMachine<ConillStateMachine>();
            Vector3 origin = sm.transform.position;
            Vector3 spawnPos = sm.controller.spawnPos;

            target = origin + Random.insideUnitSphere.normalized * sm.pratrolLenght;
            target.y = 0;



            if (Vector3.Distance(spawnPos, target) > sm.howFarFromSpawn)
            {
                target = spawnPos + (target - spawnPos).normalized * sm.howFarFromSpawn;
            }

            sm.controller.navMeshAgent.SetDestination(target);

        }
        public override void OnUpdate()
        {
            if (sm.controller.navMeshAgent.remainingDistance == 0)
            {

                TransitionToState(ConillState.IDLE);
            }
            if (GetStateMachine<ConillStateMachine>().controller.villagers.Count > 0)
            {
                TransitionToState(ConillState.FLEE);
            }
        }
        public override void OnExit()
        {

        }
    }

    public class FleeState : AbstractState
    {
        ConillStateMachine sm;
        List<GameObject> villagers;
        public override void OnEnter()
        {
            sm = GetStateMachine<ConillStateMachine>();
            villagers = sm.controller.villagers;

        }
        public override void OnUpdate()
        {
            if (villagers.Count == 0)
            {
                TransitionToState(ConillState.IDLE);
            }
            else
            {
                Vector3 villagerPos = villagers[0].transform.position;
                Vector3 target = (sm.transform.position - villagerPos).normalized * sm.pratrolLenght + sm.transform.position;
                target.y = 0;
                sm.controller.navMeshAgent.SetDestination(target);
            }

        }
        public override void OnExit()
        {
            sm.controller.navMeshAgent.ResetPath();
        }
    }

}
