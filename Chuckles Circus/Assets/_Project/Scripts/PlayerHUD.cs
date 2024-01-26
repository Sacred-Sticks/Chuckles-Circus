using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    public TextMeshProUGUI popIndicator;
    
    void Start()
    {
     
    }

    public void UpdatePopulationIndicator(int population, int maxpop)
    {
        popIndicator.text = "Population: " + population + "/" + maxpop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
