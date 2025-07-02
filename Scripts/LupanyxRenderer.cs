using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LupanyxRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Asigna el SpriteRenderer desde el Inspector

    public TMP_Text Sexo;
    public TMP_Text PelajeColorLabel;
    public TMP_Text PatronLabel;
    public TMP_Text PelajeLargo;
    public TMP_Text DietaLabel;
    public TMP_Text IQLabel;
    public TMP_Text CriasProm;
    public TMP_Text VidaProm;
    public TMP_Text AlturaCm;
    public TMP_Text PesoKg;
    public void RenderPhenotype(Phenotype phenotype)
    {
        // Aqu� puedes implementar la l�gica para renderizar el fenotipo
        // Por ejemplo, podr�as cambiar el color del objeto seg�n el fenotipo

        Sexo.text            = phenotype.Sexo;
        spriteRenderer.color = phenotype.PelajeColor;
        PelajeColorLabel.text= phenotype.PelajeColorLabel;
        PatronLabel.text     = phenotype.PatronLabel;
        PelajeLargo.text     = phenotype.PelajeLargo ? "Largo" : "Corto";
        DietaLabel.text      = phenotype.DietaLabel;
        IQLabel.text         = phenotype.IQLabel;
        CriasProm.text       = phenotype.CriasProm.ToString();
        VidaProm.text        = phenotype.VidaProm.ToString("F1") + " años";
        AlturaCm.text        = phenotype.AlturaCm.ToString("F1") + " cm";
        PesoKg.text          = phenotype.PesoKg.ToString("F1") + " kg";
        
        // Tambi�n podr�as mostrar otros atributos del fenotipo en la consola o UI
        Debug.Log(phenotype.PhenotypeString());
    }


}
