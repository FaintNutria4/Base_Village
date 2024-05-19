using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent_System_Manager : MonoBehaviour
{
    private Inventary inventary;
    private HealthManager healthManager;
    private AgentController agentController;
    private NavMeshAgent agentNavMesh;
    public Transform head;
    public Bed bed;

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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
