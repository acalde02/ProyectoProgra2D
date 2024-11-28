using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlControler : MonoBehaviour
{
    [SerializeField] private float nivel = 0;
    [SerializeField] private float experiencia = 0;
    [SerializeField] private int nivel1;
    [SerializeField] private int nivel2;
    [SerializeField] private int nivel3;
    public Sprite spritelvl1;
    public Sprite spritelvl2;
    public Sprite spritelvl3;


    public void AddExperiencia(float value) // Método para sumar experiencia
    {
        experiencia += value;
        Debug.Log("experiencia");
        // Comprobar y actualizar el nivel según la experiencia acumulada
        GameObject player = GameObject.Find("PlayerBody");
        

        switch (nivel)
        {

            
            case 0: // Subir a nivel 1
                if (experiencia > nivel1)
                {
                    player.GetComponent<SpriteRenderer>().sprite = spritelvl1;
                    experiencia = experiencia - nivel1;
                    nivel = 1;
                    Debug.Log("¡Subiste a nivel 2!");
                }
                break;

            case 1: // Subir a nivel 2
                if (experiencia > nivel2)
                {
                    player.GetComponent<SpriteRenderer>().sprite = spritelvl2;
                    experiencia = experiencia - nivel2;
                    nivel = 2;
                    Debug.Log("¡Subiste a nivel 3!");
                }
                break;

            case 2: // Subir a nivel 3
                if (experiencia > nivel3)
                {
                    player.GetComponent<SpriteRenderer>().sprite = spritelvl3;
                    experiencia = experiencia - nivel3;
                    nivel = 3;
                    Debug.Log("¡Subiste a nivel 4!");
                }
                break;

            default:
                Debug.Log("No hay más niveles disponibles.");
                break;
        }
    }
}


