using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class HousesStateManager
{
    private static HousesStateManager instance = null;
    private Dictionary<House, bool> housesList = new Dictionary<House, bool>();


    public static HousesStateManager GetInstance()
    {

        instance ??= new HousesStateManager();

        return instance;
    }

    public Dictionary<House, bool> GetHousesDictionary()
    {
        return housesList;
    }

    public void AddHouse(House house)
    {
        housesList.Add(house, false);
    }

}