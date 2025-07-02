using UnityEngine;

[CreateAssetMenu(fileName = "TablaPreciosRaza", menuName = "SimuladorGenetico/TablaPrecios")]
public class RacePriceTable : ScriptableObject
{
    [System.Serializable]
    public struct Fila
    {
        public RaceDefinition raza;
        public int precioCompra;  // monedas para comprar 1 ejemplar
        public int precioVenta;   // lo que recibes al vender 1 adulto
    }

    public Fila[] precios;
}
