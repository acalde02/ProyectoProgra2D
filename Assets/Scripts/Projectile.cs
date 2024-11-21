using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    //[SerializeField] private float lifeTime;
    [SerializeField] private int damage;
    
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
        // other.GetComponent<Health>()?.TakeDamage(damage);
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
