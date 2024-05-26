using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{


    public Item_Info item_info;
    private float nextInteractionTime = 0;

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
        if (nextInteractionTime <= Time.time)
        {
            item_info.template.Interact(actor, this);
            nextInteractionTime = Time.time + item_info.template.interactionSpeed;
        }


    }
}
