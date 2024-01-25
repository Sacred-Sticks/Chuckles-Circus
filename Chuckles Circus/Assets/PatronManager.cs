using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    int maxPatrons;
    int deployRate;


    [SerializeField] GameObject patronprefab;
    [SerializeField] Transform[] patronSpawns;
    [SerializeField] int defaultMaxPatrons;
    [SerializeField,
        Tooltip("Each tick, a 0-100 roll must be >= this value to spawn a patron.")] int defaultDeployRate;

    [Header("--- DEBUG VALS ---")]
    [SerializeField] bool gameStart;
    [SerializeField] int cumCounter;
    [SerializeField] Dictionary<int, Patron> patrons;
    void Start()
    {
        patrons = new Dictionary<int, Patron>();

        if (defaultDeployRate == 0)
        {
            // set deploy rate, rolled every tick 1-100.
            // needs to roll above the rate to deploy a patron
            deployRate = 80;
        }
        else
            deployRate = defaultDeployRate;

        if (defaultMaxPatrons == 0)
        {
            // set max patrons to default
            maxPatrons = 20;
        }
        else
            maxPatrons = defaultMaxPatrons;

        StartGame();
    }

    public void StartGame()
    {
        // set up the logic for sending in new patrons
        gameStart = true;
        InvokeRepeating("GameTick", 1f, 1f);
    }

    void ChangeMaxPatrons(int newMax)
    {
        maxPatrons = newMax;
    }
    public void SpawnPatron()
    {
        if (!PopulationUnderMax())
            return;

        Debug.Log("[PatronManager] Spawning patron...");
        // determine the random stats the patron will start with.
        // lets try having a pool of total points that this code will decide how to allocate.
        // Ultimately, Id like to make it so if a person has more money they are more depressed to begin

        if (patronSpawns.Length>0)
        {
            Transform chosen = patronSpawns[(int) Random.Range(0,patronSpawns.Length)];
            Instantiate(patronprefab, chosen.position, chosen.rotation);
            // the patron itself will handle setting it's stats.
            // Set the ID from the new patron as to avoid a get component call

            Debug.Log("[PatronManager] Spawned a patron at a random spawn.");
        }
        else
        {
            Instantiate(patronprefab);
            Debug.Log("[PatronManager] No patron spawns set. Default spawn used.");
        }
    }

    public int RegisterPatron(Patron patron)
    {
        int patronID = cumCounter++;

        if (!patrons.ContainsKey(patronID))
        {
            patrons.Add(patronID, patron);
            Debug.Log("Added new patron to dict.");
            return patronID;
        }

        Debug.Log("Dict already contains patron with same ID");
        return -1;
    }

    public void RegisterAttraction(Attraction built)
    {

    }

    bool PopulationUnderMax()
    {
        if (patrons.Count < maxPatrons)
            return true;
        else return false;
    }
    public void GameTick()
    {
        // deploy patrons based on deploy rate
        int roll = Random.Range(0, 100);
        Debug.Log("[Patron Manager] Rolled for deploy" + roll);
        if (roll >= deployRate)
        {
            if (patrons.Count < maxPatrons)
            {
                Debug.Log("[PatronManager] Pop not max. Can spawn.");
                SpawnPatron();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
