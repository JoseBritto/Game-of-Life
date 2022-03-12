using UnityEngine;


public class CameraMove : MonoBehaviour
{
    private Controls controls;

    public float Speed = 2;

    Vector3 startPos;
    public Transform Background;

    private new Camera camera;

    private void Awake()
    {
        controls = InputManager.Instance.InputControls;
        camera = GetComponent<Camera>();
    }

   
    private void FixedUpdate()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        var input = controls.InGame.Move.ReadValue<Vector2>().normalized;

        if (input == Vector2.zero)
            return;

        if (startPos == default)
            startPos = transform.position;

        var movement = Speed * Time.fixedDeltaTime * input;

        transform.Translate(movement);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, camera.orthographicSize * aspectRatio, Background.position.x * 2 - (camera.orthographicSize * aspectRatio)), 
            Mathf.Clamp(transform.position.y, camera.orthographicSize, Background.position.y * 2 - camera.orthographicSize), 
            transform.position.z);
        
    }
}
