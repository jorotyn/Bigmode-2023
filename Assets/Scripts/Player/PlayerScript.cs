using UnityEngine;

[RequireComponent(typeof(PlayerCharacterController))]
public class PlayerScript : MonoBehaviour
{
    public PlayerCharacterController CharacterController;
    public float JumpHeight = 3.5f;
    public float JumpTimeToApex = .4f;
    public float MoveSpeed = 6f;
    public float AccelerationTimeAir = .2f;
    public float AccelerationTimeGround = .1f;

    private float _jumpVelocity = 8f;
    private float _gravity = -20;

    private Vector3 _velocity;

    private float _velocity_x_smoothing;// don't touch this, it's for SmoothDamp to keep track of

    public void Start()
    {
        CharacterController = GetComponent<PlayerCharacterController>();
        _gravity = -(2 * JumpHeight) / Mathf.Pow(JumpTimeToApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * JumpTimeToApex;
    }

    public void Update()
    {
        if (CharacterController.CurrentCollisions.Above || CharacterController.CurrentCollisions.Below)
        {
            _velocity.y = 0;
        }

        var input = InputManager.CurrentDirectionalInput();
        if (InputManager.JumpPressed() && CharacterController.CurrentCollisions.Below)
        {
            _velocity.y = _jumpVelocity;
        }

        float targetX = input.x * MoveSpeed;
        float accelerationTime = CharacterController.CurrentCollisions.Below ? AccelerationTimeGround : AccelerationTimeAir;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetX, ref _velocity_x_smoothing, accelerationTime);
        _velocity.y += _gravity * Time.deltaTime;
        CharacterController.Move(_velocity * Time.deltaTime);
    }
}
