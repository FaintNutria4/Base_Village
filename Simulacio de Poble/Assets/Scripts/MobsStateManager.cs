using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MobsStateManager
{
    private static MobsStateManager instance = null;
    private List<Transform> rabitsList = new List<Transform>();
    private List<Transform> goblinList = new List<Transform>();


    public static MobsStateManager GetInstance()
    {

        instance ??= new MobsStateManager();

        return instance;
    }

    public List<Transform> GetRabitsList()
    {
        return rabitsList;
    }

    public List<Transform> GetGoblinList()
    {
        return goblinList;
    }
}