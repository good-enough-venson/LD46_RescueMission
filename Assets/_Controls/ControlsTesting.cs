using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsTesting : MonoBehaviour
{
    Controls controls;

    private void Awake()
    {
        controls = new Controls();

        controls.InGame_Actions.AimScreen.performed += (val) => {
            print(val.ReadValue<Vector2>());
        };
    }

    void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }
}
