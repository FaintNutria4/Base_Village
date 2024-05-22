using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabitController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Vector3 spawnPos;
    public List<GameObject> villagers = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        spawnPos = transform.position;
    }

    public void Move(Vector3 point)
    {
        navMeshAgent.SetDestination(point);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Villager")
        {
            villagers.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Villager")
        {
            villagers.Remove(other.gameObject);

        }
    }
}
