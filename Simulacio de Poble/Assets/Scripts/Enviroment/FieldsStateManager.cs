using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FieldsStateManager
{
    private static FieldsStateManager instance = null;



    private Dictionary<Transform, bool> emptyFields = new Dictionary<Transform, bool>();
    private Dictionary<Transform, bool> fullFields = new Dictionary<Transform, bool>();


    public static FieldsStateManager GetInstance()
    {

        instance ??= new FieldsStateManager();

        return instance;
    }

    public Dictionary<Transform, bool> GetEmptyFields()
    {
        return emptyFields;
    }

    public Dictionary<Transform, bool> GetFullFields()
    {
        return fullFields;
    }

}