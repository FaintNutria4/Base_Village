using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KevinCastejon.FiniteStateMachine;
using UnityEngine;
using UnityEngine.AI;

public class GoblinStateMachine : AbstractFiniteStateMachine
{
    public GoblinController controller;
    public float idleWait = 0.5f;
    public float pratrolLenght = 5f;
    public float howFarFromSpawn = 30f;

    public enum GoblinState
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }
    private void Awake()
    {
        controller = GetComponent<GoblinController>();
        Init(GoblinState.IDLE,
            AbstractState.Create<IdleState, GoblinState>(GoblinState.IDLE, this),
            AbstractState.Create<PatrolState, GoblinState>(GoblinState.PATROL, this),
            AbstractState.Create<ChaseState, GoblinState>(GoblinState.CHASE, this),
            AbstractState.Create<AttackState, GoblinState>(GoblinState.ATTACK, this)
        );
    }

    public class IdleState : AbstractState
    {
        public override void OnEnter()
        {
            Debug.Log("Idle");
            IEnumerator coroutine = WaitTime(GetStateMachine<GoblinStateMachine>().idleWait);
            GetStateMachine<GoblinStateMachine>().StartCoroutine(coroutine);
        }
        public override void OnUpdate()
        {
            if (GetStateMachine<GoblinStateMachine>().controller.villagers.Count > 0)
            {
                TransitionToState(GoblinState.CHASE);
            }

        }
        public override void OnExit()
        {

        }

        IEnumerator WaitTime(float time)
        {
            yield return new WaitForSeconds(time);
            if (GetStateMachine<GoblinStateMachine>().GetCurrentStateEnumValue<GoblinState>() == GoblinState.IDLE) TransitionToState(GoblinState.PATROL);
        }
    }

    public class PatrolState : AbstractState
    {
        public Vector3 target;
        GoblinStateMachine sm;
        public override void OnEnter()
        {
            Debug.Log("Patrol");
            sm = GetStateMachine<GoblinStateMachine>();
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

                TransitionToState(GoblinState.IDLE);
            }
            if (GetStateMachine<GoblinStateMachine>().controller.villagers.Count > 0)
            {
                TransitionToState(GoblinState.CHASE);
            }
        }
        public override void OnExit()
        {

        }
    }

    public class ChaseState : AbstractState
    {
        GoblinStateMachine sm;
        List<GameObject> villagers;
        NavMeshAgent navMeshAgent;

        float attackDistance;
        public override void OnEnter()
        {
            Debug.Log("Chase");
            sm = GetStateMachine<GoblinStateMachine>();
            navMeshAgent = sm.controller.navMeshAgent;
            villagers = sm.controller.villagers;
            attackDistance = sm.controller.goblinAttack.attackDistance;




        }
        public override void OnUpdate()
        {

            villagers.RemoveAll(x => x == null);

            if (villagers.Count == 0)
            {
                TransitionToState(GoblinState.PATROL);
            }
            else
            {
                Vector3 target = villagers[0].transform.position;
                target.y = 0;
                navMeshAgent.SetDestination(target);

                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(sm.transform.position, villagers[0].transform.position, NavMesh.AllAreas, path);

                if (path.status != NavMeshPathStatus.PathComplete)
                {
                    villagers.Remove(villagers[0]);

                }
                else
                {
                    float distance = Vector3.Distance(villagers[0].transform.position, sm.transform.position);
                    if (distance < attackDistance) TransitionToState(GoblinState.ATTACK);
                }

            }

        }



        public override void OnExit()
        {
            navMeshAgent.ResetPath();
        }
    }

    public class AttackState : AbstractState
    {
        List<GameObject> villagers;
        GoblinStateMachine sm;
        GoblinController controller;
        public override void OnEnter()
        {
            Debug.Log("attack");
            sm = GetStateMachine<GoblinStateMachine>();
            controller = sm.controller;
            villagers = controller.villagers;

        }
        public override void OnUpdate()
        {
            villagers.RemoveAll(x => x == null);
            if (villagers.Count == 0) TransitionToState(GoblinState.PATROL);
            else
            {
                float distance = Vector3.Distance(villagers[0].transform.position, sm.transform.position);
                if (distance > controller.goblinAttack.attackDistance) TransitionToState(GoblinState.CHASE);

                controller.LookAt(villagers[0].transform.position);
                if (controller.goblinAttack.Attack(controller))
                { // If true goblin killed
                    villagers.Remove(villagers[0]);
                }
            }


        }
        public override void OnExit()
        {

        }
    }
}
