using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene(1); 
    }


    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
        #else
            Application.Quit(); // Sale del juego en la versión compilada
        #endif
    }
}
