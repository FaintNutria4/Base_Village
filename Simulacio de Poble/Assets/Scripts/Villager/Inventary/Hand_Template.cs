using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu]
public class Hand_Template : Item_Template
{

    public override void Interact(Agent_System_Manager actor, Item item)
    {

        UnityEngine.Transform head = actor.head;
        Ray ray = new Ray(head.position, head.forward);



        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance, LayerMask.GetMask("Hand_Interactable")))
        {

            I_Interactable interactable = (I_Interactable)hitInfo.transform.gameObject.GetComponent(typeof(I_Interactable));

            interactable.Interact(actor, item);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}