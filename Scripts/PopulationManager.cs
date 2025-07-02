using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// Agrégalo a un GameObject vació llamado “GameController” (por ejemplo)
public class PopulationManager : MonoBehaviour
{
    [Header("Prefabs & Razas")]
    public GameObject lupanyxPrefab;            // Prefab con LupanyxDigitalis + LupanyxRenderer
    public List<RaceDefinition> razas;          // Arrastra Umbryon, Albarino, Brunnir …

    [Header("Población en runtime (auto-llenado)")]
    public List<LupanyxDigitalis> individuos = new();

    /* ---------- API pública sencilla ---------- */

    /// Crea n machos y hembras de una raza dada
    public void SpawnBatch(RaceDefinition raza, int machos, int hembras)
    {
        SpawnMany(raza, machos, true);
        SpawnMany(raza, hembras, false);
    }

    /// Aparea dos individuos (índices en la lista) → cría inmediata
    public void Breed(int indexMale, int indexFemale)
    {
        if (!ValidIndex(indexMale) || !ValidIndex(indexFemale))
        {
            Debug.LogWarning("Índices inválidos");
            return;
        }

        var padre = individuos[indexMale].dADN;
        var madre = individuos[indexFemale].dADN;

        if (padre.gen_sexo.AleloB != CromosomaSexual.Y ||
            madre.gen_sexo.AleloB == CromosomaSexual.Y)
        {
            Debug.LogWarning("El orden es (macho, hembra). Corrígelo.");
            return;
        }

        CrearIndividuo(madre, padre);
    }

    /* ---------- Implementación privada ---------- */

    void SpawnMany(RaceDefinition raza, int cantidad, bool machos)
    {
        for (int i = 0; i < cantidad; i++)
        {
            var dna = new DADN(raza, machos);
            CrearIndividuo(raza);
        }
    }

    void CrearIndividuo(DADN madre, DADN padre)
    {
        GameObject go = Instantiate(lupanyxPrefab, transform);
        var lup = go.GetComponent<LupanyxDigitalis>();
        lup.Init(madre, padre);
        individuos.Add(lup);
    }

    void CrearIndividuo(RaceDefinition race)
    {
        GameObject go = Instantiate(lupanyxPrefab, transform);
        var lup = go.GetComponent<LupanyxDigitalis>();
        lup.Init(race);
        individuos.Add(lup);
    }

    bool ValidIndex(int i) => i >= 0 && i < individuos.Count;
}
