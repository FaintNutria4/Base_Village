using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Food_Template : Item_Template
{

    [SerializeField]
    private int healingHunger;

    public int HealingHunger { get => healingHunger; set => healingHunger = value; }

    public override void Interact(Agent_System_Manager manager, Item item)
    {
        Inventary inventary = manager.Inventary;
        HealthManager healthManager = manager.HealthManager;
        healthManager.AddHunger(-healingHunger); // negative healing Hunger to actually heal hunger

        inventary.DeleteCurrentItem(item.item_info);
        Destroy(item.gameObject);
    }
}
