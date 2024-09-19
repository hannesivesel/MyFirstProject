using System;
using UnityEngine;

public class InputActions : MonoBehaviour
{

    private InputSystem_Actions _inputSystem;
    
    public float Horizontal;
    public bool Jump;


    private void Update()
    {
        Horizontal = _inputSystem.Player.Move.ReadValue<Vector2>().x;
        Jump = _inputSystem.Player.Jump.WasPressedThisFrame();
    }

    private void Awake() { _inputSystem = new InputSystem_Actions(); }

    private void OnEnable() { _inputSystem.Enable(); }

    private void OnDisable() { _inputSystem.Disable(); }
    
}

// public variabler : liten forbokstav
// private variabler : starter med _