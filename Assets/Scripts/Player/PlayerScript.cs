using UnityEngine;

[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerScript : MonoBehaviour
{
    public PlayerCharacterController CharacterController;
    public float MoveSpeed = 6f;

    private Vector3 _velocity;
    private float _gravity = -20;

    public void Awake()
    {
        CharacterController = GetComponent<PlayerCharacterController>();
    }

    public void Update()
    {
        var input = InputManager.CurrentDirectionalInput();
        _velocity.x = input.x * MoveSpeed;
        _velocity.y += _gravity * Time.deltaTime;
        CharacterController.Move(_velocity * Time.deltaTime);
    }
}
