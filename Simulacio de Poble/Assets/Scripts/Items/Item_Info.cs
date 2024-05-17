using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Info
{
    public Item_Template template;
    public int item_ID; // 0-99 reserved for custom items

    public bool Equals(Item_Info other)
    {
        return this.item_ID == other.item_ID;
    }
}
