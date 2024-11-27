using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshTest : MonoBehaviour
{
    private NavMeshSurface surface;

    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
            Debug.Log("NavMeshSurface configurado correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró un componente NavMeshSurface.");
        }
    }
}