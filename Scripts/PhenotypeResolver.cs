using UnityEngine;
using System;

public struct Phenotype
{
    public string   Sexo;
    public Color    PelajeColor;
    public string   PelajeColorLabel;
    public string   PatronLabel;
    public bool     PelajeLargo;
    public string   DietaLabel;
    public string   IQLabel;
    public int      CriasProm;
    public float    VidaProm;
    public float    AlturaCm;
    public float    PesoKg;

    public string PhenotypeString()
    {
        return $"Sexo: {Sexo}, Color: {PelajeColorLabel}, " + $"Patrón: {PatronLabel}, Pelaje Largo: {PelajeLargo}, " + 
               $"Vida Prom: {VidaProm} años, Altura: {AlturaCm} cm, Peso: {PesoKg} kg, " + 
               $"Crías Prom: {CriasProm}, Dieta: {DietaLabel}, IQ: {IQLabel}";
    }
}

public static class PhenotypeResolver
{
    public static Phenotype Resolve(DADN dna)
    {
        var p = new Phenotype();

        /* ---------- SEXO ---------- */
        bool esMacho = dna.gen_sexo.AleloB == CromosomaSexual.Y;
        p.Sexo = esMacho ? "Macho" : "Hembra";

        /* ---------- COLOR + CROMATICA ---------- */
        var cA = dna.gen_color.AleloA;
        var cB = dna.gen_color.AleloB;
        Gen_Color colorVis = ExpresaColor(cA, cB);

        p.PelajeColor = ColorFromEnum(colorVis);
        Debug.Log($"color rgb = {p.PelajeColor}");
        p.PelajeColorLabel = ColorLabel(colorVis);

        /* ---------- PATRÓN ---------- */
        var patA = dna.gen_patron.AleloA;
        var patB = dna.gen_patron.AleloB;
        var patVis = ExpresaPatron(patA, patB);
        p.PatronLabel = PatronTexto(patVis);

        /* ---------- PELAJE ---------- */
        p.PelajeLargo = (dna.gen_pelaje.AleloA == Gen_Pelaje.L &&
                         dna.gen_pelaje.AleloB == Gen_Pelaje.L);

        /* ---------- DIETA ---------- */
        p.DietaLabel = ExpresaDieta(dna.gen_dieta.AleloA, dna.gen_dieta.AleloB);

        /* ---------- INTELIGENCIA ---------- */
        int iq = (IntValor(dna.gen_inteligencia.AleloA) +
                  IntValor(dna.gen_inteligencia.AleloB)) / 2;
        p.IQLabel = IntTexto(iq);

        /* ---------- CRÍAS & VIDA ---------- */
        p.CriasProm = Mathf.RoundToInt(
            DominanciaMedia(dna.gen_crias.AleloA, dna.gen_crias.AleloB));

        p.VidaProm = Mathf.Round(
            DominanciaMedia(dna.gen_vida.AleloA, dna.gen_vida.AleloB) * 10f) / 10f;

        /* ---------- ALTURA Y PESO ---------- */
        float altura = esMacho
            ? DominanciaMedia(dna.gen_altura_Y.AleloA, dna.gen_altura_Y.AleloB)
            : DominanciaMedia(dna.gen_altura_X.AleloA, dna.gen_altura_X.AleloB);

        p.AlturaCm = Mathf.Round(altura * 10f) / 10f;

        float masa = DominanciaMedia(dna.gen_masa.AleloA, dna.gen_masa.AleloB);
        p.PesoKg = Mathf.Round(altura * masa * 10f) / 10f;

        return p;
    }

    /* ========== REGLAS DE COLOR ========== */

    private static Gen_Color ExpresaColor(Gen_Color a, Gen_Color b)
    {
        // 1. Albino epistático recesivo
        if (a == Gen_Color.A && b == Gen_Color.A) return Gen_Color.A;

        // 2. Azul epistático dominante
        if (a == Gen_Color.S || b == Gen_Color.S) return Gen_Color.S;

        // 3. Codominancias
        if ((a == Gen_Color.B && b == Gen_Color.W) || (a == Gen_Color.W && b == Gen_Color.B))
        {
            float rnd = UnityEngine.Random.value;
            if (rnd > 0.66f)
                return Gen_Color.B; //Negro
            else if (rnd > 0.33f)
                return Gen_Color.W; // Blanco
            return Gen_Color.G; // Gris
        }

        if ((a == Gen_Color.M && b == Gen_Color.W) || (a == Gen_Color.W && b == Gen_Color.M))
        {
            float rnd = UnityEngine.Random.value;
            if (rnd > 0.66f)
                return Gen_Color.M; // Marrón
            else if (rnd > 0.33f)
                return Gen_Color.W; // Blanco
            return Gen_Color.E; // Beige
        }

        // 4. Dorado: mut recesiva → necesita D/D
        if (a == Gen_Color.D && b == Gen_Color.D) return Gen_Color.D;

        // 5. Rojo: dominante parcial (si R presente se mezcla)
        if (a == Gen_Color.R || b == Gen_Color.R)
            return (a == Gen_Color.R && b == Gen_Color.R)
                ? Gen_Color.R       // hipotético R/R
                : Gen_Color.R;      // R expresa sobre fondo

        // 6. Jerarquía simple restante
        Gen_Color[] jer = { Gen_Color.B, Gen_Color.M, Gen_Color.E,
                            Gen_Color.G, Gen_Color.W };
        foreach (var c in jer)
            if (a == c || b == c) return c;

        return a; // fallback
    }

    /* ========== REGLAS DE PATRÓN ========== */
    private static Gen_Patron ExpresaPatron(Gen_Patron a, Gen_Patron b)
    {
        if (a == Gen_Patron.S || b == Gen_Patron.S) return Gen_Patron.S;
        if (a == Gen_Patron.P || b == Gen_Patron.P) return Gen_Patron.P;
        return Gen_Patron.H;
    }

    /* ========== DIETA ========== */
    private static string ExpresaDieta(Gen_Dieta a, Gen_Dieta b)
    {
        // Recesivos completos
        if (a == Gen_Dieta.C && b == Gen_Dieta.C) return "Carnívoro";
        if (a == Gen_Dieta.H && b == Gen_Dieta.H) return "Herbívoro";

        return "Omnívoro";
    }

    /* ========== NUMÉRICOS: dominancia media ========== */
    private static float DominanciaMedia(float a, float b)
    {
        
        return UnityEngine.Random.value < 0.5f ? a : b;

    }

    /* ========== HELPERS VARIOS ========== */

    private static int IntValor(Gen_Inteligencia iq) =>
        iq switch
        {
            Gen_Inteligencia.L => 6,
            Gen_Inteligencia.ML => 7,
            Gen_Inteligencia.M => 8,
            Gen_Inteligencia.MH => 9,
            _ => 10
        };

    private static string IntTexto(int v) =>
        v switch
        {
            6 => "Bajo",
            7 => "Med-Bajo",
            8 => "Medio",
            9 => "Med-Alto",
            _ => "Alto"
        };

    private static string PatronTexto(Gen_Patron p) =>
        p switch
        {
            Gen_Patron.S => "Sólido",
            Gen_Patron.P => "Pecho + patas",
            _ => "Cabeza + pecho + patas"
        };

    private static Color ColorFromEnum(Gen_Color c) => c switch
    {
        Gen_Color.B => new Color(15f / 255f, 15f / 255f, 15f / 255f, 1f),
        Gen_Color.M => new Color(110f / 255f, 65f / 255f, 35f / 255f, 1f),
        Gen_Color.E => new Color(200f / 255f, 175f / 255f, 140f / 255f, 1f),
        Gen_Color.G => new Color(125f / 255f, 125f / 255f, 125f / 255f, 1f),
        Gen_Color.W => new Color(240f / 255f, 240f / 255f, 240f / 255f, 1f),
        Gen_Color.D => new Color(230f / 255f, 185f / 255f, 70f / 255f, 1f),
        Gen_Color.S => new Color(70f / 255f, 85f / 255f, 120f / 255f, 1f),
        Gen_Color.A => new Color(250f / 255f, 250f / 255f, 250f / 255f, 1f),
        Gen_Color.R => new Color(170f / 255f, 35f / 255f, 25f / 255f, 1f),
        _ => new Color(100f / 255f, 100f / 255f, 100f / 255f, 1f)
    };

    private static string ColorLabel(Gen_Color c) => c switch
    {
        Gen_Color.B => "Negro",
        Gen_Color.M => "Marrón",
        Gen_Color.E => "Beige",
        Gen_Color.G => "Gris",
        Gen_Color.W => "Blanco",
        Gen_Color.D => "Dorado",
        Gen_Color.S => "Azul acero",
        Gen_Color.A => "Albino",
        Gen_Color.R => "Rojo intenso",
        _ => "Desconocido"
    };
}
