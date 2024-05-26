using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoblinHealth : MobHealth
{
    public List<Item_Info> loot = new List<Item_Info>();

    public void Start()
    {
        MobsStateManager.GetInstance().GetGoblinList().Add(transform);
    }

    internal override void Die(Agent_System_Manager killer)
    {
        killer.Inventary.addItem(loot);
        MobsStateManager.GetInstance().GetGoblinList().Remove(transform);

        base.Die(killer);


    }

}
