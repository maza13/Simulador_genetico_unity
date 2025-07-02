using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// ------------- DADN - ADN de animales digitales ------------------- //

// ------------- ENUMS PARA ALELOS ------------------- //

public enum Gen_Color { B, M, E, G, W, D, S, A, R } // Negro, Marrón, Beige, Gris, Blanco, Dorado, Azul, Albino, Rojo
public enum Gen_Patron { S, P, H }                    // Sólido, PechoPatas, CabezaPechoPatas
public enum Gen_Pelaje { S, L }                       // Corto, Largo
public enum Gen_Dieta { H, OH, O, OC, C }            // Omnívoro, Omnívoro-carne, Omnívoro-hierba, Carnívoro, Herbívoro
public enum Gen_Inteligencia { L, ML, M, MH, H }            // Bajo, Medio-bajo, Medio, Medio-alto, Alto
public enum CromosomaSexual { X, Y }

// ------------- ESTRUCTURA PARA CUALQUIER PAR DE GENES ------------------- //

[Serializable]
public struct ParGen<Alelo>
{
    public Alelo AleloA;
    public Alelo AleloB;
    
    public ParGen(Alelo a, Alelo b) { AleloA = a; AleloB = b; }

    public Alelo AleloRandom()
    {
        System.Random rng = new System.Random();
        return (rng.Next(2) == 0) ? AleloA : AleloB;
    }

    public override string ToString() => $"[{AleloA}, {AleloB}]";
}

// ------------- CLASE PRINCIPAL DE DADN ------------------- //

[Serializable]
public class DADN
{

    // Genotipo/Fenotipo
    public ParGen<CromosomaSexual> gen_sexo;
    public ParGen<Gen_Color> gen_color;
    public ParGen<Gen_Patron> gen_patron;
    public ParGen<Gen_Pelaje> gen_pelaje;
    public ParGen<Gen_Dieta> gen_dieta;
    public ParGen<Gen_Inteligencia> gen_inteligencia;
    public ParGen<float> gen_crias;          // Ejemplo: [4.12,3.96]
    public ParGen<float> gen_vida;           // Ejemplo: [12.0f, 11.9f]
    public ParGen<float> gen_altura_X;         // altura hembra Ejemplo: [78.1f, 81.2f]
    public ParGen<float> gen_altura_Y;         // altura macho Ejemplo:  [120.1f, 122.2f]
    public ParGen<float> gen_masa;           // Ejemplo: [0.38f, 0.4f]


    // Genes núcleo especie/raza
    public ParGen<string>[] RS = new ParGen<string>[10];
    // Genes individuales/familia
    public ParGen<string>[] GS = new ParGen<string>[10];


    // --------- CONSTRUCTOR BASE
    public DADN()
    {
        for (int i = 0; i < 10; i++)
        {
            RS[i] = new ParGen<string>("A1", "A1");
        }

        for (int i = 0; i < 10; i++)
        {
            GS[i] = new ParGen<string>("Z9", "Z9");
        }

    }

    // --------- CONSTRUCTOR CON RAZA DEFINIDA
    public DADN(RaceDefinition raza, bool? forzarMacho = null)
    {
        System.Random rng = new System.Random();

        if (rng == null) rng = new System.Random();

        /* --------- SEXO ---------- */
        bool esMacho = forzarMacho ?? (rng.Next(2) == 0);
        gen_sexo = esMacho
            ? new ParGen<CromosomaSexual>(CromosomaSexual.X, CromosomaSexual.X)
            : new ParGen<CromosomaSexual>(CromosomaSexual.X, CromosomaSexual.Y);

        /* --------- COLOR / PATRÓN / PELAJE: mutación rara ---------- */
        gen_color = new ParGen<Gen_Color>(
            MutarAlelo(raza.aleloColorA, raza.pMutColor, rng),
            MutarAlelo(raza.aleloColorB, raza.pMutColor, rng));

        gen_patron = new ParGen<Gen_Patron>(
            MutarAlelo(raza.aleloPatronA, raza.pMutPatron, rng),
            MutarAlelo(raza.aleloPatronB, raza.pMutPatron, rng));

        gen_pelaje = new ParGen<Gen_Pelaje>(
            MutarAlelo(raza.aleloPelajeA, raza.pMutPelaje, rng),
            MutarAlelo(raza.aleloPelajeB, raza.pMutPelaje, rng));

        /* --------- DIETA / INTELIGENCIA con distribución discreta ---------- */
        gen_dieta = new ParGen<Gen_Dieta>(
            SorteaDiscreto<Gen_Dieta>(raza.probDieta, rng),
            SorteaDiscreto<Gen_Dieta>(raza.probDieta, rng));

        gen_inteligencia = new ParGen<Gen_Inteligencia>(
            SorteaDiscreto<Gen_Inteligencia>(raza.probInteligencia, rng),
            SorteaDiscreto<Gen_Inteligencia>(raza.probInteligencia, rng));

        /* --------- NÚMEROS CON DISTRIBUCIÓN NORMAL ---------- */
        gen_crias = new ParGen<float>(
            GaussClamp(raza.criasMin, raza.criasMax, rng),
            GaussClamp(raza.criasMin, raza.criasMax, rng));

        gen_vida = new ParGen<float>(
            GaussClamp(raza.vidaMin, raza.vidaMax, rng),
            GaussClamp(raza.vidaMin, raza.vidaMax, rng));

        gen_altura_Y = new ParGen<float>(
            GaussClamp(raza.alturaMachoMin, raza.alturaMachoMax, rng),
            GaussClamp(raza.alturaMachoMin, raza.alturaMachoMax, rng));

        gen_altura_X = new ParGen<float>(
            GaussClamp(raza.alturaHembraMin, raza.alturaHembraMax, rng),
            GaussClamp(raza.alturaHembraMin, raza.alturaHembraMax, rng));

        gen_masa = new ParGen<float>(
            GaussClamp(raza.masaMin, raza.masaMax, rng),
            GaussClamp(raza.masaMin, raza.masaMax, rng));

        /* --------- RS & GS permanecen puros por ahora ---------- */
        for (int i = 0; i < 10; i++)
        {
            RS[i] = new ParGen<string>(raza.RS_Puros[i], raza.RS_Puros[i]);

            // Letra desde 'A' hasta 'Z' (26 letras, sin ñ)
            char letra_A = (char)('A' + rng.Next(0, 26));
            // Dígito entre '0' y '9'
            char digito_A = (char)('0' + rng.Next(0, 10));
            
            // Letra desde 'A' hasta 'Z' (26 letras, sin ñ)
            char letra_B = (char)('A' + rng.Next(0, 26));
            // Dígito entre '0' y '9'
            char digito_B = (char)('0' + rng.Next(0, 10));

            GS[i] = new ParGen<string>($"{letra_A}{digito_A}", $"{letra_B}{digito_B}"); ;

        }
    }

    /* ========== HELPERS DADN RACE DEFINITION ========== */

    // Mutación puntual: con probabilidad p elige otro alelo aleatorio del mismo enum
    private T MutarAlelo<T>(T aleloBase, float pMut, System.Random rng)
    {
        if (rng.NextDouble() >= pMut) return aleloBase;

        Array valores = Enum.GetValues(typeof(T));
        T nuevo;
        do { nuevo = (T)valores.GetValue(rng.Next(valores.Length)); }
        while (nuevo.Equals(aleloBase));
        return nuevo;
    }

    // Gauss clamp dentro de min-max
    private float GaussClamp(float min, float max, System.Random rng)
    {
        double mu = (min + max) / 2.0;
        double sigma = (max - min) / 8.0;                   // ±3σ cubre rango
        double u1 = 1.0 - rng.NextDouble();                 // Box-Muller
        double u2 = 1.0 - rng.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                               Math.Sin(2.0 * Math.PI * u2);
        double valor = mu + sigma * randStdNormal;
        return (float)Math.Round(Mathf.Clamp((float)valor, min, max), 2);
    }

    // Sorteo discreto con probabilidades
    private U SorteaDiscreto<U>(float[] probs, System.Random rng)
    {
        float p = (float)rng.NextDouble();
        float acumulado = 0f;
        for (int i = 0; i < probs.Length; i++)
        {
            acumulado += probs[i];
            if (p <= acumulado)
                return (U)Enum.GetValues(typeof(U)).GetValue(i);
        }
        // Fallback
        return (U)Enum.GetValues(typeof(U)).GetValue(probs.Length - 1);
    }



    // --------- CONSTRUCTOR PARA REPRODUCCIÓN
    public DADN(DADN madre, DADN padre)
    {
        // Sexo: padre hereda X o Y, madre hereda solo X
        gen_sexo = new ParGen<CromosomaSexual>(madre.gen_sexo.AleloRandom(), padre.gen_sexo.AleloRandom());

        // Genes discretos: alelo random de cada progenitor
        gen_color        = new ParGen<Gen_Color>        (madre.gen_color.AleloRandom(),        padre.gen_color.AleloRandom());
        gen_patron       = new ParGen<Gen_Patron>       (madre.gen_patron.AleloRandom(),       padre.gen_patron.AleloRandom());
        gen_pelaje       = new ParGen<Gen_Pelaje>       (madre.gen_pelaje.AleloRandom(),       padre.gen_pelaje.AleloRandom());
        gen_dieta        = new ParGen<Gen_Dieta>        (madre.gen_dieta.AleloRandom(),        padre.gen_dieta.AleloRandom());
        gen_inteligencia = new ParGen<Gen_Inteligencia> (madre.gen_inteligencia.AleloRandom(), padre.gen_inteligencia.AleloRandom());

        // Genes numéricos: alelo random + mutación pequeña (±2%)
        gen_vida = new ParGen<float>(
            MutarNum(madre.gen_vida.AleloRandom(), 0.02f),
            MutarNum(padre.gen_vida.AleloRandom(), 0.02f));
        gen_crias = new ParGen<float>(
            MutarNum(madre.gen_crias.AleloRandom(), 0.05f),
            MutarNum(padre.gen_crias.AleloRandom(), 0.05f));

        // Gen de altura, esta ligado al sexo.
        gen_altura_X = new ParGen<float>(
            MutarNum(madre.gen_altura_X.AleloRandom(), 0.02f),
            MutarNum(padre.gen_altura_X.AleloRandom(), 0.02f));
        gen_altura_Y = new ParGen<float>(
            MutarNum(madre.gen_altura_Y.AleloRandom(), 0.02f),
            MutarNum(padre.gen_altura_Y.AleloRandom(), 0.02f));
        
        gen_masa = new ParGen<float>(
            MutarNum(madre.gen_masa.AleloRandom(), 0.05f),
            MutarNum(padre.gen_masa.AleloRandom(), 0.05f));

        // Genes núcleo especie/raza (RS) y familia/individuales (GS): igual, alelo random por cada uno
        for (int i = 0; i < 10; i++)
        {
            RS[i] = new ParGen<string>(
                madre.RS[i].AleloRandom(),
                padre.RS[i].AleloRandom());
            GS[i] = new ParGen<string>(
                madre.GS[i].AleloRandom(),
                padre.GS[i].AleloRandom());
        }

    }

    // Función para mutar números (puedes ajustar el rango)
    private float MutarNum(float baseValor, float maxPorc)
    {
        System.Random rng = new System.Random();

        int muta = rng.Next(1, 100);
        if (muta >= (maxPorc*100)) return baseValor; // Sin mutación

        float factor = 1f + ((float)rng.NextDouble() * 2f - 1f) * maxPorc;
        return (float)Math.Round(baseValor * factor, 2);
    }

    // --------- CONVERSIÓN A TEXTO LEGIBLE
    public void PrintStringADN()
    {
        string adn_text = $"Sexo: {gen_sexo}, Color: {gen_color}, Patron: {gen_patron}, Pelaje: {gen_pelaje}, Dieta: {gen_dieta}, Crias: {gen_crias}, Vida: {gen_vida}, Altura X: {gen_altura_X}, Altura Y: {gen_altura_Y}, Masa: {gen_masa}, Int: {gen_inteligencia}";
        string GS_text = $"GS0: {GS[0]}, GS1: {GS[1]}, GS2: {GS[2]}, GS3: {GS[3]}, GS4: {GS[4]}, GS5: {GS[5]}, GS6: {GS[6]}, GS7: {GS[7]}, GS8: {GS[8]}, GS9: {GS[9]}";

        string RS_text = $"RS0: {RS[0]}";

        for (int i = 1; i < 10; i++)
        {
            RS_text += $", RS{i}: {RS[i]}";
        }

        adn_text += "\n" + RS_text;
        adn_text += "\n" + GS_text;

        Debug.Log(adn_text);
    }
}

