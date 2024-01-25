using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patron : MonoBehaviour
{
    // Patrons will have a max potential money (bank), depression stat
    // Bank: variable used to determine how much can be taken from this npc
    // depression: stat will determine the roll from the station this npc is visiting. Will start anywhere between 0-15 and increase by 1 per 30 seconds.
    // visiting : stores a reference to the component of the station this npc decided to visit
    // if bank runs out or depression reaches 20, the npc leaves the map.
    // I set up depression so the ceiling can be raised via a method. 
    // This can allow a player to purchase a tech that allows residents to become more depressed, which makes them spend money
    // Each transaction will minus money but also minus depression.


    [SerializeField] TextMeshProUGUI cashText;

    [Header("---Spawn Parameters---")]
    [SerializeField] int depressFloor;
    [SerializeField] int depressCeiling;
    [SerializeField] int startCashFloor;
    [SerializeField] int startCashCeiling;
    [SerializeField] int drate; // adds 1 depression every drate game ticks.
    [SerializeField,
    Tooltip("Maximum depression value allowed")]
    int defaultDepressionMax;
    [Header("--- DEBUG VALS ---")]
    [SerializeField] int _ID;
    [SerializeField] int bank;
    [SerializeField] int depression;

    // NON-INSPECTOR SHIT
    PatronManager patronManager;
    NavMeshAgent navAgent;
    int dc = 0;// depression counter
    int depressionMax;
    bool leaving;
    Transform leavePoint;

    void Start()
    {
        patronManager = GameObject.FindObjectOfType<PatronManager>();
        //Register the patron
        _ID = patronManager.RegisterPatron(this);
        leaving = false;
        leavePoint = GameObject.Find("LeavePoint").transform;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        // set stats
        
        depression = Random.Range(depressFloor, depressCeiling);
        bank = Random.Range(startCashFloor, startCashCeiling);

        if (drate == 0)
            drate = 5; // set a default if none set custom

        if (defaultDepressionMax == 0)
            depressionMax = 20; // set a default if none set custom
        else
            depressionMax = defaultDepressionMax;

        InvokeRepeating("GameTick", 1f, 1f);

    }

    public int PayForSomething(int price, int happy)
    {
        if (bank - price <= 0)
        {
            int moneyleft = bank;
            bank -= bank;
            Debug.Log("[Patron " + _ID + "] Is bankrupt. They were just leaving...");
            depression = depressionMax;
            StartLeaveRoutine();
            return moneyleft;
        }

        if (bank - price > 0)
        {
            bank -= price;
            depression -= happy;
            return price;
        }
        // else if they arent giving the remains of their money or paying in full, they shouldnt be in this method
        return -1;
    }
    void DecideWhereToGo()
    {
        // based on money and depression decide where to go.
        // Possibilities: Visit attraction, loiter, leave
        // Depression 20 = leave, depression > 15 = start rolling to add more depression




    }
    void StartLeaveRoutine()
    {
        Debug.Log("[Patron " + _ID + "] Is leaving.");
        navAgent.SetDestination(leavePoint.position);
    }

    void AddDepression()
    {
        depression++;
    }
    void AddDepression(int val)
    {
        depression+=val;
    }

    void CureDepression()
    {
        depression--;
    }

    void CureDepression(int val)
    {
        depression-=val;
    }
    
    void GameTick()
    {
        // become depressed (it's not hard)
        if (dc == drate)
        {
            if (depression < depressionMax)
            {
                AddDepression(1);
            }
                dc = 0;
        } else
        {
            dc++;
        }

        // decide whether or not to KYS after getting +1 depression
        if (depression == depressionMax)
        {
            leaving = true;
            StartLeaveRoutine(); // do it pussy
        }


        cashText.text = "$"+bank.ToString();

    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
