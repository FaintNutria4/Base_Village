using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{


    public Item_Info item_info;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(Agent_System_Manager actor)
    {
        item_info.template.Interact(actor, this);

    }
}
