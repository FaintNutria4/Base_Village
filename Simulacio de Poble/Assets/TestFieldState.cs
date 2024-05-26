using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestFieldState : MonoBehaviour
{

    private int counter = 0;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Number of fields before removing: " + FieldsStateManager.GetInstance().GetEmptyFields().Count);
            var a = FieldsStateManager.GetInstance().GetEmptyFields().Keys;
            //FieldsManager.GetInstance().GetEmptyFields().TryRemove(a.First());
            Debug.Log("Number of fields after removing: " + FieldsStateManager.GetInstance().GetEmptyFields().Count);
        }
        if (counter != FieldsStateManager.GetInstance().GetEmptyFields().Count)
        {
            counter = FieldsStateManager.GetInstance().GetEmptyFields().Count;
            Debug.Log("Number of empty fields: " + counter);
        }

    }
}
