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
            GameObject limite = collision.gameObject;
            if(limite.name == "limite_abajo"){
                transform.position = new Vector2(transform.position.x, 15.25f);
            }
            if (limite.name == "limite_arriba")
            {
                transform.position = new Vector2(transform.position.x, -10f);
            }
            if (limite.name == "limite_derecho")
            {
                transform.position = new Vector2(20f, transform.position.y);
            }
            if (limite.name == "limite_derecho")
            {
                transform.position = new Vector2(-18f, transform.position.y);
            }


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
