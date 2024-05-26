using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WoodsManager
{
    private static WoodsManager instance = null;



    private List<Transform> woods = new List<Transform>();


    public static WoodsManager GetInstance()
    {

        instance ??= new WoodsManager();

        return instance;
    }

    public List<Transform> GetWoods()
    {
        return woods;
    }


}