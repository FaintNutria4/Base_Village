using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestFieldState2 : MonoBehaviour
{
    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var fields = FieldsStateManager.GetInstance().GetEmptyFields().Where(pair => pair.Value == true).Select(pair => pair.Key);
        if (counter != fields.Count())
        {
            counter = fields.Count();
            Debug.Log("number of fields reserved: " + counter);
        }
    }
}
