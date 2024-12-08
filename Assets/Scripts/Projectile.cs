using System.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject explosion;

    private readonly int _explosionHash = Animator.StringToHash("Explosion");
    private readonly int _explosionIdleHash = Animator.StringToHash("ExplosionIdle");

    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isDestroyed; // Controla si el proyectil ya fue destruido

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ActivateProjectile();
        _isDestroyed = false; // Resetear estado
        Invoke(nameof(DestroyProjectile), lifeTime); // Destruir automáticamente después del tiempo de vida
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(DestroyProjectile)); // Cancelar destrucción automática si se desactiva
    }

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed) return; // Evitar múltiples interacciones simultáneas
        Debug.Log($"Collision detected with: {other?.gameObject.name} (Tag: {other?.tag})");

        if (other.CompareTag("asteroide"))
        {
            asteroide_logica asteroidScript = other.GetComponent<asteroide_logica>();
            if (asteroidScript != null)
            {
                await Task.Delay(30);
                if(asteroidScript != null)
                {
                    asteroidScript.hacerDanio(damage);
                }
                else
                {
                    return;
                }
                
            }
            DestroyProjectile();
        }

        if (other.CompareTag("PlayerHitbox"))
        {
            GameObject.FindWithTag("Player").GetComponent<Health>()?.TakeDamage(damage);
            DestroyProjectile();
        }

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyProjectile();
            }
            
        }
        
    }

    public void DestroyProjectile()
    {
        if (_isDestroyed) return; // Evitar múltiples llamadas a la destrucción

        _isDestroyed = true;
        _rb.velocity = Vector2.zero; // Detener el movimiento
        projectile.SetActive(false); // Desactivar el modelo del proyectil
        explosion.SetActive(true); // Activar la explosión
        _anim.Play(_explosionHash); // Reproducir animación de explosión

        // Esperar a que termine la animación de explosión antes de desactivar el GameObject
        Invoke(nameof(DeactivateProjectile), 0.5f); // Ajusta el tiempo según la duración de la animación
    }

    private void ActivateProjectile()
    {
        projectile.SetActive(true); // Activar el modelo del proyectil
        explosion.SetActive(false); // Desactivar la explosión
        _anim.Play(_explosionIdleHash); // Reproducir animación inicial
        _rb.velocity = transform.up * speed; // Configurar velocidad del proyectil
    }

    private void DeactivateProjectile()
    {
        gameObject.SetActive(false); // Desactivar el objeto para reutilizarlo en el pool
    }
    public void cambiardaño(int danio)
    {
        damage = damage + danio;
    }
}
