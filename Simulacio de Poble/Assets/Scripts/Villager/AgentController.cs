using System;
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
        if (!manager.IsAwake()) return;
        manager.Inventary.RotateItem();
    }

    public bool RotateItem(Item_Template item_Template)
    {
        if (!manager.IsAwake()) return false;
        Item_Info item_Info = manager.Inventary.getItemInfo(item_Template);
        if (item_Info == null) return false;
        else
        {
            manager.Inventary.ChangeCurrentItem(item_Info);
            return true;
        }
    }

    public void RotateItem(Item_Info item_Info)
    {
        if (!manager.IsAwake()) return;
        manager.Inventary.ChangeCurrentItem(item_Info);
    }



    public void WakeUp()
    {
        if (manager.IsAwake()) return;
        else if (manager.house == null) return;
        else manager.house.bed.WakeUp();
    }
    public void Move(Vector3 target)
    {
        if (!manager.IsAwake()) return;
        manager.AgentNavMesh.SetDestination(target);

    }

    public void Interact()
    {
        if (!manager.IsAwake()) return;
        Inventary inventary = manager.Inventary;
        inventary.Interact(manager);

    }


    public void LookAt(Vector3 target)
    {
        if (!manager.IsAwake()) return;
        Vector3 aux = target;
        aux.y = manager.gameObject.transform.position.y; // the y axis must be equal to body y to not rotate the whole body to look the ground

        manager.gameObject.transform.LookAt(aux);
        manager.head.LookAt(target);

    }

    internal void StopMoving()
    {
        Debug.Log("Stop Moving");
        manager.AgentNavMesh.ResetPath();
    }

}
