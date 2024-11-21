using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem; // El sistema de partículas
    [SerializeField] private float particleSpeedMultiplier = 1f; // Multiplicador de velocidad

    private Rigidbody2D rb; // Rigidbody del jugador

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb == null || particleSystem == null) return;

        // Obtener la dirección y velocidad del movimiento del jugador
        Vector2 velocity = rb.velocity;
        float speed = velocity.magnitude;

        // Configurar la velocidad de las partículas
        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.x = velocity.x * particleSpeedMultiplier;
        velocityOverLifetime.y = velocity.y * particleSpeedMultiplier;

        // Activar o desactivar el sistema de partículas según el movimiento
        if (speed > 0.1f) // Si el jugador se está moviendo
        {
            if (!particleSystem.isPlaying)
                particleSystem.Play();
        }
        else
        {
            if (particleSystem.isPlaying)
                particleSystem.Stop();
        }
    }
}