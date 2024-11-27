using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlControler : MonoBehaviour
{
    [SerializeField] private float nivel = 0;
    [SerializeField] private float experiencia = 0;

    public void AddExperiencia(float value) // Método para sumar experiencia
    {
        experiencia += value;
        Debug.Log("experiencia");
        // Comprobar y actualizar el nivel según la experiencia acumulada
        switch (nivel)
        {
            case 0: // Subir a nivel 1
                if (experiencia > 350)
                {
                    experiencia = experiencia - 350;
                    nivel = 1;
                    Debug.Log("¡Subiste a nivel 1!");
                }
                break;

            case 1: // Subir a nivel 2
                if (experiencia > 900)
                {
                    experiencia = experiencia - 900;
                    nivel = 2;
                    Debug.Log("¡Subiste a nivel 2!");
                }
                break;

            case 2: // Subir a nivel 3
                if (experiencia > 2000)
                {
                    experiencia = experiencia - 2000;
                    nivel = 3;
                    Debug.Log("¡Subiste a nivel 3!");
                }
                break;

            case 3: // Subir a nivel 4
                if (experiencia > 4000)
                {
                    experiencia = experiencia - 4000;
                    nivel = 4;
                    Debug.Log("¡Subiste a nivel 4!");
                }
                break;

            default:
                Debug.Log("No hay más niveles disponibles.");
                break;
        }
    }
}


