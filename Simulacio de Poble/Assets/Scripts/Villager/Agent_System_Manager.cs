using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_System_Manager : MonoBehaviour
{
    private Inventary inventary;
    private HealthManager healthManager;
    public PlayerController playerController;
    public Bed bed;

    public Inventary Inventary { get => inventary; set => inventary = value; }
    public HealthManager HealthManager { get => healthManager; set => healthManager = value; }
    public PlayerController PlayerController { get => playerController; set => playerController = value; }





    // Start is called before the first frame update
    void Start()
    {
        inventary = gameObject.GetComponent<Inventary>();
        healthManager = gameObject.GetComponent<HealthManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
