using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, I_DamageTaker
{

    public bool isAwake = true;
    public float health = 100;
    public float maxHealth = 100;


    /*  1 dia temps simulat = 24 min reals
        Una persona dorm 8h de les 24h del dia, deixant 16h de hores actives
        100 Max sleepiness / 16 hores simulades = 6,25 sleepiness/ h simulades
        Passant-lo a sleepiness/ sec reals:
        6,25 sleepiness/ h simulades *  1 h simulades / 1 min real * 1 min real/ 60 sec reals = 6,25/60

        Seguint la mateixa logica la recuperació per sec real és = 12,5/60 (dormir 8h simulades)
    */
    public float sleepiness = 0;
    public float sleepinessForSecond = 6.25f / 60;
    public float sleepinessRecover = -12.5f / 60; // valor negatiu per recuperar quan sumem el valor a la sleepiness
    public float hunger = 0;
    public float hungerForSecond = 100f / 48 * 1f / 60;
    public float needsLackDamage = 2f / 60;
    public Agent_System_Manager agent;

    public void TakeDamage(Agent_System_Manager actor, Item item)
    {
        Weapon_Template weapon = (Weapon_Template)item.item_info.template;
        health -= weapon.damage;
        if (health < 0) Die(actor);
    }

    private void TakeNecessityDamage()
    {
        health -= needsLackDamage * Time.deltaTime;
        if (health < 0) Die();
    }

    public List<Item_Info> TakeDamage(float damage) //Method for mob dmg
    {

        health -= damage;
        if (health < 0)
        {
            return agent.Inventary.GetCleanInventary();
        }
        else return null;
    }

    private void Die(Agent_System_Manager killer)
    {
        Inventary killerInventary = killer.Inventary;
        Inventary inventary = agent.Inventary;


        killerInventary.addItem(inventary.GetCleanInventary());
        DestroyImmediate(gameObject);


    }

    private void Die()
    {
        DestroyImmediate(gameObject);
    }

    internal void AddHunger(int hunger)
    {
        float aux = this.hunger += hunger;
        if (aux > 100)
        {
            this.hunger = 100;
            return;
        }
        else if (aux < 0)
        {
            this.hunger = 0;
            return;
        }
        this.hunger = aux;
    }

    public void SetIsAwake(bool isAwake)
    {
        this.isAwake = isAwake;
    }



    private void AddSleepiness(float sleepiness)
    {
        float aux = this.sleepiness += sleepiness;
        if (aux > 100)
        {
            this.sleepiness = 100;
            return;
        }
        else if (aux < 0)
        {
            this.sleepiness = 0;
            return;
        }
        this.sleepiness = aux;
    }

    private void SleepLogic()
    {
        if (isAwake)
        {
            if (sleepiness == 100) TakeNecessityDamage();
            AddSleepiness(sleepinessForSecond * Time.deltaTime);
        }
        else
        {
            AddSleepiness(sleepinessRecover * Time.deltaTime);
        }
    }

    private void AddHunger(float hunger)
    {
        float aux = this.hunger += hunger;
        if (aux > 100)
        {
            this.hunger = 100;
            return;
        }
        else if (aux < 0)
        {
            this.hunger = 0;
            return;
        }
        this.hunger = aux;
    }
    private void HungerLogic()
    {
        if (hunger == 100) TakeNecessityDamage();
        AddHunger(hungerForSecond * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SleepLogic();
        HungerLogic();
    }
}
