using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{

    private static ItemFactory itemFactory = null;
    private int new_id = 100;

    public static ItemFactory GetInstance()
    {

        itemFactory ??= new ItemFactory();

        return itemFactory;
    }

    public Item_Info CreateItemID(Item_Template item_template)
    {
        Item_Info item = new Item_Info();
        item.template = item_template;
        item.item_ID = new_id;
        new_id++;
        return item;
    }

}
