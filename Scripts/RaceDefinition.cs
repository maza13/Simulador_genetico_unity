using UnityEngine;

[CreateAssetMenu(fileName = "NuevaRaza", menuName = "SimuladorGenetico/DefinicionRaza")]
public class RaceDefinition : ScriptableObject
{
    [Header("Información Básica")]
    public string nombreRaza;
    public Color colorRepresentativo;

    [Header("Genes Fenotípicos (Pares de Alelos)")]
    public Gen_Color aleloColorA;
    public Gen_Color aleloColorB;
    [Range(0f, 1f)] public float pMutColor = 0.001f;

    public Gen_Patron aleloPatronA;
    public Gen_Patron aleloPatronB;
    [Range(0f, 1f)] public float pMutPatron = 0.001f;

    public Gen_Pelaje aleloPelajeA;
    public Gen_Pelaje aleloPelajeB;
    [Range(0f, 1f)] public float pMutPelaje = 0.001f;

    [Header("Distribución Dieta (H/OH/O/OC/C)")]
    public Gen_Dieta aleloDietaA;
    public Gen_Dieta aleloDietaB;
    public float[] probDieta = { 0.70f, 0.15f, 0.10f, 0.04f, 0.01f };

    [Header("Distribución Inteligencia (L/ML/M/MH/H)")]
    public Gen_Inteligencia aleloInteligenciaA;
    public Gen_Inteligencia aleloInteligenciaB;
    public float[] probInteligencia = { 0.10f, 0.20f, 0.40f, 0.25f, 0.05f };

    [Header("Rangos Numéricos")]
    [Tooltip("Número de crías promedio y su variación")]
    [Range(1, 6)]public float criasMin;
    [Range(4, 9)]public float criasMax;

    [Tooltip("Esperanza de vida promedio en años y su variación")]
    [Range(5, 12)]public float vidaMin;
    [Range(8, 15)]public float vidaMax;

    [Tooltip("Altura promedio machos en cm y variación")]
    [Range(60, 130)]public float alturaMachoMin;
    [Range(60, 150)]public float alturaMachoMax;

    [Tooltip("Altura promedio hembras en cm y variación")]
    [Range(50, 105)]public float alturaHembraMin;
    [Range(60, 115)]public float alturaHembraMax;

    [Tooltip("Factor promedio de masa corporal y variación")]
    [Range(0.3f, 0.45f)]public float masaMin;
    [Range(0.3f, 0.45f)]public float masaMax;

    [Header("Genes RS y GS (Opcionales por ahora)")]
    public string[] RS_Puros = new string[10]; // Por definir
    public string[] GS_Base = new string[10];  // Por definir

}
