using UnityEngine;

public class SmoothCameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform player; // Transform de la nave
    [SerializeField] private Vector3 offset;   // Desplazamiento respecto al jugador
    [SerializeField] private float smoothTime = 0.3f; // Tiempo de suavizado

    private Vector3 velocity = Vector3.zero; // Velocidad utilizada por SmoothDamp

    private void LateUpdate()
    {
        if (player == null) return;

        // Posición objetivo basada en el jugador y el offset
        Vector3 targetPosition = player.position + offset;

        // Suavizar la transición hacia la posición objetivo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Mantener la cámara en el plano 2D (ajuste en Z)
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }
}