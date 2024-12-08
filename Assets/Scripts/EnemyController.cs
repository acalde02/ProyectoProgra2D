using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Movement Settings")]
    [SerializeField] private float retreatDistance = 1f;    // Distancia para retroceder si está demasiado cerca
    [SerializeField] private float moveSpeed = 3.5f;        // Velocidad del enemigo (NavMeshAgent)

    [Header("Enemy Shooting Settings")]
    [SerializeField] private Transform spawnPoint;          // Punto desde donde se disparan las balas
    [SerializeField] private GameObject bulletPrefab;       // Prefab de la bala
    [SerializeField] private float shootRange = 5f;         // Rango máximo para disparar
    [SerializeField] private float fireRate = 1.5f;        // Tiempo entre disparos
    [SerializeField] private float projectileLifeTime, despawnLifetime = 2f;
    
    [Header("Projectile Pool Settings")]
    [SerializeField] private Transform projectilePool;      // Pool de proyectiles
    [SerializeField] private Transform activeProjectilePool; // Pool de proyectiles activos
    
    [Header ("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] FloatingHealthBar healthBar;
    private int currentHealth;

    private NavMeshAgent _agent;
    private Transform _player;
    private float _nextFireTime;
    private bool _canSpawn = true;
    
    
    

    private void Start()
    {
        // Inicializa el NavMeshAgent
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = moveSpeed; // Configurar velocidad mediante SerializeField
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        currentHealth = maxHealth;

        // Encuentra al jugador
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
        }

        // Asegura que el enemigo esté sobre el NavMesh
        EnsureAgentOnNavMesh();
    }

    private void Update()
    {
        if (_player == null || !_agent.isOnNavMesh) return;

        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer > retreatDistance)
        {
            // Moverse hacia el jugador
            _agent.SetDestination(_player.position);
        }
        else
        {
            // Retroceder si está demasiado cerca
            RetreatFromPlayer();
        }

        // Disparar si está en rango
        if (distanceToPlayer <= shootRange)
        {
            ShootAtPlayer();
        }

        // Rotar hacia el jugador
        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        if (_player == null) return;

        // Calcula la dirección hacia el jugador
        Vector2 direction = _player.position - transform.position;

        // Calcula el ángulo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 110f;

        // Aplica la rotación al enemigo
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ShootAtPlayer()
    {
        if (!_canSpawn) return;

        _canSpawn = false;
        StartCoroutine(SpawnCooldown());

        GameObject projectile;

        if (projectilePool.childCount <= 0)
        {
            // Aquí se asegura que la rotación coincida con el spawnPoint
            projectile = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            projectile = projectilePool.GetChild(0).gameObject;
            projectile.transform.position = spawnPoint.position;
            projectile.transform.rotation = spawnPoint.rotation; // Ajustar la rotación aquí
            projectile.SetActive(true);
        }

        projectile.transform.SetParent(activeProjectilePool);
        StartCoroutine(DestroyProjectile(projectile.GetComponent<Projectile>()));
    }



    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        _canSpawn = true;
    }

    private IEnumerator DestroyProjectile(Projectile projectile)
    {
        yield return new WaitForSeconds(projectileLifeTime);
        projectile.DestroyProjectile();
        yield return new WaitForSeconds(despawnLifetime);
        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(projectilePool);
    }

    private void RetreatFromPlayer()
    {
        Vector3 retreatDirection = (transform.position - _player.position).normalized;
        Vector3 retreatPosition = transform.position + retreatDirection * retreatDistance;

        // Mover al enemigo a la posición de retirada
        _agent.SetDestination(retreatPosition);
    }

    private void EnsureAgentOnNavMesh()
    {
        if (!_agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
                _agent.enabled = true;
            }
            else
            {
                Debug.LogError($"El agente {gameObject.name} no está sobre el NavMesh y no se encontró un punto cercano.");
                _agent.enabled = false;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Funcion al morir
    }
}
