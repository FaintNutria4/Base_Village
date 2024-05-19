using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIDTester : MonoBehaviour
{
    public Item_Template item_Template;
    // Start is called before the first frame update
    void Start()
    {
        ItemFactory factory = ItemFactory.GetInstance();

        Item_Info item = factory.CreateItemID(item_Template);

        Debug.Log(item.item_ID);
        item = factory.CreateItemID(item_Template);
        Debug.Log(item.item_ID);
        item = factory.CreateItemID(item_Template);
        Debug.Log(item.item_ID);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
