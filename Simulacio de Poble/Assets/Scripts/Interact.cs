using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{

    I_Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E) && interactable != null) interactable.Interact(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<I_Interactable>(out I_Interactable _interactable)) interactable = _interactable;

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.TryGetComponent<I_Interactable>(out I_Interactable _interactable)) interactable = null;
    }
}
