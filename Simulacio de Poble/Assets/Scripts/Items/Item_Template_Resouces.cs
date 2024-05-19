using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Template_Resouces : MonoBehaviour
{
    public Item_Template[] templates;
    private static Item_Template_Resouces instance;

    public static Item_Template_Resouces GetInstance()
    {
        if (instance == null)
        {
            instance = (Instantiate(Resources.Load("Item_Template_Resources")) as GameObject).GetComponent<Item_Template_Resouces>();
        }
        return instance;
    }

    public Item_Template GetEmpyHand()
    {
        return Array.Find(templates, item => item.name_ID == "Ma");
    }
    public Item_Template GetAixida()
    {
        return Array.Find(templates, item => item.name_ID == "Aixida");
    }
    public Item_Template GetArc()
    {
        return Array.Find(templates, item => item.name_ID == "Arc");
    }
    public Item_Template GetEspasa()
    {
        return Array.Find(templates, item => item.name_ID == "Espasa");
    }
    public Item_Template GetCarn()
    {
        return Array.Find(templates, item => item.name_ID == "Carn");
    }
    public Item_Template GetBlat()
    {
        return Array.Find(templates, item => item.name_ID == "Blat");
    }
}
