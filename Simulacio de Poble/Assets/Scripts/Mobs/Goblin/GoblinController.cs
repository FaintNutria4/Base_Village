using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public GoblinHealth goblinHealth;
    public GoblinAttack goblinAttack;
    public Vector3 spawnPos;
    public List<GameObject> villagers = new List<GameObject>();
    public Transform head;




    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        goblinHealth = GetComponent<GoblinHealth>();
        goblinAttack = GetComponent<GoblinAttack>();
        spawnPos = transform.position;
    }

    public void Move(Vector3 point)
    {
        navMeshAgent.SetDestination(point);
    }

    public void LookAt(Vector3 target)
    {

        Vector3 aux = target;
        aux.y = gameObject.transform.position.y; // the y axis must be equal to body y to not rotate the whole body to look the ground

        gameObject.transform.LookAt(aux);
        head.LookAt(target);
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
