using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;

public class Agent_System_Manager : MonoBehaviour
{
    private Inventary inventary;
    private HealthManager healthManager;
    private AgentController agentController;
    private NavMeshAgent agentNavMesh;
    public Transform head;
    public House house;

    public Inventary Inventary { get => inventary; set => inventary = value; }
    public HealthManager HealthManager { get => healthManager; set => healthManager = value; }
    public AgentController AgentController { get => agentController; set => agentController = value; }
    public NavMeshAgent AgentNavMesh { get => agentNavMesh; set => agentNavMesh = value; }





    // Start is called before the first frame update
    void Start()
    {
        inventary = gameObject.GetComponent<Inventary>();
        healthManager = gameObject.GetComponent<HealthManager>();
        agentController = gameObject.GetComponent<AgentController>();
        agentNavMesh = gameObject.GetComponent<NavMeshAgent>();
        VillagersStateManager.GetInstance().GetVillagers().Add(this);

    }

    // Update is called once per frame
    internal bool HasWeapons()
    {
        return inventary.items.Exists(x => x.template is Weapon_Template);
    }

    public bool HasFood()
    {
        return inventary.items.Exists(x => x.template is Food_Template);
    }
    public bool IsAwake()
    {
        return HealthManager.isAwake;
    }
    public bool isHome()
    {
        return house != null;
    }

    public bool CanHarvest()
    {
        return inventary.items.Exists(x => x.template.name_ID == "Aixida");
    }

    public bool CanHunt()
    {
        return inventary.items.Exists(x => x.template.name_ID == "Arc");
    }
    public float getCurrentInteractDistance()
    {
        return inventary.currentItem.item_info.template.interactionDistance;
    }
    public List<Transform> GetWoods()
    {
        return WoodsManager.GetInstance().GetWoods();
    }
    public FieldsStateManager GetFields()
    {
        return FieldsStateManager.GetInstance();
    }
}
