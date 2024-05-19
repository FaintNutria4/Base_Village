using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour, I_DamageTaker
{
    public float health = 30;
    public Item_Template reward;
    public int reward_count = 1;

    // Start is called before the first frame update
    public void TakeDamage(Agent_System_Manager actor, Item item)
    {
        Weapon_Template weapon = (Weapon_Template)item.item_info.template;
        health -= weapon.damage;
        Debug.Log("me diste");
        if (health <= 0) Die(actor);
    }

    private void Die(Agent_System_Manager killer)
    {
        Inventary killerInventary = killer.Inventary;
        ItemFactory itemFactory = ItemFactory.GetInstance();

        for (int i = 0; i < reward_count; i++)
        {
            Item_Info Item_reward = itemFactory.CreateItemID(reward);
            killerInventary.addItem(Item_reward);
        }

        DestroyImmediate(gameObject);

    }
}
