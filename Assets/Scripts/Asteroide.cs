using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroide : MonoBehaviour
{
    public GameObject prefapasteroide;
    [SerializeField] private int numeroAsteroides = 10;
    [SerializeField] private Vector2 spawnArea = new Vector2(33f, 33f);
    [SerializeField] private Vector2 sizeRange = new Vector2(0.5f, 2f);
    public Transform asteroidContainer;
    [SerializeField] private float vida_base = 15;
    [SerializeField] private float puntos_base = 10;
    void Start()
    {
        SpawnAsteroides();
    }

    void SpawnAsteroides()
    {
        for (int i = 0; i < numeroAsteroides; i++)
        {
            
            Vector3 randomPosition = new Vector3(Random.Range(-spawnArea.x / 2, spawnArea.x / 2),Random.Range(-spawnArea.y / 2, spawnArea.y / 2),0f );
            GameObject asteroid = Instantiate(prefapasteroide, randomPosition, Quaternion.identity);
            float randomScale = Random.Range(sizeRange.x, sizeRange.y);
            asteroid.transform.parent = asteroidContainer;
            asteroid.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            asteroide_logica asteroidScript = asteroid.GetComponent<asteroide_logica>();
            if (asteroidScript != null)
            {
                asteroidScript.setPuntos = Mathf.RoundToInt(randomScale * puntos_base);
                asteroidScript.Vida = Mathf.RoundToInt(randomScale * vida_base);
            }
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.drag = randomScale * 2;
            }

        }
    }
}

