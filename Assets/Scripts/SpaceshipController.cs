using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(50f, 2000f)]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _previousPosition; // Guarda la posición previa para revertir en caso de colisión

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 movement = MovementLogic();
        _rb.velocity = movement;
        RotateTowardsDirection(movement);

        // Guarda la posición actual como la previa antes de mover la nave
        _previousPosition = transform.position;
    }

    private Vector2 MovementLogic()
    {
        var horizontal = InputManager.Instance.GetHorizontalMovement();
        var vertical = InputManager.Instance.GetVerticalMovement();

        // Combine horizontal and vertical inputs
        Vector2 direction = new Vector2(horizontal, vertical);

        // Scale direction vector, then normalize to prevent redundant calculations
        return direction.normalized * speed * Time.deltaTime;
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0.01f) // Evita rotaciones si el movimiento es insignificante
        {
            // Calcula el ángulo usando Mathf.Atan2 (el eje Y es positivo hacia arriba en Unity)
            float targetAngle = Mathf.Atan2(direction.x * (-1), -direction.y * (-1)) * Mathf.Rad2Deg;

            // Suaviza la rotación hacia el ángulo objetivo
            float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

            // Aplica la rotación
            transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
        }
    }

    // Detecta colisiones con objetos que tienen el tag "Limite"
    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        if (collision.gameObject.CompareTag("limite"))
        {
            // Si colisiona con un límite, regresa la nave a su posición previa
            Debug.Log("Colision con limite detectada.");
            Vector3 localOffset = new Vector3(0f, -1f, 0f);
            transform.position = transform.TransformPoint(localOffset);
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180f);
            _rb.velocity = Vector2.zero; // Detén el movimiento
        }
        /*if ((collision.gameObject.CompareTag("asteroide"))){
           
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector3 pushDirection = collision.transform.position - transform.position;
                rb.AddForce(pushDirection.normalized * 0.2f, ForceMode2D.Impulse);
            }
        }*/
    }
}
