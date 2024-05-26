using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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


    public void Start()
    {
        FieldsStateManager instance = FieldsStateManager.GetInstance();
        instance.GetEmptyFields().TryAdd(transform, false);

    }
    void Update()
    {
        if (State == S_Harvest.Planted) PlantedLogic();


    }

    public void Interact(Agent_System_Manager actor, Item item)
    {

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
            FieldsStateManager instance = FieldsStateManager.GetInstance();
            instance.GetFullFields().Add(transform, false);
        }
    }

    private void InteractionGrowed(Inventary inventary)
    {
        State = S_Harvest.Empty;
        growed.SetActive(false);

        Item_Info harvest_item_info = ItemFactory.GetInstance().CreateItemID(harvest);
        inventary.addItem(harvest_item_info);
        FieldsStateManager instance = FieldsStateManager.GetInstance();
        instance.GetFullFields().Remove(transform);
        instance.GetEmptyFields().Add(transform, false);
    }
    private void InteractionEmpty()
    {
        State = S_Harvest.Planted;
        growTimer = Time.time + timeToGrow;
        planted.SetActive(true);

        FieldsStateManager.GetInstance().GetEmptyFields().Remove(transform);


    }
}
