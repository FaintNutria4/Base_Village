using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour
{
    enum classtype
    {
        Recolecter,
        Hunter,
        Warrior
    }
    Item_Template_Resouces resouces;
    ItemFactory itemFactory;
    // Start is called before the first frame update
    void Start()
    {
        resouces = Item_Template_Resouces.GetInstance();
        itemFactory = ItemFactory.GetInstance();
        List<Agent_System_Manager> villagers = VillagersStateManager.GetInstance().GetVillagers();

        foreach (Agent_System_Manager agent in villagers)
        {
            GiveItems(agent);
        }

    }

    private classtype i = classtype.Recolecter;

    private void GiveItems(Agent_System_Manager manager)
    {
        switch (i)
        {
            case classtype.Recolecter:
                manager.Inventary.addItem(itemFactory.CreateItemID(resouces.GetAixida()));
                i = classtype.Hunter;
                break;
            case classtype.Hunter:
                manager.Inventary.addItem(itemFactory.CreateItemID(resouces.GetArc()));
                i = classtype.Warrior;
                break;
            case classtype.Warrior:
                manager.Inventary.addItem(itemFactory.CreateItemID(resouces.GetEspasa()));
                i = classtype.Recolecter;
                break;
        }
    }

}
