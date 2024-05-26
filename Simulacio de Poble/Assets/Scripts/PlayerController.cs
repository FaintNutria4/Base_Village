using System;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    public NavMeshAgent player;
    public Camera cam;

    public UnityEngine.Transform head;

    public Agent_System_Manager manager;

    public Item_Template item_Info;
    public Item_Template item_Info2;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (IsAwake())
        {
            if (Input.GetMouseButtonDown(0)) Move(Input.mousePosition); //Left click moves
            if (Input.GetMouseButtonDown(1)) LookAt(Input.mousePosition); // Right click looks
            if (Input.GetKeyDown(KeyCode.A))
            {
                Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
                AddItem(resouces.GetEspasa());
                AddItem(resouces.GetArc());


            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
                AddItem(resouces.GetAixida());
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Item_Template_Resouces resouces = Item_Template_Resouces.GetInstance();
                AddItem(resouces.GetBlat());
                AddItem(resouces.GetCarn());
            }

            if (Input.GetKeyDown(KeyCode.I)) Interact();
            if (Input.GetKeyDown(KeyCode.C)) RotateItem();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K)) WakeUp();// Awake if wants to
        }
    }


    public void RotateItem()
    {
        manager.Inventary.RotateItem();
    }

    private void AddItem(Item_Template item_Info)
    {
        Inventary inventary = manager.Inventary;
        ItemFactory itemFactory = ItemFactory.GetInstance();
        inventary.addItem(itemFactory.CreateItemID(item_Info));
    }

    private bool IsAwake()
    {
        return manager.HealthManager.isAwake;
    }

    private void WakeUp()
    {
        if (manager.IsAwake()) return;
        else if (manager.house == null) return;
        else manager.house.bed.WakeUp();
    }
    private void Move(Vector3 target)
    {

        Ray ray = cam.ScreenPointToRay(target);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) player.SetDestination(hit.point);

    }

    private void Interact()
    {
        Inventary inventary = manager.Inventary;
        inventary.Interact(manager);

    }


    private void LookAt(Vector3 target)
    {
        Ray ray = cam.ScreenPointToRay(target);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 aux = hit.point;
            aux.y = player.gameObject.transform.position.y; // the y axis must be equal to body y to not rotate the whole body to look the ground

            player.gameObject.transform.LookAt(aux);
            head.LookAt(hit.point);
        }


    }
}


