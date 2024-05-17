using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DoorController : MonoBehaviour, I_Interactable
{

    public Animator animator;

    public bool isOpen = false;

    public void Interact(Agent_System_Manager actor, Item item)
    {
        if (item.item_info.template.name_ID != "Ma") return;
        if (isOpen)
        {
            isOpen = !isOpen;
            animator.SetTrigger("CloseDoor");
        }
        else
        {
            isOpen = !isOpen;
            animator.SetTrigger("OpenDoor");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
