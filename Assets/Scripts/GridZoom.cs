using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridDisplay))]
public class GridZoom : MonoBehaviour
{
    private Controls controls;

    public Camera Camera;

    public float Sensitivity = 1;

    public int MinCamSize = 10;

    public int MaxCamSize = 1000;

    public int DefaultCamSize = 50;

    private Transform bgGrid;

    private void Awake()
    {
        controls = InputManager.Instance.InputControls;

        controls.InGame.Scroll.performed += scroll_performed;

        bgGrid = GetComponent<GridDisplay>().BackgroundGrid;

        Camera.orthographicSize = DefaultCamSize;
    }

    private void scroll_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;

        var delta = ctx.ReadValue<float>();

        var newSize = Camera.orthographicSize + delta * PlatformData.Instance.PlatformScrollMultiplier * Sensitivity;

        newSize = Mathf.Clamp(newSize, MinCamSize, Mathf.Min(MaxCamSize, Mathf.Min(bgGrid.localScale.x / aspectRatio, bgGrid.localScale.y) / 2 ));

        Camera.orthographicSize = newSize;
    }
}
