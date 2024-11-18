using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;       // Velocidad de movimiento
    [SerializeField] private float stopDistance = 3f;    // Distancia mínima para detenerse
    [SerializeField] private float shootRange = 5f;      // Rango máximo para disparar
    [SerializeField] private float fireDelay = 3f;       // Delay entre disparos
    [SerializeField] private GameObject bulletPrefab;    // Prefab de la bala
    [SerializeField] private Transform firePoint;        // Punto desde donde dispara


    private Rigidbody2D _rb;
    private Transform player;                            // Referencia al jugador
    private float nextFireTime;                          // Tiempo para el siguiente disparo

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // Encuentra al jugador en la escena (asegúrate de que tenga la etiqueta "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        // Siempre mirar al jugador
        RotateTowardsPlayer();

        // Comprobar la distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance && distanceToPlayer <= shootRange)
        {
            // Si está entre stopDistance y shootRange, moverse hacia el jugador
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= stopDistance)
        {
            // Si está dentro de stopDistance, detenerse
            StopMovement();
        }

        // Solo dispara si está dentro del rango de disparo
        if (distanceToPlayer <= shootRange)
        {
            Shoot();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Moverse hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void StopMovement()
    {
        // Detener al enemigo (esto mantiene su posición)
        _rb.velocity = Vector2.zero;
    }

    private void RotateTowardsPlayer()
    {
        // Dirección hacia el jugador
        Vector2 direction = player.position - transform.position;

        // Calcula el ángulo hacia el jugador
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica la rotación en el eje Z
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Shoot()
    {
        // Solo dispara si ha pasado suficiente tiempo desde el último disparo
        if (Time.time >= nextFireTime)
        {
            // Instancia la bala desde el firePoint en la dirección actual
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Actualiza el tiempo del próximo disparo
            nextFireTime = Time.time + fireDelay;
        }
    }
}
