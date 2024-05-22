using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinHealth : MobHealth
{
    public List<Item_Info> loot = new List<Item_Info>();

    private void Die(Agent_System_Manager killer)
    {
        Inventary killerInventary = killer.Inventary;
        ItemFactory itemFactory = ItemFactory.GetInstance();

        for (int i = 0; i < reward_count; i++)
        {
            Item_Info Item_reward = itemFactory.CreateItemID(reward);
            killerInventary.addItem(Item_reward);
        }

        killerInventary.addItem(loot);
        DestroyImmediate(gameObject);

    }

}
