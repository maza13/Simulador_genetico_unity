using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public PopulationManager populationManager;

    public GameObject lupanyxPrefab;   // arrástralo en el Inspector

    public RaceDefinition razaAlbarino;

    public LupanyxDigitalis lupanyxDigitalis;

    public void teststs()
    {
        
        SpawnLupanxs(2);
        
    }


    public void SpawnLupanxs(int len)
    {
        for (int i = 0; i < len; i++)
        {
            GameObject go = Instantiate(lupanyxPrefab);          // clone
            var lupa = go.GetComponent<LupanyxDigitalis>();      // ya viene en el prefab
            lupa.Init(razaAlbarino);                             // tu método de inicio
        }
    }


}
