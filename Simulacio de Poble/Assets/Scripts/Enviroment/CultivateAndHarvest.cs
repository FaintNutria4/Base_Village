using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CultivateAndHarvest : MonoBehaviour, I_Interactable
{
    private enum S_Harvest { Empty, Planted, Growed }

    public Tool_Template tool;
    public Item_Template harvest;
    public GameObject planted;
    public GameObject growed;

    private S_Harvest State = 0;
    public float timeToGrow = 60;

    private float growTimer;

    void Update()
    {
        if (State == S_Harvest.Planted) PlantedLogic();


    }

    public void Interact(Agent_System_Manager actor, Item item)
    {
        ;
        if (item.item_info.template.name_ID != tool.name_ID) return;

        Inventary inventary = actor.Inventary;
        switch (State)
        {

            case S_Harvest.Planted:

                break;

            case S_Harvest.Growed:

                InteractionGrowed(inventary);

                break;

            case S_Harvest.Empty:
                InteractionEmpty();

                break;
        }
    }

    private void PlantedLogic()
    {
        if (Time.time >= growTimer)
        {
            State = S_Harvest.Growed;
            planted.SetActive(false);
            growed.SetActive(true);
        }
    }

    private void InteractionGrowed(Inventary inventary)
    {
        State = S_Harvest.Empty;
        growed.SetActive(false);

        Item_Info harvest_item_info = ItemFactory.GetInstance().CreateItemID(harvest);
        inventary.addItem(harvest_item_info);
    }
    private void InteractionEmpty()
    {
        State = S_Harvest.Planted;
        growTimer = Time.time + timeToGrow;
        planted.SetActive(true);
    }
}
