using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroide_logica : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float puntos;
    void Start()
    {
        
    }
    public float setPuntos{
        set { puntos = value; }
    }
    public float Vida
    {
        get { return vida; } // Obtener el valor actual
        set { vida = value; } // Asignar un nuevo valor
    }
    public void hacerDanio(float vida_quitar)
    {
        vida = vida - vida_quitar;
        Debug.Log("me quitaste vida mi vida ahora es"+ vida + "menos" + vida_quitar);
        if (vida <= 0)
        {
            eliminarme();
            Debug.Log("eliminaste un asteroide");
        }
    }

    public void eliminarme()
    {
        GameObject player = GameObject.Find("Player");
        LvlControler script_puntos = player.GetComponent<LvlControler>(); // componente script de lvl
        script_puntos.AddExperiencia(puntos);
        Destroy(gameObject);
    }



}
