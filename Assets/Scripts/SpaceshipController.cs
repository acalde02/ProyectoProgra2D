using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [Header("Player Settings")]
    [Range(50f, 2000f)]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 movement = MovementLogic();
        _rb.velocity = movement;
        RotateTowardsDirection(movement);
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

}