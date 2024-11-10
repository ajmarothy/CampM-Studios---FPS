using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour, ISettings
{
    [SerializeField] private float sensitivity = 1.0f;
    public PlayerController playerController;
    public CameraController cameraController;
    public InputSystem_Actions playerInputActions;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        cameraController = new CameraController();
        playerController = GameManager.instance.playerScript;
        playerInputActions.Enable();
        LoadControlSettings();
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        playerController.SetSensitivity(sensitivity);
    }

    public void UpdateCameraSensitivity(float newSensitivity)
    {
        cameraController.SetSensitivity(newSensitivity);
    }

    public void StartRebind(string actionName, Action onComplete = null)
    {
        InputAction action = playerInputActions.FindAction(actionName);
        if (action == null) return;

        action.Disable();

        action.PerformInteractiveRebinding().OnComplete(opertion => { 
            action.Enable();
            SaveBinding(action);
            onComplete?.Invoke();
            opertion.Dispose(); }).Start();
    }

    private void SaveBinding(InputAction action)
    {
        string rebinds = action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(action.name + "_rebinds", rebinds);
        PlayerPrefs.Save();
    }

    private void LoadControlSettings()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        foreach(InputAction action in playerInputActions)
        {
            string rebinds = PlayerPrefs.GetString(action.name + "_rebinds", string.Empty);
            if (!string.IsNullOrEmpty(rebinds))
            {
                action.LoadBindingOverridesFromJson(rebinds);
            }
        }
        ApplySettings();
    }

    public void ApplySettings() 
    { 
        playerController.SetSensitivity(sensitivity); 
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        foreach(InputAction action in playerInputActions)
        {
            SaveBinding(action);
        }
        PlayerPrefs.Save();
    }

    public void ResetToDefaults()
    {
        SetSensitivity(1.0f);
        foreach (InputAction action in playerInputActions)
        {
            action.RemoveAllBindingOverrides();
        }
        SaveSettings();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
}
