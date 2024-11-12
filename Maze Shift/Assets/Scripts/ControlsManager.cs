using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour, ISettings
{
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private bool invertY = false;
    [SerializeField] private Slider sensitivitySlider;

    public PlayerController playerController;
    public CameraController cameraController;
    public InputSystem_Actions playerInputActions;
    public PlayerInput playerInput;
    public TMP_Dropdown controlSchemeDropdown;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Enable();
        LoadControlSettings();
    }

    private void Start()
    {
        if(sensitivitySlider != null)
        {
            sensitivitySlider.value = sensitivity;
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
        PopulateControlSchemeDropdown();
        controlSchemeDropdown.onValueChanged.AddListener(OnControlSchemeChanged);
        SetDropdownToCurrentControlScheme();
    }

    private void PopulateControlSchemeDropdown()
    {
        controlSchemeDropdown.ClearOptions();
        var controlSchemes = playerInput.actions.controlSchemes;
        List<string> options = new List<string>();
        foreach(var scheme in controlSchemes)
        {
            options.Add(scheme.name);
        }
        controlSchemeDropdown.AddOptions(options);
    }

    public void SetDropdownToCurrentControlScheme()
    {
        string currentScheme = playerInput.currentControlScheme;
        int index = controlSchemeDropdown.options.FindIndex(option => option.text == currentScheme);
        if(index >= 0)
        {
            controlSchemeDropdown.value = index;
        }
    }

    private void OnControlSchemeChanged(int index)
    {
        string selectedScheme = controlSchemeDropdown.options[index].text;
        playerInput.SwitchCurrentControlScheme(selectedScheme);
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        cameraController.SetSensitivity(sensitivity);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.Save();
    }

    public void SetInvertYAxis(bool isInvertedY)
    {
        invertY = isInvertedY;
        cameraController.SetInvertY(invertY);
    }

    public void StartRebind(Button button, string actionName)
    {
        InputAction action = playerInputActions.FindAction(actionName);
        if (action == null) return;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Press a key...";
        action.Disable();

        action.PerformInteractiveRebinding().OnComplete(opertion => { 
            action.Enable();
            SaveBinding(action);
            button.GetComponentInChildren<TextMeshProUGUI>().text = action.GetBindingDisplayString();
            opertion.Dispose(); }).Start();
    }

    private void SaveBinding(InputAction action)
    {
        string rebinds = action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(action.name + "_rebinds", rebinds);
        PlayerPrefs.Save();
    }

    public void LoadControlSettings()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.0f);
        invertY = PlayerPrefs.GetInt("InvertY", invertY ? 1 : 0) == 1;

        SetSensitivity(sensitivity);
        SetInvertYAxis(invertY);
        if(sensitivitySlider != null) { sensitivitySlider.value = sensitivity; }
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
        cameraController.SetSensitivity(sensitivity);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        PlayerPrefs.SetInt("InvertY", invertY ? 1 : 0);

        foreach(InputAction action in playerInputActions)
        {
            SaveBinding(action);
        }
        PlayerPrefs.Save();
    }

    public void ResetToDefaults()
    {
        SetSensitivity(1.0f);
        SetInvertYAxis(false);
        foreach (InputAction action in playerInputActions)
        {
            action.RemoveAllBindingOverrides();
        }
        SaveSettings();
    }

    private void OnEnable() { playerInputActions.Enable(); }

    private void OnDisable() { playerInputActions.Disable(); }
}
