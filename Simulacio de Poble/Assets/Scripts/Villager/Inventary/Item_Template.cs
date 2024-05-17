using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public abstract class Item_Template : ScriptableObject
{

    public abstract void Interact(Agent_System_Manager actor, Item item);

    public String name_ID;

    public UnityEngine.Object prefab;


}
