using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedIDFactory
{
    private static BedIDFactory bedFactory = null;

    private int id = 0;

    public static BedIDFactory GetInstance()
    {

        bedFactory ??= new BedIDFactory();

        return bedFactory;
    }

    public int GetID()
    {
        int aux = id;
        id++;
        return aux;
    }
}
