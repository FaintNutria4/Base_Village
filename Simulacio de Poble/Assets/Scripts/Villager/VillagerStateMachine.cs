using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KevinCastejon.FiniteStateMachine;
using System.Linq;
using UnityEditor;
using System.Collections.Concurrent;


public class VillagerStateMachine : AbstractFiniteStateMachine
{

    public AgentController controller;
    public Agent_System_Manager manager;
    MobsStateManager mobsState;
    public float idleWait = 0.5f;
    public float hungerBelowX = 25f; //max acceptable hunger value 
    public float sleepinessBelowX = 90f; //max acceptable sleepiness value
    public float maxWoodsDistance = 20f; //max distance from woods position to hunt in
    public float dangerDetectionDistance = 15f;
    public float preyDetectionDistance = 10f;


    public enum VillagerState
    {
        IDLE,
        CULTIVATE,
        HARVEST,
        EAT,
        HUNTWOODS,
        HUNTRABIT,
        ATTACKRABIT,
        CHASEGOBLIN,
        ATTACKGOBLIN,
        GOHOME,
        SLEEP,
        GETOUTHOME

    }
    private void Awake()
    {
        controller = GetComponent<AgentController>();
        manager = GetComponent<Agent_System_Manager>();
        mobsState = MobsStateManager.GetInstance();

        Init(VillagerState.IDLE,
            AbstractState.Create<IdleState, VillagerState>(VillagerState.IDLE, this),
            AbstractState.Create<CultivateState, VillagerState>(VillagerState.CULTIVATE, this),
            AbstractState.Create<HarveshState, VillagerState>(VillagerState.HARVEST, this),
            AbstractState.Create<EatState, VillagerState>(VillagerState.EAT, this),
            AbstractState.Create<HuntWoodsState, VillagerState>(VillagerState.HUNTWOODS, this),
            AbstractState.Create<HuntRabitState, VillagerState>(VillagerState.HUNTRABIT, this),
            AbstractState.Create<AttackRabitState, VillagerState>(VillagerState.ATTACKRABIT, this),
            AbstractState.Create<ChaseGoblinState, VillagerState>(VillagerState.CHASEGOBLIN, this),
            AbstractState.Create<AttackGoblinState, VillagerState>(VillagerState.ATTACKGOBLIN, this),
            AbstractState.Create<GoHomeState, VillagerState>(VillagerState.GOHOME, this),
            AbstractState.Create<SleepState, VillagerState>(VillagerState.SLEEP, this),
            AbstractState.Create<GetOutHomeState, VillagerState>(VillagerState.GETOUTHOME, this)

        );
    }

    public void PrintEmptyFields()
    {

        Debug.Log("SM Number of empty fields: " + FieldsStateManager.GetInstance().GetEmptyFields().Count);


    }
    public bool CheckEat()
    {
        if (manager.HealthManager.hunger >= hungerBelowX && manager.HasFood()) return true;
        else return false;
    }
    public bool CheckSleep()
    {
        if (manager.HealthManager.sleepiness >= sleepinessBelowX && manager.HealthManager.hunger < hungerBelowX) return true;
        else return false;
    }

    public bool CheckCultivate()
    {
        if (!manager.CanHarvest()) return false;
        else if (manager.GetFields().GetEmptyFields().Count > 0) return true;
        else return false;
    }
    public bool CheckHarvest()
    {
        if (!manager.CanHarvest()) return false;
        else if (manager.GetFields().GetFullFields().Count > 0) return true;
        return false;
    }
    public bool CheckHuntWoods()
    {
        return manager.CanHunt();
    }
    public Transform GetClosestTransform(List<Transform> mobs)
    {
        Transform closestMob = mobs[0];
        float distance = Vector3.Distance(closestMob.position, transform.position);
        foreach (Transform mob in mobs)
        {
            float aux = Vector3.Distance(mob.position, transform.position);
            if (distance > aux)
            {
                distance = aux;
                closestMob = mob;
            }
        }
        return closestMob;
    }

    bool reserve = true;
    public Transform ReserveClosestEmptyField()
    {
        Dictionary<Transform, bool> map = FieldsStateManager.GetInstance().GetEmptyFields();
        Debug.Log("Reserve Empty: " + map.Count);
        var fields = map.Where(pair => pair.Value == false).Select(pair => pair.Key);
        if (fields.Count() == 0) return null;
        else
        {
            Transform closestField = fields.First();
            float distance = Vector3.Distance(closestField.transform.position, transform.position);
            foreach (Transform field in fields)
            {
                float aux = Vector3.Distance(field.position, transform.position);
                if (distance > aux)
                {
                    distance = aux;
                    closestField = field;
                }
            }
            Debug.Log("Reserve Empty2: " + map.Count);
            if (reserve && FieldsStateManager.GetInstance().GetEmptyFields().ContainsKey(closestField))
            {
                Debug.Log("Reserve Empty3: " + map.Count);
                FieldsStateManager.GetInstance().GetEmptyFields()[closestField] = true;
            }



            return closestField;
        }
    }



    public void CancelReserveEmptyField(Transform field)
    {
        manager.GetFields().GetEmptyFields()[field] = false;
    }
    public Transform ReserveClosestGrownedField()
    {

        var fields = manager.GetFields().GetFullFields().Where(pair => pair.Value == false).Select(pair => pair.Key);
        if (fields.Count() == 0) return null;
        else
        {
            Transform closestField = fields.First();
            float distance = Vector3.Distance(closestField.transform.position, transform.position);
            foreach (Transform field in fields)
            {
                float aux = Vector3.Distance(field.position, transform.position);
                if (distance > aux)
                {
                    distance = aux;
                    closestField = field;
                }
            }
            manager.GetFields().GetFullFields()[closestField] = true;
            Debug.Log("Reserve Full Field: " + closestField);
            return closestField;
        }
    }

    public void CancelReserveGrownedField(Transform field)
    {
        manager.GetFields().GetFullFields()[field] = false;

    }
    public void ReserveClosestHouse()
    {
        HousesStateManager housesState = HousesStateManager.GetInstance();
        Dictionary<House, bool> housesDic = housesState.GetHousesDictionary();
        var houses = housesDic.Where(pair => pair.Value == false).Select(pair => pair.Key);
        if (houses.Count() == 0) manager.house = null;
        else
        {
            House closestHouse = houses.First();
            float distance = Vector3.Distance(closestHouse.transform.position, transform.position);
            foreach (House house in houses)
            {
                float aux = Vector3.Distance(house.transform.position, transform.position);
                if (distance > aux)
                {
                    distance = aux;
                    closestHouse = house;
                }
            }
            manager.house = closestHouse;
            housesDic[closestHouse] = true;


        }
    }
    public void RemoveHouseReserve()
    {
        Dictionary<House, bool> housesDic = HousesStateManager.GetInstance().GetHousesDictionary();
        housesDic[manager.house] = false;
        manager.house = null;
    }
    public bool IsPreyNear()
    {
        List<Transform> rabits = mobsState.GetRabitsList();
        if (rabits.Count == 0) return false;
        else
        {
            Transform closestPrey = GetClosestTransform(rabits);
            float distance = Vector3.Distance(closestPrey.position, transform.position);
            return distance <= preyDetectionDistance;
        }
    }
    public bool IsInDanger()
    {
        List<Transform> goblins = mobsState.GetGoblinList();
        if (goblins.Count == 0) return false;
        else
        {
            Transform closestGoblin = GetClosestTransform(goblins);
            float distance = Vector3.Distance(closestGoblin.position, transform.position);
            return distance <= dangerDetectionDistance;
        }
    }
    public bool CloseEnoughtToInteract(Transform other)
    {
        float distance = Vector3.Distance(other.position, transform.position);
        float interactDistance = manager.getCurrentInteractDistance();
        return distance <= interactDistance;
    }

    public class IdleState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        public override void OnEnter()
        {
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {
            if (sm.IsInDanger() && !sm.manager.isHome()) // In danger and not in home
            {
                if (sm.manager.HasWeapons()) TransitionToState(VillagerState.CHASEGOBLIN);
                else TransitionToState(VillagerState.GOHOME);
            }
            else
            {
                if (sm.IsInDanger()) // In danger and in home
                {
                    if (sm.manager.HasWeapons()) TransitionToState(VillagerState.GETOUTHOME);
                    else
                    {
                        if (sm.CheckEat()) TransitionToState(VillagerState.EAT);
                        else if (sm.CheckSleep()) TransitionToState(VillagerState.SLEEP);
                    }
                }
                else // Not in danger 
                {
                    if (sm.manager.isHome()) // Not in danger and not in home
                    {
                        if (sm.CheckEat()) TransitionToState(VillagerState.EAT);
                        else if (sm.CheckSleep()) TransitionToState(VillagerState.SLEEP);
                        else if (sm.CheckHarvest()) TransitionToState(VillagerState.GETOUTHOME);
                        else if (sm.CheckCultivate()) TransitionToState(VillagerState.GETOUTHOME);
                        else if (sm.CheckHuntWoods()) TransitionToState(VillagerState.GETOUTHOME);
                    }
                    else // Not in danger and not home
                    {
                        if (sm.CheckEat()) TransitionToState(VillagerState.EAT);
                        else if (sm.CheckSleep()) TransitionToState(VillagerState.GOHOME);
                        else if (sm.CheckHarvest()) TransitionToState(VillagerState.HARVEST);
                        else if (sm.CheckCultivate()) TransitionToState(VillagerState.CULTIVATE);
                        else if (sm.CheckHuntWoods()) TransitionToState(VillagerState.HUNTWOODS);
                    }
                }

            }



        }


    }

    public class CultivateState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Transform field;
        public override void OnEnter()
        {
            Debug.Log(this.Name);
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            controller.RotateItem(resouces.GetAixida());
            field = sm.ReserveClosestEmptyField();
            if (field == null) TransitionToState(VillagerState.IDLE);
            else
            {
                controller.Move(field.position);
                sm.PrintEmptyFields();
            }


        }
        public override void OnUpdate()
        {
            if (sm.IsInDanger())
            {
                controller.StopMoving();
                sm.CancelReserveEmptyField(field);
                TransitionToState(VillagerState.IDLE);
            }
            else
            {
                float distance = Vector3.Distance(field.position, sm.transform.position);
                if (distance < controller.manager.getCurrentInteractDistance())
                {
                    controller.StopMoving();
                    controller.LookAt(field.position);
                    controller.Interact();
                    TransitionToState(VillagerState.IDLE);
                }
            }
        }
        public override void OnExit()
        {

        }

    }

    public class HarveshState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Transform field;
        public override void OnEnter()
        {
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            controller.RotateItem(resouces.GetAixida());
            field = sm.ReserveClosestGrownedField();
            if (field == null) TransitionToState(VillagerState.IDLE);
            else controller.Move(field.position);
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {
            if (sm.IsInDanger())
            {
                controller.StopMoving();
                TransitionToState(VillagerState.IDLE);
                sm.CancelReserveGrownedField(field);
            }
            else
            {
                float distance = Vector3.Distance(field.position, sm.transform.position);
                if (distance < controller.manager.getCurrentInteractDistance())
                {
                    controller.StopMoving();
                    controller.LookAt(field.position);
                    controller.Interact();
                    TransitionToState(VillagerState.IDLE);
                }
            }
        }
        public override void OnExit()
        {

        }


    }
    public class EatState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Agent_System_Manager manager;


        public override void OnEnter()
        {

            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            manager = sm.manager;
            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            if (!controller.RotateItem(resouces.GetCarn()))
            {
                if (!controller.RotateItem(resouces.GetBlat()))
                {
                    TransitionToState(VillagerState.IDLE);
                }
            }
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {
            controller.Interact();
            TransitionToState(VillagerState.IDLE);

        }

    }
    public class HuntWoodsState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Agent_System_Manager manager;
        Vector3 woodsPos;
        Vector3 target;


        public override void OnEnter()
        {

            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            manager = sm.manager;
            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            if (!controller.RotateItem(resouces.GetArc()))
            {
                TransitionToState(VillagerState.IDLE);
            }
            else
            {
                woodsPos = sm.GetClosestTransform(manager.GetWoods()).position;
                SetTargetToRandomWoodsPos();
                controller.Move(target);
            }
            Debug.Log(this.Name);


        }
        public override void OnUpdate()
        {
            if (sm.IsInDanger()) TransitionToState(VillagerState.CHASEGOBLIN);
            else if (sm.IsPreyNear()) TransitionToState(VillagerState.HUNTRABIT);
            else
            {
                if (manager.AgentNavMesh.remainingDistance == 0 && manager.AgentNavMesh.pathPending == false)
                {
                    SetTargetToRandomWoodsPos();
                    controller.Move(target);
                }
            }
        }
        public override void OnExit()
        {
            controller.StopMoving();
        }

        private void SetTargetToRandomWoodsPos()
        {
            target = woodsPos + Random.insideUnitSphere * sm.maxWoodsDistance;
            target.y = 0;
        }

    }
    public class HuntRabitState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Agent_System_Manager manager;
        Vector3 woodsPos;
        Transform target;


        public override void OnEnter()
        {

            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            manager = sm.manager;
            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            if (!controller.RotateItem(resouces.GetArc()))
            {
                TransitionToState(VillagerState.IDLE);
            }
            else
            {
                target = sm.GetClosestTransform(sm.mobsState.GetRabitsList());
                controller.Move(target.position);
            }
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {
            if (sm.IsInDanger())
            {
                TransitionToState(VillagerState.CHASEGOBLIN);
            }
            else
            {
                if (sm.CloseEnoughtToInteract(target)) TransitionToState(VillagerState.ATTACKRABIT);
                else controller.Move(target.position);
            }

        }
        public override void OnExit()
        {
            controller.StopMoving();
        }

    }



    public class AttackRabitState : AbstractState
    {

        VillagerStateMachine sm;
        AgentController controller;
        Transform target;
        public override void OnEnter()
        {

            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            if (sm.mobsState.GetRabitsList().Count == 0)
            {
                TransitionToState(VillagerState.IDLE);
            }
            else
            {
                target = sm.GetClosestTransform(sm.mobsState.GetRabitsList());
            }
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {

            if (sm.IsInDanger()) TransitionToState(VillagerState.CHASEGOBLIN);
            else if (sm.mobsState.GetRabitsList().Count == 0 || target == null) TransitionToState(VillagerState.IDLE);
            else
            {
                float distance = Vector3.Distance(target.position, sm.transform.position);
                if (!sm.CloseEnoughtToInteract(target)) TransitionToState(VillagerState.HUNTRABIT);
                else
                {
                    controller.LookAt(target.position);
                    controller.Interact();
                }
            }

        }

    }
    public class ChaseGoblinState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Agent_System_Manager manager;
        Vector3 woodsPos;
        Transform target;


        public override void OnEnter()
        {
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            manager = sm.manager;

            if (!sm.IsInDanger())
            {
                TransitionToState(VillagerState.IDLE);
            }
            else
            {

                Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
                if (!controller.RotateItem(resouces.GetArc()))
                {
                    if (!controller.RotateItem(resouces.GetEspasa())) TransitionToState(VillagerState.IDLE);
                }

                target = sm.GetClosestTransform(sm.mobsState.GetGoblinList());
                controller.Move(target.position);
            }
            Debug.Log(this.Name);
        }

        public override void OnUpdate()
        {
            if (!sm.IsInDanger())
            {
                TransitionToState(VillagerState.IDLE);
            }
            else
            {
                if (sm.CloseEnoughtToInteract(target)) TransitionToState(VillagerState.ATTACKGOBLIN);
                else controller.Move(target.position);
            }

        }
        public override void OnExit()
        {
            controller.StopMoving();
        }
    }
    public class AttackGoblinState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Transform target;
        public override void OnEnter()
        {

            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            if (!sm.IsInDanger())
            {
                TransitionToState(VillagerState.IDLE);
            }
            else
            {
                target = sm.GetClosestTransform(sm.mobsState.GetGoblinList());
            }
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {

            if (!sm.IsInDanger()) TransitionToState(VillagerState.IDLE);
            else
            {
                if (target == null) TransitionToState(VillagerState.CHASEGOBLIN);
                else
                {
                    float distance = Vector3.Distance(target.position, sm.transform.position);
                    if (!sm.CloseEnoughtToInteract(target)) TransitionToState(VillagerState.CHASEGOBLIN);
                    else
                    {
                        controller.LookAt(target.position);
                        controller.Interact();
                    }
                }


            }

        }
        public override void OnExit()
        {
            controller.StopMoving();
        }

    }
    public class GoHomeState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Agent_System_Manager manager;

        public override void OnEnter()
        {
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            manager = controller.manager;
            sm.ReserveClosestHouse();

            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            controller.RotateItem(resouces.GetEmpyHand());
            controller.Move(manager.house.entrancePos.position);
        }
        public override void OnUpdate()
        {

            if (sm.IsInDanger())
            {
                if (controller.manager.HasWeapons())
                {
                    sm.RemoveHouseReserve();
                    TransitionToState(VillagerState.CHASEGOBLIN);

                }
                else
                {
                    if (!manager.house.IsDoorOpen())
                    {
                        if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                        {
                            controller.LookAt(manager.house.door.position);
                            controller.Interact();
                            controller.Move(manager.house.exitPos.position);
                        }
                    }
                    else
                    {
                        if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                        {
                            controller.LookAt(manager.house.door.position);
                            controller.Interact();
                            TransitionToState(VillagerState.IDLE);
                        }

                    }
                }

            }
            else
            {

                if (!manager.house.IsDoorOpen())
                {

                    if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                    {
                        controller.LookAt(manager.house.door.position);
                        controller.Interact();
                        controller.Move(manager.house.exitPos.position);
                    }
                }
                else
                {
                    if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                    {
                        controller.LookAt(manager.house.door.position);
                        controller.Interact();
                        TransitionToState(VillagerState.IDLE);
                    }

                }
            }
        }
        public override void OnExit()
        {
            controller.StopMoving();
        }

    }

    public class SleepState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        Transform bed;
        HealthManager healthManager;
        bool sleeping;

        public override void OnEnter()
        {
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;
            bed = sm.manager.house.bed.transform;
            healthManager = controller.manager.HealthManager;
            sleeping = false;

            controller.Move(bed.position);
            controller.RotateItem(Item_Template_Resouces.GetInstance().GetEmpyHand());
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {
            if (!sleeping)
            {
                float distance = Vector3.Distance(bed.position, controller.transform.position);
                if (distance < controller.manager.getCurrentInteractDistance())
                {
                    controller.StopMoving();
                    controller.LookAt(bed.position);
                    controller.Interact();
                    sleeping = true;
                }
            }
            else
            {
                if (sm.IsInDanger() && controller.manager.HasWeapons()) TransitionToState(VillagerState.GETOUTHOME);
                else if (healthManager.hunger == 100) TransitionToState(VillagerState.EAT);
                else if (healthManager.sleepiness == 0) TransitionToState(VillagerState.IDLE);

            }
        }



        public override void OnExit()
        {
            controller.WakeUp();
        }

    }

    public class GetOutHomeState : AbstractState
    {
        VillagerStateMachine sm;
        AgentController controller;
        public override void OnEnter()
        {
            sm = GetStateMachine<VillagerStateMachine>();
            controller = sm.controller;

            Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
            controller.RotateItem(resouces.GetEmpyHand());
            controller.Move(sm.manager.house.exitPos.position);
            Debug.Log(this.Name);
        }
        public override void OnUpdate()
        {
            if (sm.IsInDanger())
            {
                if (controller.manager.HasWeapons())
                {
                    if (sm.manager.house.IsDoorOpen())
                    {
                        sm.RemoveHouseReserve();
                        TransitionToState(VillagerState.CHASEGOBLIN);
                    }
                    else
                    {

                        if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                        {
                            controller.LookAt(sm.manager.house.door.position);
                            controller.Interact();
                            sm.RemoveHouseReserve();
                            TransitionToState(VillagerState.CHASEGOBLIN);
                        }
                    }

                }
                else
                {
                    if (!sm.manager.house.IsDoorOpen())
                    {
                        TransitionToState(VillagerState.IDLE);
                    }
                    else
                    {
                        TransitionToState(VillagerState.GOHOME);
                    }
                }

            }
            else
            {
                if (sm.manager.house.IsDoorOpen())
                {
                    if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                    {
                        controller.LookAt(sm.manager.house.door.position);
                        controller.Interact();
                        sm.RemoveHouseReserve();
                        TransitionToState(VillagerState.IDLE);
                    }

                }
                else
                {

                    if (controller.manager.AgentNavMesh.remainingDistance == 0 && controller.manager.AgentNavMesh.pathPending == false)
                    {
                        controller.LookAt(sm.manager.house.door.position);
                        controller.Interact();
                        controller.Move(sm.manager.house.entrancePos.position);
                    }
                }
            }




        }
        public override void OnExit()
        {
            controller.StopMoving();

        }

    }
}
