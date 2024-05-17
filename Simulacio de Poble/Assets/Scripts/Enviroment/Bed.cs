using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Bed : MonoBehaviour, I_Interactable
{
    public int bed_ID;
    public Transform sleepPosition;
    public Transform wakeTransform;
    public Transform actortransform;
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
        actor.bed = this;

    }


    public void WakeUp()
    {

        actortransform.SetPositionAndRotation(wakeTransform.position, wakeTransform.rotation);
        healthManager.SetIsAwake(true);
        agent.enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        BedIDFactory factory = BedIDFactory.GetInstance();
        bed_ID = factory.GetID();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
