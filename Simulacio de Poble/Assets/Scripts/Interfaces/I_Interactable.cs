using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface I_Interactable
{
    public void Interact(Agent_System_Manager actor, Item item);

}

