using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    
    [Header ("Enemy Objects")]
    [SerializeField] private GameObject enemyPrefabComponents;
    [SerializeField] private GameObject explosion;
    
    [Header("Enemy Movement Settings")]
    [SerializeField] private float retreatDistance = 1f;    // Distancia para retroceder si está demasiado cerca
    [SerializeField] private float moveSpeed = 3.5f;        // Velocidad del enemigo (NavMeshAgent)

    [Header("Enemy Shooting Settings")]
    [SerializeField] private Transform spawnPoint;          // Punto desde donde se disparan las balas
    [SerializeField] private GameObject bulletPrefab;       // Prefab de la bala
    [SerializeField] private float shootRange = 5f;         // Rango máximo para disparar
    [SerializeField] private float fireRate = 1.5f;        // Tiempo entre disparos
    [SerializeField] private float projectileLifeTime, despawnLifetime = 2f;

    private Transform projectilePool;      // Pool de proyectiles
    private Transform activeProjectilePool; // Pool de proyectiles activos
    
    [Header ("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] private Animator animator;
    private int currentHealth;

    private NavMeshAgent _agent;
    private Transform _player;
    private float _nextFireTime;
    private bool _canSpawn = true;
    
    private readonly int _explosionHash = Animator.StringToHash("EnemyExplosion");
    private readonly int _explosionIdleHash = Animator.StringToHash("ExplosionIdle");

    private Animator _anim;
    private bool _isDestroyed; // Controla si el el enemigo ya fue destruido
    
    

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        // Inicializa el NavMeshAgent
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = moveSpeed; // Configurar velocidad mediante SerializeField
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        currentHealth = maxHealth;
        _isDestroyed = false;

        // Encuentra al jugador
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Debug.Log("Player found");
            _player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con la etiqueta 'Player'.");
        }

        // Asegura que el enemigo esté sobre el NavMesh
        EnsureAgentOnNavMesh();
        
        projectilePool = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<Transform>();
        activeProjectilePool = GameObject.FindGameObjectWithTag("ActiveProjectilePool").GetComponent<Transform>();
        
    }
    
    

    private void Update()
    {
        if (_player == null)
        {
            Debug.LogError("Player is null! Enemy cannot follow.");
            return;
        }

        if (!_agent.isOnNavMesh)
        {
            Debug.LogError($"Enemy {name} is not on the NavMesh!");
            return;
        }
        
        _agent.SetDestination(_player.position);


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
    
    public void Die()
    {
        if (_isDestroyed) return; // Evitar múltiples llamadas a la destrucción
        
        _isDestroyed = true;
        currentHealth = maxHealth;
        enemyPrefabComponents.SetActive(false); // Desactivar el modelo del enemigo
        explosion.SetActive(true); // Activar la explosión
        _anim.Play(_explosionHash); // Reproducir animación de explosión
        explosion.SetActive(false);
        GameObject.FindWithTag("Player").GetComponent<LvlControler>().AddExperiencia(30);
        
        float animationDuration = GetAnimationClipLength("EnemyExplosion");

        // Reproduce la animación y espera a que termine antes de realizar otras acciones
        Invoke(nameof(PlayExplosionIdle), animationDuration); // Cambia al estado idle después de la explosión
        Invoke(nameof(ReturnToPool), animationDuration + 0.1f); // Desactiva el GameObject después
    }
    
    private void PlayExplosionIdle()
    {
        _anim.Play(_explosionIdleHash);
    }

    private float GetAnimationClipLength(string clipName)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the EnemyController!");
            return 0f; // Return a default value to prevent further errors
        }
        // Ensure the Animator's RuntimeAnimatorController is valid
        if (animator.runtimeAnimatorController != null)
        {
            // Loop through all animation clips in the Animator's controller
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName) // Check for a matching clip name
                {
                    return clip.length; // Return the length of the matching clip
                }
            }
        }
        

        Debug.LogWarning($"Animation clip '{clipName}' not found in Animator!");
        return 0f; // Return 0 if the clip is not found
    }

    
    private void ReturnToPool()
    {
        // Reset enemy visibility and deactivate
        enemyPrefabComponents.SetActive(true); // Reset for reuse
        gameObject.SetActive(false);
        transform.SetParent(GameObject.FindWithTag("EnemyPool").transform); // Move back to the pool
    }

}
