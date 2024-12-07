using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroide_logica : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private float puntos;
    [SerializeField] private GameObject prefap_boost_vida;
    [SerializeField] private GameObject prefap_boost_valocidad_disparo;
    [SerializeField] private GameObject prefap_boost_velocidad_jugador;
    [SerializeField] private GameObject prefap_boost_daño;


    void Start()
    {

    }
    public float setPuntos {
        set { puntos = value; }
    }
    public float Vida
    {
        get { return vida; }
        set { vida = value; }
    }
    public void hacerDanio(float vida_quitar)
    {
        vida = vida - vida_quitar;
        Debug.Log("me quitaste vida mi vida ahora es" + vida + "menos" + vida_quitar);
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

        int numeroAleatorio = Random.Range(1, 16);
        switch (numeroAleatorio)
        {
            case 1: //spawn vida 
               
                Object.Instantiate(prefap_boost_vida, transform.position, transform.rotation);
                
                break;
            case 2://spawn velocidad de disparo
                Object.Instantiate(prefap_boost_valocidad_disparo, transform.position, transform.rotation);
                break;
            case 3: // spawn velocidad jugador
                Object.Instantiate(prefap_boost_velocidad_jugador, transform.position, transform.rotation);
                break;
            case 4: //spawn mas daño de ataque jugador  
                Object.Instantiate(prefap_boost_daño, transform.position, transform.rotation);
                break;

        }
        Debug.Log(numeroAleatorio);










        Destroy(gameObject);
    }



}
