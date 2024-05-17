using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactoryTest : MonoBehaviour
{

    static ItemFactory factory;

    public Item_Template item_Template;
    // Start is called before the first frame update
    void Start()
    {
        factory = ItemFactory.GetInstance();
        Item_Info item_Info = factory.CreateItemID(item_Template);
        gameObject.AddComponent<Item>().item_info = item_Info;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
