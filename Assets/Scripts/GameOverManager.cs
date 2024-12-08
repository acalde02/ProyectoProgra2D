using System.Collections; // Necesario para IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para manejar UI
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private float restartDelay = 5f; // Tiempo antes del reinicio en segundos
    [SerializeField] private TextMeshProUGUI countdownText; // Referencia al texto del Canvas

    private void Start()
    {
        // Inicia la Coroutine para el contador
        StartCoroutine(RestartGameAfterDelay());
    }

    private IEnumerator RestartGameAfterDelay()
    {
        float timeLeft = restartDelay;

        while (timeLeft > 0)
        {
            // Actualiza el texto en la pantalla
            countdownText.text = $"Reiniciando en {Mathf.Ceil(timeLeft)} segundos...";
            yield return new WaitForSeconds(1f); // Espera 1 segundo
            timeLeft -= 1f; // Resta un segundo
        }

        // Reinicia el juego cuando el tiempo llegue a 0
        RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(1); // Recarga la escena actual
    }
}