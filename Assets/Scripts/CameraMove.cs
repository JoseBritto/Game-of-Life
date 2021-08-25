using UnityEngine;


public class CameraMove : MonoBehaviour
{
    private Controls controls;

    public float Speed = 2;

    private void Awake()
    {
        controls = InputManager.Instance.InputControls;
    }

   
    private void FixedUpdate()
    {
        var input = controls.InGame.Move.ReadValue<Vector2>().normalized;

        var movement = input * Time.fixedDeltaTime * Speed;
    
        transform.Translate(movement);
    }
}
