using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private bool _isFiring;

    private TestInpuActions _inputActions;

    //Aquí ponemos todas las acciones puntuales que vaya a usar nuestra aplicación
    public Action JumpPerformed,
        FirePerformed,
        PausePerformed,
        UnPausePerformed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _inputActions = new TestInpuActions();
        _inputActions.Player.Enable();
    }

    private void Start()
    {
        _inputActions.Player.Jump.performed += JumpOnPerformed;
        _inputActions.Player.Fire.started += FireOnStarted;
        _inputActions.Player.Fire.canceled += FireOnCanceled;
        _inputActions.Player.Pause.performed += PauseOnPerformed;
        _inputActions.UI.UnPause.performed += UnPauseOnPerformed;
    }

    private void PauseOnPerformed(InputAction.CallbackContext obj)
    {
        PausePerformed?.Invoke();
        SwitchPlayerToUI();
    }

    private void UnPauseOnPerformed(InputAction.CallbackContext obj)
    {
        UnPausePerformed?.Invoke();
        SwitchUIToPlayer();
    }

    private void FireOnStarted(InputAction.CallbackContext obj)
    {
        FirePerformed?.Invoke();
        _isFiring = true;
    }
    
    private void FireOnCanceled(InputAction.CallbackContext obj)
    {
        FirePerformed = null;
        _isFiring = false;
    }

    private void JumpOnPerformed(InputAction.CallbackContext obj)
    {
        JumpPerformed?.Invoke();
    }

    public float GetHorizontalMovement()
    {
        return _inputActions.Player.HorizontalMove.ReadValue<float>();
    }
    
    public float GetVerticalMovement()
    {
        return _inputActions.Player.VerticalMove.ReadValue<float>();
    }

    public void SwitchUIToPlayer()
    {
        _inputActions.UI.Disable();
        _inputActions.Player.Enable();
    }

    private void SwitchPlayerToUI()
    {
        _inputActions.Player.Disable();
        _inputActions.UI.Enable();
    }
    
    
    public void DisableAllInputs()
    {
        Debug.Log("Disabling all inputs.");
        _inputActions.Player.Disable();
        _inputActions.UI.Disable();
    }
    
    public bool IsFiring()
    {
        return _isFiring;
    }
}
