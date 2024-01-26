using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum PatronState
{
    Idle = 0,
    MoveToAttraction = 1,
    EnteringPark = 2,
    LeavingPark = 3
}

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
    [SerializeField, Tooltip("Tolerance decreased cured depression from attractions" +
        " and increases the chance for the patron to leave before fully depressed or broke." +
        "Tolerance 1 mitigates cure by 1 and increases chance to leave by 10%. As a trade-off, their bank increases by 50% each point.")] 
                     int tolerance;
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
    [SerializeField] PatronState state;

    // NON-INSPECTOR SHIT
    PatronManager patronManager;
    NavMeshAgent navAgent;
    int dc = 0;// depression counter
    int depressionMax;
    bool leaving;
    Transform exit;
    Transform enter;

    void Start()
    {
        patronManager = GameObject.FindObjectOfType<PatronManager>();
        //Register the patron
        _ID = patronManager.RegisterPatron(this);
        leaving = false;
        exit = patronManager.exits[0];
        enter = patronManager.enterance;
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


        state = PatronState.EnteringPark;
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
    void DecideAIState()
    {
        // based on money and depression decide where to go.
        // Possibilities: Visit attraction, loiter, leave
        // Depression 20 = leave, depression > 15 = start rolling to add more depression

        // simple finite state machine based off the enum. This re-executes every tick.
        switch(state)
        {
            case (PatronState.EnteringPark):
                Debug.Log("[Patron " + _ID + "] Remaining dist to enterance = " + navAgent.remainingDistance);
                if (!navAgent.pathPending) // Check if the path is finished calculating
                {
                    if (navAgent.remainingDistance <= 0.1f && !navAgent.pathPending) // Check if the agent is close enough to the entrance
                    {
                        state = PatronState.Idle;
                    }
                    else if (navAgent.pathPending) // Set the destination if it's not already set
                    {
                        navAgent.SetDestination(enter.position);
                        Debug.Log("[Patron " + _ID + "] Set nav destination to entrance.");
                    }

                }
                break;

            case (PatronState.Idle):
                // if there are no attractions or if the patron is on cooldown
                Vector3 origin = gameObject.transform.position;
                float dist = 10; // maximum wander 10
                int layermask = -1; //all layers

                Vector3 randomDir = UnityEngine.Random.insideUnitSphere * dist;
                randomDir += origin;

                NavMeshHit navHit;

                NavMesh.SamplePosition(randomDir, out navHit, dist, layermask);

                navAgent.SetDestination(navHit.position);
                break;

        }


    }
    void StartLeaveRoutine()
    {

        Debug.Log("[Patron " + _ID + "] Is leaving.");
        navAgent.SetDestination(exit.position);
        // relies on the patron to enter a trigger box with the tag exit to die
        
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("exit"))
        {
            patronManager.UnregisterPatron(_ID);
            Destroy(gameObject);
        }
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
        if (depression == depressionMax && state != PatronState.LeavingPark)
        {
            state = PatronState.LeavingPark;
            StartLeaveRoutine(); // do it pussy
            return;
        }

        DecideAIState(); // do this affter the depression calculation because it will be dependant

        // wait till all actions are done this tick to display new bank
        cashText.text = "$"+bank.ToString();

    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
