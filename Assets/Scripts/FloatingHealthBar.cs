using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Camera camera;
    [SerializeField] public Transform target;
    [SerializeField] Vector2 offset;
    
    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // Assign the main camera if it's not already assigned
        if (camera == null)
        {
            camera = Camera.main; // Find the main camera in the scene
            if (camera == null)
            {
                Debug.LogError("No Main Camera found! Please assign a camera to the FloatingHealthBar script.");
            }
        }
    }

    
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + (Vector3)offset;
    }
}
