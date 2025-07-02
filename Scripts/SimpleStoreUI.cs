using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleStoreUI : MonoBehaviour
{
    public EconomyManager eco;
    public TMP_Text txtMonedas;
    public TMP_Dropdown dropdownRaza;
    public Toggle toggleMacho;

    void Start()
    {
        // Llenar dropdown con nombres
        dropdownRaza.ClearOptions();
        var opciones = new System.Collections.Generic.List<string>();
        foreach (var f in eco.precios.precios) opciones.Add(f.raza.nombreRaza);
        dropdownRaza.AddOptions(opciones);

        ActualizarTexto();
    }

    public void BtnComprar()
    {
        var raza = eco.precios.precios[dropdownRaza.value].raza;
        bool ok = eco.Comprar(raza, toggleMacho.isOn);
        if (!ok) Debug.Log("No tienes suficientes monedas");
        ActualizarTexto();
    }

    /* Para vender llama esta función desde un botón que tengas en cada lobo (ejemplo) */
    public void VenderIndividuo(LupanyxDigitalis lobo)
    {
        eco.Vender(lobo);
        ActualizarTexto();
    }

    void ActualizarTexto() => txtMonedas.text = $"Monedas: {eco.monedas}";
}
