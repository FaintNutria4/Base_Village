using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu]
public class Tool_Template : Item_Template
{

    public override void Interact(Agent_System_Manager actor, Item item)
    {

        UnityEngine.Transform head = actor.head;
        Ray ray = new Ray(head.position, head.forward);



        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance, LayerMask.GetMask("Tool_Interactable")))
        {

            I_Interactable interactable = (I_Interactable)hitInfo.transform.gameObject.GetComponent(typeof(I_Interactable));

            interactable.Interact(actor, item);
        }

    }


}
