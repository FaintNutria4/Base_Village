using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class Inventary : MonoBehaviour
{

    public List<Item_Info> items = new List<Item_Info>();

    public Item currentItem = null;
    public Item_Template hand_template;
    private Item_Info hand;

    public Transform itemHolder;

    internal void addItem(Item_Info item_info)
    {
        if (!items.Contains(item_info))
        {
            items.Add(item_info);
            if (currentItem == null)
            {
                currentItem = Create_Item(item_info);
            }
        }

    }

    internal void addItem(List<Item_Info> item_List)
    {

        foreach (Item_Info item in item_List) addItem(item);

    }

    internal void DeleteCurrentItem(Item_Info item_info)
    {
        items.Remove(item_info);

        if (currentItem == null) return;

        Item _currentItem = currentItem.GetComponent<Item>();

        if (_currentItem.item_info.item_ID == item_info.item_ID)
        {
            currentItem = null;
        }


    }

    internal void ChangeCurrentItem(Item_Info item_info)
    {
        if (currentItem != null) DestroyImmediate(currentItem.gameObject);


        currentItem = Create_Item(item_info);
    }

    private Item Create_Item(Item_Info item_Info)
    {


        GameObject gameObject = (GameObject)Instantiate(item_Info.template.prefab, itemHolder.position, item_Info.template.prefab.GetComponent<Transform>().rotation, itemHolder);
        Item item = gameObject.AddComponent<Item>();
        item.item_info = item_Info;
        return item;

    }

    public void Interact(Agent_System_Manager agent_System_Manager)
    {
        currentItem.Interact(agent_System_Manager);
    }


    // Start is called before the first frame update

    void Start()
    {
        ItemFactory factory = ItemFactory.GetInstance();
        addItem(factory.CreateItemID(hand_template)); // Give hand as start item
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int currentItemIndex = 0;
    public void RotateItem()
    {
        if (items.Count > currentItemIndex + 1)
        {
            currentItemIndex++;
        }
        else
        {
            currentItemIndex = 0;
        }
        Item_Info selectedItem = items[currentItemIndex];
        ChangeCurrentItem(selectedItem);
    }

    public List<Item_Info> GetCleanInventary()  //Inventary with no hand
    {
        List<Item_Info> aux = new List<Item_Info>(items);
        aux.Remove(hand);
        return aux;
    }
}
