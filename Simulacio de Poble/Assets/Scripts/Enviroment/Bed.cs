using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Bed : MonoBehaviour, I_Interactable
{
    public Transform sleepPosition;
    public Transform wakeTransform;
    public Transform actortransform;
    private House house;
    HealthManager healthManager;

    NavMeshAgent agent;


    public void Interact(Agent_System_Manager actor, Item item)
    {
        if (item.item_info.template.name_ID != "Ma") return;


        healthManager = actor.HealthManager;
        healthManager.SetIsAwake(false);
        agent = actor.GetComponent<NavMeshAgent>();
        agent.enabled = false;

        actortransform = actor.transform;

        wakeTransform.SetPositionAndRotation(actortransform.position, actortransform.rotation);
        actortransform.SetPositionAndRotation(sleepPosition.position, sleepPosition.rotation);

    }


    public void WakeUp()
    {

        actortransform.SetPositionAndRotation(wakeTransform.position, wakeTransform.rotation);
        healthManager.SetIsAwake(true);
        agent.enabled = true;
    }

    public void SetUpHouse(House house)
    {
        this.house = house;
    }
}
