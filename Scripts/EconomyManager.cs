using UnityEngine;
using System.Collections.Generic;

public class EconomyManager : MonoBehaviour
{
    [Header("Referencias")]
    public PopulationManager popManager;   // arrastra tu PopulationManager
    public RacePriceTable precios;      // arrastra la tabla de arriba

    [Header("Monedas jugador")]
    public int monedas = 500;

    /* ------------ API pública ------------- */

    public bool Comprar(RaceDefinition raza, bool macho)
    {
        int precio = PrecioCompra(raza);
        if (monedas < precio) return false;

        monedas -= precio;
        popManager.SpawnBatch(raza, macho ? 1 : 0, macho ? 0 : 1);
        return true;
    }

    public bool Vender(LupanyxDigitalis indiv)
    {
        int precio = PrecioVenta(indiv.race);
        monedas += precio;
        popManager.individuos.Remove(indiv);
        Destroy(indiv.gameObject);
        return true;
    }

    /* ------------ HELPERS ------------- */

    int PrecioCompra(RaceDefinition r)
    {
        foreach (var f in precios.precios)
            if (f.raza == r) return f.precioCompra;
        return 9999;
    }

    int PrecioVenta(RaceDefinition r)
    {
        foreach (var f in precios.precios)
            if (f.raza == r) return f.precioVenta;
        return 0;
    }
}
