using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{


    public Agent_System_Manager manager;



    // Start is called before the first frame update
    void Start()
    {
        manager = gameObject.GetComponent<Agent_System_Manager>();
    }


    public void AddItem(Item_Template item_Info)
    {
        Inventary inventary = manager.Inventary;
        ItemFactory itemFactory = ItemFactory.GetInstance();
        inventary.addItem(itemFactory.CreateItemID(item_Info));
    }

    public void RotateItem()
    {
        if (!IsAwake()) return;
        manager.Inventary.RotateItem();
    }

    public void RotateItem(Item_Info item_Info)
    {
        if (!IsAwake()) return;
        manager.Inventary.ChangeCurrentItem(item_Info);
    }

    public bool IsAwake()
    {
        return manager.HealthManager.isAwake;
    }

    public void WakeUp()
    {
        if (IsAwake()) return;
        manager.bed.WakeUp();
    }
    public void Move(Vector3 target)
    {
        if (!IsAwake()) return;
        manager.AgentNavMesh.SetDestination(target);

    }

    public void Interact()
    {
        if (!IsAwake()) return;
        Inventary inventary = manager.Inventary;
        inventary.Interact(manager);

    }


    public void LookAt(Vector3 target)
    {
        if (!IsAwake()) return;
        Vector3 aux = target;
        aux.y = manager.gameObject.transform.position.y; // the y axis must be equal to body y to not rotate the whole body to look the ground

        manager.gameObject.transform.LookAt(aux);
        manager.head.LookAt(target);



    }
}
