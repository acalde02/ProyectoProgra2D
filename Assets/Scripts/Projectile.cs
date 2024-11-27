using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    //[SerializeField] private float lifeTime;
    [SerializeField] private int damage = 5;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject explosion;

    private readonly int _explosionHash = Animator.StringToHash("Explosion");
    private readonly int _explosionIdleHash = Animator.StringToHash("ExplosionIdle");

    private Rigidbody2D _rb;
    private Animator _anim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ActivateProjectile();
    }

    private void OnEnable()
    {
        ActivateProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("asteroide"))
        {
            asteroide_logica asteroidScript = other.GetComponent<asteroide_logica>();
            asteroide_logica.setVida(); //arreglar para cambiar la vida y restarle el daño de la bala y si es menor a 0 que se destruya al momento y le sume xp al jugador
            Invoke(nameof(DestroyAfterDelay), 0.05f);
        }
        else
        {
            Debug.Log("La bala ha activado el trigger");
            DestroyProjectile();
        }
    }

    private void DestroyAfterDelay() //esperar antes de destruir para que de tiempo a interactuar con las fisicas del asteroide
    {
        Debug.Log("La bala ha activado el trigger después de esperar");

        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        _rb.velocity = Vector2.zero;
        projectile.SetActive(false);
        explosion.SetActive(true);
        _anim.Play(_explosionHash);
    }

    private void ActivateProjectile()
    {
        projectile.SetActive(true);
        explosion.SetActive(false);
        _anim.Play(_explosionIdleHash);
        _rb.velocity = transform.up * speed;
    }
}
