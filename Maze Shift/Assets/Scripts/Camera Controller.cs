using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivity = 1.0f;
    [SerializeField] float lockVertMin = -90f;
    [SerializeField] float lockVertMax = 90f;
    [SerializeField] bool invertY = false;
    [SerializeField] Toggle invertYToggle;

    private InputSystem_Actions inputActions;
    float rotX;
    float recoilOffsetY;

    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseY = lookInput.y * sensitivity * Time.deltaTime * (invertY ? 1 : -1);
        float mouseX = lookInput.x * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX + mouseY, lockVertMin, lockVertMax);

        recoilOffsetY = Mathf.Lerp(recoilOffsetY, 0, Time.deltaTime / 0.5f);

        transform.localRotation = Quaternion.Euler(rotX + recoilOffsetY, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

    public void SetInvertY(bool isInverted) { invertY = isInverted; }

    public void ApplyRecoil(float recoilAmount) { recoilOffsetY -= recoilAmount; }

    private void OnEnable() { inputActions.Enable(); }

    private void OnDisable() { inputActions.Disable(); }
}
