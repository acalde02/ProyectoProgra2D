using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlControler : MonoBehaviour
{
    [SerializeField] GameObject experienceManager;
    [SerializeField] private int nivel = 0;
    [SerializeField] private float experiencia = 0;
    [SerializeField] private int nivel1;
    [SerializeField] private int nivel2;
    [SerializeField] private int nivel3;
    public Sprite spritelvl1;
    public Sprite spritelvl2;
    public Sprite spritelvl3;
    
    private ExperienceManager _experienceManager;

    private void Start()
    {
        _experienceManager = experienceManager.GetComponent<ExperienceManager>();
    }

    public int Getnivel()
    {
        return nivel;
    }
    
    public void AddExperiencia(float value) // M�todo para sumar experiencia
    {
        experiencia += value;
        Debug.Log("experiencia");
        // Comprobar y actualizar el nivel seg�n la experiencia acumulada
        GameObject player = GameObject.Find("PlayerBody");
        

        switch (nivel)
        {

            
            case 0: // Subir a nivel 1
                if (experiencia > nivel1)
                {
                    player.GetComponent<SpriteRenderer>().sprite = spritelvl1;
                    experiencia = experiencia - nivel1;
                    nivel = 1;
                    Debug.Log("�Subiste a nivel 2!");
                }
                break;

            case 1: // Subir a nivel 2
                if (experiencia > nivel2)
                {
                    player.GetComponent<SpriteRenderer>().sprite = spritelvl2;
                    experiencia = experiencia - nivel2;
                    nivel = 2;
                    Debug.Log("�Subiste a nivel 3!");
                }
                break;

            case 2: // Subir a nivel 3
                if (experiencia > nivel3)
                {
                    player.GetComponent<SpriteRenderer>().sprite = spritelvl3;
                    experiencia = experiencia - nivel3;
                    nivel = 3;
                    Debug.Log("�Subiste a nivel 4!");
                }
                break;

            default:
                Debug.Log("No hay m�s niveles disponibles.");
                break;
        }
        
        _experienceManager.UpdateInterface();
    }

    public int ReturnGatheredExperience()
    {
        int gatheredExperience = (int)experiencia;
        return gatheredExperience;
    }

    public int ReturnTotalExperience()
    {
        switch (nivel)
        {
            case 0:
                return nivel1;
            case 1:
                return nivel2;
            case 2:
                return nivel3;
            default:
                return 0;
        }
    }
    
}


