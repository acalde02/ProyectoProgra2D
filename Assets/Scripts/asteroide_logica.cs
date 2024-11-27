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
    public float setVida
    {
        set { vida = value; }
    }
}
