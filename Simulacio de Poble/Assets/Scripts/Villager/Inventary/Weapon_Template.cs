using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon_Template : Item_Template
{

    public enum Weapon_Type
    {
        None,
        Melee,
        Ranged
    }

    public Weapon_Type weapon_Type;
    public float interactionDistance;
    public float damage;


    public override void Interact(Agent_System_Manager actor, Item item)
    {
        Transform head = actor.PlayerController.head;
        Ray ray = new Ray(head.position, head.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance))
        {
            hitInfo.transform.gameObject.TryGetComponent(out I_DamageTaker interactable);

            interactable.TakeDamage(actor, item);
        }

    }
}
