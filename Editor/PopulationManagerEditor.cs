#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PopulationManager))]
public class PopulationManagerEditor : Editor
{
    string[] razaNombres;
    int indexRaza;
    int machos = 1, hembras = 1;
    int idxMacho, idxHembra;

    void OnEnable()
    {
        var pm = (PopulationManager)target;
        razaNombres = new string[pm.razas.Count];
        for (int i = 0; i < razaNombres.Length; i++)
            razaNombres[i] = pm.razas[i].nombreRaza;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();  // todas las variables públicas

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== Generar lote ===", EditorStyles.boldLabel);

        if (razaNombres.Length > 0)
        {
            indexRaza = EditorGUILayout.Popup("Raza", indexRaza, razaNombres);
            machos = EditorGUILayout.IntField("Machos", machos);
            hembras = EditorGUILayout.IntField("Hembras", hembras);

            if (GUILayout.Button("Spawn ▼"))
            {
                var pm = (PopulationManager)target;
                pm.SpawnBatch(pm.razas[indexRaza], machos, hembras);
            }
        }
        else
            EditorGUILayout.HelpBox("Arrastra razas al arreglo 'razas' arriba.", MessageType.Info);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("=== Reproducción rápida ===", EditorStyles.boldLabel);

        var pop = ((PopulationManager)target).individuos;
        EditorGUILayout.LabelField($"Población actual: {pop.Count}");

        idxMacho = EditorGUILayout.IntField("Idx macho", idxMacho);
        idxHembra = EditorGUILayout.IntField("Idx hembra", idxHembra);

        if (GUILayout.Button("Breed ►"))
        {
            var pm = (PopulationManager)target;
            pm.Breed(idxMacho, idxHembra);
        }
    }
}
#endif
