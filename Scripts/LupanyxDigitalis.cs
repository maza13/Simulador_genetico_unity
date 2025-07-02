using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LupanyxDigitalis : MonoBehaviour
{
    public DADN dADN;

    public RaceDefinition race;
    public void Init(RaceDefinition race, bool? esMacho = null)
    {
        dADN = new DADN(race, esMacho);
        if (dADN != null)
        {
            dADN.PrintStringADN();
            this.race = race;
            Phenotype p = PhenotypeResolver.Resolve(dADN);
            Debug.Log($"{p.PhenotypeString()}");
        }
        else
        {
            Debug.LogError("DADN is not assigned.");
        }
    }
    
    public void Init(DADN madre, DADN padre)
    {

        dADN = new DADN(madre, padre);
        if (dADN != null)
        {
            dADN.PrintStringADN();
            Phenotype p = PhenotypeResolver.Resolve(dADN);
            Debug.Log(p.PhenotypeString());
        }
        else
        {
            Debug.LogError("DADN is not assigned.");
        }
    }



}
