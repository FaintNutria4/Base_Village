using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class VillagersStateManager
{
    private static VillagersStateManager instance = null;



    private List<Agent_System_Manager> villagers = new List<Agent_System_Manager>();


    public static VillagersStateManager GetInstance()
    {

        instance ??= new VillagersStateManager();

        return instance;
    }

    public List<Agent_System_Manager> GetVillagers()
    {
        return villagers;
    }

}